using Data;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.U2D;

public abstract class Spotter : MonoBehaviour, ISoundSource
{
    protected LightMovement _patrolMovement;

    [Header("Spotter Vision Variables")]
    [SerializeField, Range(0, 180), Tooltip("L'angle de vue de l'objet.")]
    protected float _visionAngle;

    [SerializeField, Range(0, 25), Tooltip("La distance de vue de l'objet.")]
    protected float _visionDistance;

    [SerializeField, Range(0, 30), Tooltip("Le niveau de details du cone de vision.")]
    private int _visionConeDetail;

    [SerializeField, Tooltip("Le sprite shape de l'objet.")]
    private SpriteShapeController _spriteShapeController;

    [Header("Spotter Spotting Variables")]
    [SerializeField, Range(0, 10), Tooltip("Le temps que mets l'objet a reperer le joueur quand il entre dans le cone de vision.")]
    private float _timeToSpot;

    [SerializeField, Tooltip("Le prefab du warning meter.")]
    private GameObject _warningMeterPrefab;

    [SerializeField, Range(0, 2), Tooltip("La distance par rapport a l'objet de spawn du warning meter.")]
    private float _warningMeterSpawnDistance;

    [SerializeField, Tooltip("Pour l'animation de stretch and squash. Represente la valeur de y au cours du temps.")]
    private AnimationCurve _spotScaleAnimation;

    [Header("Spotter Sounds")]
    [SerializeField]
    private Sound[] _sounds;
    public Sound[] Sounds => _sounds;

    private Spline _spriteShapeSpline; // Sert a modifier le sprite du cone de vision
    private RaycastHit2D[] _raycastHits; // Stock les infos des raycasts
    private float _spotMeter; // 0 a 1, si ca atteint 1, le joueur est spotted
    protected bool _playerInSight;
    private GameObject _warningMeter;
    private Transform _warningMeterFill;
    protected Vector2 _playerPosition;
    //private Transform _gfxTransform;
    private SpriteShapeRenderer _spriteShapeRenderer;
    private Color _spriteShapeIniColor;
    private AudioSource _audioSource;
    private Sound _previousSound;
    private static bool _playerAlreadyLost;

    public AudioSource AudioSource => _audioSource;

    private bool canTakeDamage = true;
    public float damageDelay = 0.5f;

    //====================================================================================================
    public void Awake()
    {
        if (!TryGetComponent(out _patrolMovement))
            throw new System.Exception("Pas de PatrolMovement dans " + name);

        _playerAlreadyLost = false;
    }

    public void Start()
    {
        _spriteShapeController.transform.localScale = Vector3.one;

        //_gfxTransform = transform.GetChild(0);

        _spriteShapeRenderer = _spriteShapeController.GetComponent<SpriteShapeRenderer>();
        _spriteShapeIniColor = _spriteShapeRenderer.color;

        _audioSource = GetComponent<AudioSource>();
    }

    public void FixedUpdate()
    {
        //Debug.Log(_playerInSight);

        ConeRaycasts(
            numberOfCasts: 2 + _visionConeDetail,
            angle: _visionAngle,
            distance: _visionDistance,
            hits: out _raycastHits
            );

        if (_raycastHits == null)
            return;

        DrawVisionSprite();

        if (!_playerInSight)
            _spotMeter = Mathf.Clamp01(_spotMeter - (Time.deltaTime / (_timeToSpot * 2)));

        if (_warningMeter)
        {
            if (_spotMeter <= 0)
            {
                _warningMeterFill = null;
                Destroy(_warningMeter);
            }
            else
                _warningMeterFill.localScale = new Vector3(_warningMeter.transform.localScale.x * _spotMeter, 1, 1);

            _warningMeter.transform.position = transform.position + _warningMeterSpawnDistance * Vector3.up;
        }

    }

