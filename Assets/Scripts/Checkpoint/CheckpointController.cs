using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // Instance :
    public static CheckpointController instance; // Instance

    // D�sactivation des checkpoints pr�c�dents :
    private Checkpoint[] checkpoints; // Array avec tous les checkpoints de la sc�ne

    // Stocker position de spawn :
    public Vector3 spawnPoint; // Position de spawn

    private PlayerController playerController;
    // ----- VARIABLES ----- //

    private void Awake()
    {
        instance = this; // Initialisation de l'instance
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Start()
    {
        // On cherche tous les checkpoints ACTIFS (pas masqu�s) de la sc�ne pour les mettre dans la liste :
        checkpoints = FindObjectsOfType<Checkpoint>();

        // Spawn / Respawn point (sans checkpoint activ�) :
        spawnPoint = playerController.transform.position;
    }


    void Update()
    {
        
    }

    // D�sactivation de tous les checkpoints :
    public void DeactivateCheckpoints()
    {
        for (int i = 0; i < checkpoints.Length; i++) // Boucle pour chaque checkpoint de la liste
        {
            checkpoints[i].ResetCheckpoint(); // On reset chaque checkpoint
        }
    }

    // Stocker position de spawn :
    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint; // Update du spawnPoint
    }
}

