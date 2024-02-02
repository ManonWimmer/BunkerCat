using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // Cr�ation du checkpoint :
    public SpriteRenderer sr; // R�cup du SpriteRenderer du checkpoint
    public Sprite cpOn, cpOff; // Sprites avec le checkpoint ON ou OFF
    // ----- VARIABLES ----- //

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Cr�ation du checkpoint :
    private void OnTriggerEnter2D(Collider2D other) // D�s qu'un �l�ment entre dans le trigger du checkpoint
    {
        if(other.CompareTag("Player")) // Un peu mieux que other.tag == "Player", on v�rifie que ce soit le joueur qui entre le trigger du checkpoint
        {
            CheckpointController.instance.DeactivateCheckpoints(); // On met tous les chackpoints de la sc�ne � OFF

            sr.sprite = cpOn; // Le checkpoint passe de OFF � ON

            // Stocker position de spawn :
            CheckpointController.instance.SetSpawnPoint(transform.position); // Le spawnPoint prend la position du checkpoint qui vient d'�tre activ�
        }
    }

    // D�sactivation des checkpoints pr�c�dents :
    public void ResetCheckpoint()
    {
        sr.sprite = cpOff; // On d�sactive le checkpoint
    }

}
