using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPlatform : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private SpriteRenderer spriteRendererGround;

    [SerializeField]
    private SpriteRenderer spriteRendererGrass;

    private Sprite initialGround;

    private Sprite initialGrass;

    [SerializeField]
    private Sprite lightGround;

    [SerializeField]
    private Sprite lightGrass;

    public bool isLighting;
    public float timeBeforeStartLight = 0.2f;
    public float timeBeforeStopLight = 3f;

    private bool isPlayerOnPlatform;
    private bool coroutineStartPlaying = false;
    private bool coroutineStopPlaying = false;
    // ----- VARIABLES ----- //

    private void Start()
    {
        initialGround = spriteRendererGround.sprite;
        initialGrass = spriteRendererGrass.sprite;
    }

    private void Update()
    {
        if (isPlayerOnPlatform && !coroutineStartPlaying)
        {
            StartCoroutine(StartLight());
        }
        else if (!isPlayerOnPlatform && !coroutineStopPlaying && spriteRendererGround.sprite != initialGround)
        {
            StartCoroutine(StopLight());
        }

    }

    private IEnumerator StartLight()
    {
        coroutineStartPlaying = true;
        // Shake ?
        yield return new WaitForSeconds(timeBeforeStartLight);
        isLighting = true;
        spriteRendererGround.sprite = lightGround;
        spriteRendererGrass.sprite = lightGrass;
        coroutineStartPlaying = false;
    }

    private IEnumerator StopLight()
    {
        Debug.Log("stop light");
        coroutineStopPlaying = true;
        // Shake ?
        yield return new WaitForSeconds(timeBeforeStopLight);
        isLighting = false;
        spriteRendererGround.sprite = initialGround;
        spriteRendererGrass.sprite = initialGrass;
        coroutineStopPlaying = false;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        isPlayerOnPlatform = true;
        Debug.Log("oui");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isPlayerOnPlatform = false;
        Debug.Log("non");
    }

}
