using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private float fallDelay = 1f;

    [SerializeField]
    private float respawnDelay = 5f;

    [SerializeField]
    private float timeBeforeStopping = 2f;

    public bool isEnabled = false;

    private Vector2 initialPosition;

    private Shaker shaker;

    [SerializeField] 
    private Rigidbody2D rb;
    // ----- VARIABLES ----- //

    private void Awake()
    {
        shaker = gameObject.GetComponent<Shaker>();
        shaker.shakeTime = fallDelay;
    }



    private void Start()
    {
        initialPosition = transform.localPosition;
        Debug.Log(initialPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        shaker.ShakeObject();
        yield return new WaitForSeconds(fallDelay); 
        rb.bodyType = RigidbodyType2D.Dynamic;
        // La plateforme ne bouge plus (légèrement en dessous de la map mais pas trop) :
        yield return new WaitForSeconds(timeBeforeStopping);
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        isEnabled = false;
        //transform.parent.gameObject.SetActive(false);
    }

    /*
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay - timeBeforeStopping);
        Debug.Log("respawn");
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }
    */

    public void SpawnPlatform()
    {
        //transform.parent.gameObject.SetActive(true);
        Debug.Log("spawn platform");
        if (transform.localPosition.y != initialPosition.y)
        {
            transform.localPosition = new Vector2(initialPosition.x, initialPosition.y);
        }

        Debug.Log(transform.localPosition);
        //gameObject.SetActive(true);
        isEnabled = true;
    }


}
