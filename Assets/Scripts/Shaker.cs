using System.Collections;
using System.Collections.Generic;
# if UNITY_EDITOR
using UnityEditorInternal;
# endif
using UnityEngine;

public class Shaker : MonoBehaviour
{
    // ----- VARIABLES ----- //
    Vector2 objectInitialPosition;
    public float shakeMagnetude = 0.05f;
    public float shakeTime = 0.5f;
    public static Shaker instance;
    // ----- VARIABLES ----- //
    private void Awake()
    {
        instance = this;
    }

    public static Shaker GetInstance()
    {
        return instance;
    }

    public void ShakeObject()
    {
        objectInitialPosition = transform.position;
        InvokeRepeating("StartObjectShaking", 0f, 0.005f);
        Invoke("StopObjectShaking", shakeTime);
    }

    private void StartObjectShaking()
    {
        float objectShakingOffsetX = Random.value * shakeMagnetude * 2 - shakeMagnetude;
        //float objectShakingOffsetY = Random.value * shakeMagnetude * 2 - shakeMagnetude;
        Vector2 objectIntermediatePosition = transform.position;
        objectIntermediatePosition.x += objectShakingOffsetX;
        //objectIntermediatePosition.y += objectShakingOffsetY;
        transform.position = objectIntermediatePosition;
    }

    private void StopObjectShaking()
    {
        CancelInvoke("StartObjectShaking");
        transform.position = objectInitialPosition;
    }
}
