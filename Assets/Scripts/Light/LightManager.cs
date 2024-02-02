using System.Collections;
using UnityEngine;
using Data;
using System.Collections.Generic;

public class LightManager : Spotter
{
    [SerializeField, Range(0, 179), Tooltip("Rotation max")]
    private float _rotationAngle;

    [SerializeField, Range(0, 180), Tooltip("Vitesse de rotation en degres par seconde.")]
    private float _angularSpeed;

    private Quaternion _maxRotation;
    private Quaternion _minRotation;
    private Quaternion _lastRotation;
    private Quaternion _targetRotation;
    private Coroutine _lookAtCoroutine;
    private float _distanceMinMax;
    [HideInInspector] public float _deltaT;

    public float AngularSpeed => _angularSpeed;
    public Quaternion MaxRotation => _maxRotation;
    public Quaternion MinRotation => _minRotation;
    public Quaternion LastRotation => _lastRotation;
    public Quaternion TargetRotation => _targetRotation;
    public float DistanceMinMax => _distanceMinMax;
    //================================================================================================
    private new void Awake()
    {
        base.Awake();

        _maxRotation = transform.localRotation * Quaternion.Euler(0, 0, _rotationAngle * .5f);
        _minRotation = transform.localRotation * Quaternion.Euler(0, 0, -_rotationAngle * .5f);

        Quaternion q = Quaternion.Inverse(_maxRotation) * _minRotation;
        _distanceMinMax = 2 * Mathf.Abs(Mathf.Atan2(new Vector3(q.x, q.y, q.z).magnitude, q.w));

        
    }

    private new void Start()
    {
        base.Start();

        transform.localRotation = _minRotation;
        _lastRotation = _minRotation;
        _targetRotation = _maxRotation;
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        if (_playerInSight)
        {
            //Debug.Log("playerInSight");
            if (_lookAtCoroutine == null)
                StartCoroutine(LookAtCoroutine(_playerPosition));

            _patrolMovement.StopMoving();
            return;
        }

        _patrolMovement.Move();
    }


    // Inverse la direction de rotation.
    public void ChangeDirection()
    {
        (_targetRotation, _lastRotation) = (_lastRotation, _targetRotation); // On inverse la direction de rotation
    }

    private IEnumerator LookAtCoroutine(Vector2 position) // Tourne la lumière vers la position en param
    {
        Vector2 objToPlayer = position - (Vector2)transform.position; // Distance objet - position à aller
        float alphaAngle = Vector2.SignedAngle(transform.right, objToPlayer); // L'angle entre le joueur et la direction de la camera
        float betaAngle; 
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation;

        if (alphaAngle > 0) // Si le joueur est a gauche
        {
            betaAngle = Vector2.SignedAngle(transform.right, _maxRotation * Vector2.right); // Angle signé = angle + rotation sens horaire (neg) ou anti-horaire (pos)

            if (alphaAngle >= betaAngle) // Si le joueur est hors des limites de rotation
                targetRotation = _maxRotation; // Au max
            else
                targetRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + alphaAngle); // On rotate d'alphaAngle
        }
        else if (alphaAngle < 0) // Si le joueur est a droite
        {
            betaAngle = Vector2.SignedAngle(transform.right, _minRotation * Vector2.right); // Pareil qu'au dessus

            if (alphaAngle <= betaAngle) // Si le joueur est hors des limites de rotation
                targetRotation = _minRotation;
            else
                targetRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + alphaAngle);
        }
        else // Si le joueur est tout droit
        {
            yield return new WaitUntil(() => _playerInSight == false);
            _lookAtCoroutine = null;
            yield break;
        }

        float t = 0;
        while (t < 1)
        {
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return new WaitForFixedUpdate();
            t += Time.deltaTime * 3;
        }

        Quaternion a = Quaternion.Inverse(_lastRotation) * _targetRotation;
        float distanceLastTarget = 2 * Mathf.Abs(Mathf.Atan2(new Vector3(a.x, a.y, a.z).magnitude, a.w));
        Quaternion b = Quaternion.Inverse(_lastRotation) * transform.localRotation;
        float distanceCurrentTarget = 2 * Mathf.Abs(Mathf.Atan2(new Vector3(b.x, b.y, b.z).magnitude, b.w));

        _deltaT = Mathf.InverseLerp(0, distanceLastTarget, distanceCurrentTarget);
        _lookAtCoroutine = null;
        yield break;
    }

#if UNITY_EDITOR
    private new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (Application.isPlaying)
            return;

        Vector2 alphaDir = 2 * (Quaternion.Euler(0, 0, _rotationAngle * .5f) * transform.right);
        Vector2 betaDir = 2 * (Quaternion.Euler(0, 0, -_rotationAngle * .5f) * transform.right);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, alphaDir);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, betaDir);
    }
#endif
}
