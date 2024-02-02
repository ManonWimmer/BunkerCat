using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // Solution invincibilit� :
    public float countdownTimer = 2.0f; // Temps avant que le joueur re�oit des d�gats si il reste sur des spikes par ex
    public bool isPlayerColliding = false; // Joueur en collision avec l'objet ?
    // ----- VARIABLES ----- //

    void Start()
    {
        
    }

    public void Update()
    {
        // Solution invincibilit� :
        if (isPlayerColliding == true)
        {
            countdownTimer -= Time.deltaTime; // Time.deltaTime = temps entre chaque frame, prend 1 seconde pour enlever 1 � invincibleCounter
            if (countdownTimer < 0)
            {
                countdownTimer = 0; 
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other) // Cr��e par Unity, other = n'importe quel objet qui vient trigger
    {
        // V�rifier que l'objet qui trigger soit le Player :
        int layerVisible = LayerMask.NameToLayer("Player");
        if (other.tag == "Player" && other.gameObject.layer == layerVisible)
        {
            // Debug.Log("Hit"); // Message test console

            // M�thode simple :
            // FindObjectOfType<PlayerHealthController>().DealDamage(); // On enl�ve 1 � currentHealth du Player gr�ce � la fonction DealDamage de PlayerHeathController

            // M�thode avanc�e :
            PlayerHealthController.instance.DealDamage();

            // Solution invincibilit� :
            isPlayerColliding = true; // Le joueur est en collision avec l'objet
        }
    }

    // Solution invincibilit� :
    public void OnTriggerStay2D(Collider2D other) // Si il reste dans le trigger (ex : sur les spikes)
    {
        int layerVisible = LayerMask.NameToLayer("Player");
        if (other.tag == "Player" && isPlayerColliding == true && other.gameObject.layer == layerVisible) // On v�rifie que c'est bien le joueur qui est en collision avec
        {
            if (countdownTimer <= 0) // Si il est pas invincible
            {
                PlayerHealthController.instance.DealDamage(); // On lui fait prendre des d�g�ts
                countdownTimer = 2.0f; // On remet le compteur � la normale
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other) // Si il quitte le trigger
    {
        if (other.tag == "Player") // On v�rifie que ce soit bien le joueur
        {
            isPlayerColliding = false; // Il n'est plus en collision avec l'objet
        }
    }
}
