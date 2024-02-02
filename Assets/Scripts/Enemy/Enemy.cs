using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // ----- VARIABLES ----- //
    public bool canMove = true;
    // ----- VARIABLES ----- //

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Le joueur entre dans le trigger
        {
            PlayerHealthController.instance.DealDamage();
        }
    }
}
