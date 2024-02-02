using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayMovement : MonoBehaviour
{
    // ----- VARIABLES ----- //
    public GameObject waypointStart; 
    public GameObject waypointEnd;

    public float speed; // Vitesse de déplacement 
    public bool started = false;
    public bool finished = false;
    // ----- VARIABLES ----- //

    private void Start()
    {
        SetStartPosition();  // On le met au départ
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (started && !finished)
        {
            if (Vector2.Distance(waypointEnd.transform.position, transform.position) < .1f) // Finito
            {
                finished = true;
                gameObject.SetActive(false);
            }

            if (transform.CompareTag("Platforms") || transform.CompareTag("Light"))
            { // Si c'est une plateforme, peut se déplacer
                transform.position = Vector2.MoveTowards(transform.position, waypointEnd.transform.position, Time.deltaTime * speed); // Déplacement
                transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * speed, transform.localScale.y, transform.localScale.z);
            }

        }

    }

    private void SetStartPosition()
    {
        transform.position = waypointStart.transform.position;
    }

    public void StartMovement()
    {
        SetStartPosition();
        gameObject.SetActive(true);
        started = true;
        finished = false;
    }
}
