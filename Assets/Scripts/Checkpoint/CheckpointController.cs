using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // Instance :
    public static CheckpointController instance; // Instance

    // Désactivation des checkpoints précédents :
    private Checkpoint[] checkpoints; // Array avec tous les checkpoints de la scène

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
        // On cherche tous les checkpoints ACTIFS (pas masqués) de la scène pour les mettre dans la liste :
        checkpoints = FindObjectsOfType<Checkpoint>();

        // Spawn / Respawn point (sans checkpoint activé) :
        spawnPoint = playerController.transform.position;
    }


    void Update()
    {
        
    }

    // Désactivation de tous les checkpoints :
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

