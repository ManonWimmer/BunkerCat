using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class LightMovement : MonoBehaviour
{
    [SerializeField, Tooltip("Le type d'ennemi.")]
    private PATROL_TYPE _patrolType;

    [SerializeField, Range(0, 10), Tooltip("Le temps a attendre apres le lancement de la scene avant de commencer la routine de mouvement.")]
    private float _timeBeforeRoutine;

    [SerializeField, Range(0, 10), Tooltip("Le temps a attendre entre deux mouvements")]
    private float _waitTimeBetweenMoves;

    private Spotter _manager;
    private bool _canMove;
    private Coroutine _waitCoroutine;
    //private MoveDelegate _move;

    //==================================================================================
    private IEnumerator Start()
    {
        if (!TryGetComponent(out _manager))
            throw new System.Exception("Pas de script manager dans l'objet.");

        /*
        switch (_patrolType)
        {
            case PATROL_TYPE.CAMERA:
                _move = CCTVMove;
                gameObject.tag = "CCTV";
                break;

            default:
                throw new System.Exception($"{name} is of type NONE. Please choose a type for {name}");
        }*/

        gameObject.tag = "CCTV";

        yield return new WaitForSeconds(_timeBeforeRoutine);
        _canMove = true;
    }

    /// <summary>
    /// Attends avant d'autoriser l'objet de bouger a nouveau.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_waitTimeBetweenMoves);

        _canMove = true;
    }

    /// <summary>
    /// Arrete l'objet et enclenche une pause dans sa routine.
    /// </summary>
    public void StopMoving()
    {
        _canMove = false;

        if (_waitCoroutine != null)
        {
            StopCoroutine(_waitCoroutine);
            _waitCoroutine = StartCoroutine(Wait());
        }
        else
            _waitCoroutine = StartCoroutine(Wait());
    }

    /// <summary>
    /// Fait bouger l'objet s'il peut bouger.
    /// </summary>
    public void Move()
    {
        if (!_canMove)
            return;

        CCTVMove();
    }

    /// <summary>
    /// Tourne la CCTV de gauche a droite.
    /// </summary>
    private void CCTVMove()
    {
        LightManager cctv = _manager as LightManager;

        if (!cctv)
            return;

        float moveTime = cctv.DistanceMinMax / (cctv.AngularSpeed * Mathf.Deg2Rad);
        transform.localRotation = Quaternion.Lerp(cctv.LastRotation, cctv.TargetRotation, cctv._deltaT);
        cctv._deltaT += Time.fixedDeltaTime / moveTime;

        _manager.PlaySound("Move");

        if (cctv._deltaT >= 1)
        {
            transform.rotation = cctv.TargetRotation;
            cctv.StopSound();
            StopMoving();
            cctv.ChangeDirection();
            cctv._deltaT = 0;
        }
    }
}

public enum PATROL_TYPE
{
    NONE,
    CAMERA
}