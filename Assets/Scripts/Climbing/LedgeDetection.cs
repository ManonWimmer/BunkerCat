using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private float radius;

    [SerializeField]
    private LayerMask ground;

    private bool canDetected;

    private Vector2 positionRight;
    private Vector2 positionLeft;

    private PlayerController playerController;
    // ----- VARIABLES ---- //

    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (canDetected)
        {
            playerController.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, ground);
        }
        else
        {
            playerController.ledgeDetected = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Détecte un mur dans le trigger
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("detected");
            canDetected = false; // Peut pas climb
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // Détecte pas de mur dans le trigger
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetected = true; // Peut climb si y'a un mur dans le rond raycast en dessous
        }
    }

}
