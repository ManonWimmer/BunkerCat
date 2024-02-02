using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Si le joueur est sur la plateforme
        {
            other.gameObject.transform.SetParent(transform); // On met le joueur en enfant de la plateforme pour qu'il suive ses mouvements
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Si le joueur quitte la plateforme
        {
            other.gameObject.transform.SetParent(null); // Le joueur n'est plus un enfant de la plateforme, ne suit plus ses mouvements
        }
    }
}