    /// <summary>
    /// Lance des raycast en forme de cone autour de la position de l'objet.
    /// </summary>
    /// <param name="numberOfCasts">Le nombre de rayons a tirer. Au mininum deux sont necessaires.</param>
    /// <param name="angle">L'angle entre les raycasts situes aux extremites.</param>
    /// <param name="distance">La longueur des raycasts.</param>
    /// <param name="hits">Le tableau de RaycastsHits2D genere.</param>
    private void ConeRaycasts(int numberOfCasts, float angle, float distance, out RaycastHit2D[] hits)
    {
        if (numberOfCasts < 2)
        {
            hits = null;
            return;
        }

        hits = new RaycastHit2D[numberOfCasts];
        for (int i = 0; i < numberOfCasts; i++)
        {
            float alpha = i * (angle / (numberOfCasts - 1)) - angle * .5f;

            hits[i] = Physics2D.Raycast(
                origin: transform.position,
                direction: Quaternion.Euler(0, 0, alpha) * transform.right,
                distance: distance,
                //layerMask: ~LayerMask.GetMask("Player", "PlayerHidden", "Items", "Default")
                layerMask: LayerMask.GetMask("Ground")
                ) ;

            if (!hits[i])
                hits[i].point = transform.position + distance * (Quaternion.Euler(0, 0, alpha) * transform.right);
        }
    }

    /// <summary>
    /// Dessine le sprite du cone de vision a l'ecran. Il sert a detecter le joueur.
    /// </summary>
    private void DrawVisionSprite()
    {
        _spriteShapeSpline = _spriteShapeController.spline;

        // Si on a modifie le niveau de detail du cone, on cree un nouveau sprite. Sinon, on deplace juste ses points.
        if (_spriteShapeSpline.GetPointCount() != _raycastHits.Length + 1)
        {
            // On supprime tous les points du spline
            _spriteShapeSpline.Clear();

            // On crée tous les points dont on a besoin pour le sprite
            _spriteShapeSpline.InsertPointAt(0, Vector2.zero);
            _spriteShapeSpline.SetHeight(0, 0);
            for (int i = 1; i <= _raycastHits.Length; i++)
            {
                _spriteShapeSpline.InsertPointAt(i, transform.InverseTransformPoint(_raycastHits[i - 1].point));
                _spriteShapeSpline.SetHeight(i, 0);
            }
        }
        else
        {
            for (int j = 0; j < _spriteShapeSpline.GetPointCount(); j++)
            {
                if (j == 0)
                    _spriteShapeSpline.SetPosition(j, Vector2.zero);
                else
                    _spriteShapeSpline.SetPosition(j, transform.InverseTransformPoint(_raycastHits[j - 1].point));
            }
        }

        // Change la couleur du sprite
        if (_playerInSight && _spriteShapeRenderer.color != Color.red)
        {
            _spriteShapeRenderer.color = new Color(1, 0, 0, _spriteShapeIniColor.a); // Rouge
            
        }  
        else if (!_playerInSight && _spriteShapeRenderer.color != _spriteShapeIniColor)
            _spriteShapeRenderer.color = _spriteShapeIniColor;
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

        if (canTakeDamage)
        {
            PlayerHealthController.GetInstance().DealDamageWithoutKnockback();
            StartCoroutine(WaitForDamage());
        }

        if (!_warningMeter)
        {
            _warningMeter = Instantiate(_warningMeterPrefab, transform.position + _warningMeterSpawnDistance * Vector3.up, Quaternion.identity);
            _warningMeterFill = _warningMeter.transform.GetChild(0);
            PlaySound("Alert");
            StartCoroutine(SpotPlayerAnimation());
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

    private IEnumerator SpotPlayerAnimation()
    {
        float t = 0;
        //Vector3 iniScale = _gfxTransform.localScale;

        while (t < 1)
        {
            //_gfxTransform.localScale = new Vector3(iniScale.y / _gfxTransform.localScale.y, _spotScaleAnimation.Evaluate(t) * iniScale.y, 1);
            t += Time.fixedDeltaTime * 3;
            yield return new WaitForFixedUpdate();
        }

        //_gfxTransform.localScale = iniScale;
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


#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
            return;

        Vector2 alphaDir = _visionDistance * (Quaternion.Euler(0, 0, _visionAngle * .5f) * transform.right);
        Vector2 betaDir = _visionDistance * (Quaternion.Euler(0, 0, -_visionAngle * .5f) * transform.right);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, alphaDir);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, betaDir);
    }
#endif
}
