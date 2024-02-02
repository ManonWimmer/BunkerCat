using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHeadDestroy : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    public GameObject enemyToDestroy;
    // ----- VARIABLES ----- //

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Le joueur entre dans le trigger
        {
            Destroy(enemyToDestroy);
        }
    }
}
