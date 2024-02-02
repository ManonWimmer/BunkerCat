using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // Instance :
    public static LevelManager instance; // Instance

    // Respawn le joueur quand il meurt :
    public float waitToRespawn; // Temps avant de respawn, pour que le joueur comprenne qu'il est mort et à cause de quoi

    private PlayerController playerController;
    // ----- VARIABLES ----- //

    private void Awake()
    {
        instance = this; // Initialisation de l'instance
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

 

    // Respawn le joueur quand il meurt :
    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo()); // Appel à la coroutine
    }

    // Co = Corountine : delay des fonctions ou attendre que d'autres se finissent avant d'en commencer d'autres... En dehors du cercle Update - Late Update, TriggerEnter... 
    private IEnumerator RespawnCo() 
    {
        playerController.gameObject.SetActive(false); // On désactive le joueur

        yield return new WaitForSeconds(waitToRespawn); // Attendre le délai de temps waitToRespawn

        playerController.gameObject.SetActive(true); // On réactive le joueur

        playerController.transform.position = CheckpointController.instance.spawnPoint; // On met le joueur à sa position de spawn

        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth; // On reset la sant
        UIController.instance.UpdateHealthDisplay(); // On update les coeurs de l'UI
    }
}
