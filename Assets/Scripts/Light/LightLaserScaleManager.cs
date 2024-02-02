using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLaserScaleManager : MonoBehaviour
{
    [Header("Spotter Spotting Variables")]
    [SerializeField, Range(0, 10), Tooltip("Le temps que mets l'objet a reperer le joueur quand il entre dans le cone de vision.")]
    private float _timeToSpot;

    [SerializeField, Tooltip("Le prefab du warning meter.")]
    private GameObject _warningMeterPrefab;

    [SerializeField, Range(0, 3), Tooltip("La distance par rapport a l'objet de spawn du warning meter.")]
    private float _warningMeterSpawnDistance;

    [Header("Spotter Sounds")]
    [SerializeField]
    private Sound[] _sounds;
    public Sound[] Sounds => _sounds;

    private float _spotMeter; // 0 a 1, si ca atteint 1, le joueur est spotted
    protected bool _playerInSight;
    private GameObject _warningMeter;
    private Transform _warningMeterFill;
    protected Vector2 _playerPosition;
    private SpriteRenderer _spriteRenderer;
    private Color _spriteShapeColor;
    private AudioSource _audioSource;
    private Sound _previousSound;
    private static bool _playerAlreadyLost;

    public AudioSource AudioSource => _audioSource;

    public GameObject player;

    private Scaler scaler;

    private Coroutine _lookAtCoroutine;

    private bool canTakeDamage = true;
    public float damageDelay = 0.5f;

    private Collider2D _collider;
    private List<Collider2D> _overlappedColliders;
    private ContactFilter2D _contactFilter;


    //====================================================================================================
    public void Awake()
    {
        _playerAlreadyLost = false;

        _collider = GetComponent<Collider2D>();
        _overlappedColliders = new List<Collider2D>();

        _contactFilter = new();
        _contactFilter.SetLayerMask(LayerMask.GetMask("Player"));
        _contactFilter.ClearDepth();
        _contactFilter.ClearNormalAngle();
    }

    public void Start()
    {

        //_gfxTransform = transform.GetChild(0);

        //_spriteShapeRenderer = _spriteShapeController.GetComponent<SpriteShapeRenderer>();


        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteShapeColor = _spriteRenderer.color;

        _audioSource = GetComponent<AudioSource>();

        scaler = gameObject.transform.parent.GetComponent<Scaler>();

    }

    public void FixedUpdate()
    {
        //Debug.Log(_playerInSight);

        CheckColor();

        int count = Physics2D.OverlapCollider(_collider, _contactFilter, _overlappedColliders);
        if (count > 0)
        {
            _playerInSight = true;
        }
        else if (_playerInSight)
        {
            _playerInSight = false;
        }

        if (_playerInSight)
        {
            PlayerInSight(player.transform.position);
        }

        if (!_playerInSight)
        {
            PlayerOutOfSight();
            _spotMeter = Mathf.Clamp01(_spotMeter - (Time.deltaTime / (_timeToSpot * 2)));
        }

        if (_warningMeter)
        {
            if (_spotMeter <= 0 || scaler.finished)
            {
                _warningMeterFill = null;
                Destroy(_warningMeter);
            }
            else
                _warningMeterFill.localScale = new Vector3(_warningMeter.transform.localScale.x * _spotMeter, 1, 1);

            _warningMeter.transform.position = transform.position + _warningMeterSpawnDistance * Vector3.up;
        }
        /*
        if (_playerInSight)
        {
            //Debug.Log("playerInSight");
            if (_lookAtCoroutine == null)
                StartCoroutine(LookAtCoroutine(_playerPosition));

            //waypointFollower.canMove = false; //
            return;
        }*/

        //waypointFollower.canMove = true;

    }

    private void CheckColor()
    {

        // Change la couleur du sprite
        if (_playerInSight && _spriteRenderer.color != Color.red)
        {
            _spriteRenderer.color = new Color(1, 0, 0, _spriteShapeColor.a);
            if (canTakeDamage)
            {
                PlayerHealthController.GetInstance().DealDamageWithoutKnockback();
                StartCoroutine(WaitForDamage());
            }
        }
            
        else if (!_playerInSight && _spriteRenderer.color != _spriteShapeColor)
            _spriteRenderer.color = _spriteShapeColor;
    }

    private IEnumerator WaitForDamage()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageDelay); // On attend avant que le joueur puisse reprendre des degats
        canTakeDamage = true;
    }

    /// <summary>
    /// Stock la position du joueur s'il entre dans le cone de vision et fais monter le spotMeter
    /// </summary>
    /// <param name="playerPosition">Position du joueur</param>
    public void PlayerInSight(Vector2 playerPosition)
    {
        _playerInSight = true;
        _playerPosition = playerPosition;

        _spotMeter = Mathf.Clamp01(_spotMeter + (Time.deltaTime / _timeToSpot));

        if (!_warningMeter)
        {
            _warningMeter = Instantiate(_warningMeterPrefab, transform.position + _warningMeterSpawnDistance * Vector3.up, Quaternion.identity);
            _warningMeterFill = _warningMeter.transform.GetChild(0);
            PlaySound("Alert");
        }
        else
            PlaySound("Spotting");

        if (_spotMeter >= 1)
            PlayerSpotted();
    }

    /// <summary>
    /// Fais comprendre a l'objet que le joueur a quitté le champ de vision.
    /// </summary>
    public void PlayerOutOfSight()
    {
        _playerInSight = false;
    }

    /// <summary>
    /// L'objet est maintenant certain d'avoir vu le joueur. C'est une game over.
    /// </summary>
    private void PlayerSpotted()
    {
        if (_playerAlreadyLost)
            return;

        DefeatMenu.GetInstance().Show();
        /*
        if (GameManager.Instance)
            GameManager.Instance.PlayerLost();
        else
            Debug.Log("Tu as perdu mais y'a pas de GameManager. Veille à démarrer le jeu depuis le menu principal.");
        */
        _playerAlreadyLost = true;
    }

    public void PlaySound(string name)
    {
        Sound sound;

        if (_previousSound.name != name)
            sound = System.Array.Find(_sounds, s => s.name == name);
        else
            sound = _previousSound;

        if (sound.name == null)
            return;

        if (_audioSource.clip == sound.clip && _audioSource.isPlaying)
            return;

        if (name == "Spotting" && _previousSound.name == "Alert" && _audioSource.isPlaying)
            return;

        _audioSource.clip = sound.clip;
        _audioSource.volume = sound.volume;
        _audioSource.loop = sound.loop;
        _audioSource.pitch = Random.Range(0.8f, 1.2f);

        _audioSource.Play();
        _previousSound = sound;
    }

    public void StopSound()
    {
        _audioSource.Stop();
        _audioSource.clip = null;
    }

    /*
    /// <summary>
    /// Une coroutine qui va tourner la cctv vers la position donn?e.
    /// </summary>
    /// <param name="position"> La position de l'objet vers lequel on souhaite se tourner. </param>
    private IEnumerator LookAtCoroutine(Vector2 position) //
    {

        float playerY = player.transform.position.y;
        float laserY = transform.position.y;


        if (playerY > laserY) // Si le joueur est en haut
        {
            waypointFollower.currentWaypointIndex = 0;
        }
        else if (playerY < laserY) // Si le joueur en bas
        {
            waypointFollower.currentWaypointIndex = 1;
        }
        else // Si le joueur est a la meme hauteur
        {
            waypointFollower.canMove = false;
            yield return new WaitUntil(() => _playerInSight == false);
            _lookAtCoroutine = null;
            yield break;
        }

        _lookAtCoroutine = null;

        yield break;
    }*/

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) // pas le compareTag("Player") sinon on peut le détecter meme quand il est hidden dans une crate
        {
            Debug.Log("player in sight");
            PlayerInSight(player.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("player not in sight");
            PlayerOutOfSight();
        }
    }*/
}
