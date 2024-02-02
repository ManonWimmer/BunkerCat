using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // Création du checkpoint :
    public SpriteRenderer sr; // Récup du SpriteRenderer du checkpoint
    public Sprite cpOn, cpOff; // Sprites avec le checkpoint ON ou OFF
    // ----- VARIABLES ----- //

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Création du checkpoint :
    private void OnTriggerEnter2D(Collider2D other) // Dès qu'un élément entre dans le trigger du checkpoint
    {
        if(other.CompareTag("Player")) // Un peu mieux que other.tag == "Player", on vérifie que ce soit le joueur qui entre le trigger du checkpoint
        {
            CheckpointController.instance.DeactivateCheckpoints(); // On met tous les chackpoints de la scène à OFF

            sr.sprite = cpOn; // Le checkpoint passe de OFF à ON

            // Stocker position de spawn :
            CheckpointController.instance.SetSpawnPoint(transform.position); // Le spawnPoint prend la position du checkpoint qui vient d'être activé
        }
    }

    // Désactivation des checkpoints précédents :
    public void ResetCheckpoint()
    {
        sr.sprite = cpOff; // On désactive le checkpoint
    }

}
