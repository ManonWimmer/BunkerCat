using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    // ----- VARIABLES ----- //
    public GameObject[] waypoints; // Liste des waypoints que le gameObject doit "suivre" / s'y rendre 
    public int currentWaypointIndex = 0;

    public float speed; // Vitesse de d�placement 
    public bool canMove = true;
    // ----- VARIABLES ----- //

    void Update()
    {
        if (canMove)
        {
            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f) // On change de waypoint
            {
                currentWaypointIndex++; // On augmente l'index du waypoint vers lequel on se dirige
                if (currentWaypointIndex >= waypoints.Length) // Si on atteint le dernier waypoint de la liste
                {
                    currentWaypointIndex = 0; // On recommence le parcours de la liste du d�but
                }
            }

            if (transform.CompareTag("Platforms") || transform.CompareTag("Light"))
            { // Si c'est une plateforme, peut se d�placer
                transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed); // D�placement
            }

            /*
            if (transform.CompareTag("Enemies"))
            { // Si c'est un ennemi
              //Debug.Log("ici");
              //Debug.Log(transform.GetComponent<Enemy>().canMove);
                if (transform.GetComponent<Enemy>().canMove) // D�placement
                {
                    transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed); // D�placement

                }
            }
            */
        }
        
    }
}
