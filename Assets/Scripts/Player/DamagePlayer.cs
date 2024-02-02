using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // Solution invincibilité :
    public float countdownTimer = 2.0f; // Temps avant que le joueur reçoit des dégats si il reste sur des spikes par ex
    public bool isPlayerColliding = false; // Joueur en collision avec l'objet ?
    // ----- VARIABLES ----- //

    void Start()
    {
        
    }

    public void Update()
    {
        // Solution invincibilité :
        if (isPlayerColliding == true)
        {
            countdownTimer -= Time.deltaTime; // Time.deltaTime = temps entre chaque frame, prend 1 seconde pour enlever 1 à invincibleCounter
            if (countdownTimer < 0)
            {
                countdownTimer = 0; 
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other) // Créée par Unity, other = n'importe quel objet qui vient trigger
    {
        // Vérifier que l'objet qui trigger soit le Player :
        int layerVisible = LayerMask.NameToLayer("Player");
        if (other.tag == "Player" && other.gameObject.layer == layerVisible)
        {
            // Debug.Log("Hit"); // Message test console

            // Méthode simple :
            // FindObjectOfType<PlayerHealthController>().DealDamage(); // On enlève 1 à currentHealth du Player grâce à la fonction DealDamage de PlayerHeathController

            // Méthode avancée :
            PlayerHealthController.instance.DealDamage();

            // Solution invincibilité :
            isPlayerColliding = true; // Le joueur est en collision avec l'objet
        }
    }

    // Solution invincibilité :
    public void OnTriggerStay2D(Collider2D other) // Si il reste dans le trigger (ex : sur les spikes)
    {
        int layerVisible = LayerMask.NameToLayer("Player");
        if (other.tag == "Player" && isPlayerColliding == true && other.gameObject.layer == layerVisible) // On vérifie que c'est bien le joueur qui est en collision avec
        {
            if (countdownTimer <= 0) // Si il est pas invincible
            {
                PlayerHealthController.instance.DealDamage(); // On lui fait prendre des dégâts
                countdownTimer = 2.0f; // On remet le compteur à la normale
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other) // Si il quitte le trigger
    {
        if (other.tag == "Player") // On vérifie que ce soit bien le joueur
        {
            isPlayerColliding = false; // Il n'est plus en collision avec l'objet
        }
    }
}
