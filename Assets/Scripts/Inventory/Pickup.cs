using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // Gem / Heal pickup :
    public bool isGem, isHeal; // Est-ce que l'objet lié au script Pickup est une gem, un heal ?

    private bool isCollected; // Pour être sur de ne pas ramasser le collectible deux fois

    // Pickup Effect :
    public GameObject pickupEffect;

    // ----- VARIABLES ----- //

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Gem :
        if (other.CompareTag("Player") && !isCollected) // Le joueur entre dans le trigger du collectible et il n'a jamais été collecté
        {
            if (isGem) // Si le collectible est une gem
            {
                //PlayerController.instance.gemsCollected++; // On ajoute 1 au nombre de gems collectées

                isCollected = true; // On l'a collectée

                Destroy(gameObject); // On détruit l'objet

                // Pickup Effect : 
                Instantiate(pickupEffect, transform.position, transform.rotation); // Instantiate = créer une nouvelle copie d'un objet, à quelle position, quelle rotation

                UIController.instance.UpdateGemCount(); // Update du nombre de gem collectés à l'écran
            }
        }

        // Heal :
        if (isHeal)
        {
            if(PlayerHealthController.instance.currentHealth != PlayerHealthController.instance.maxHealth) // Impossible de le récolter si le joueur a full health
            {
                PlayerHealthController.instance.HealPlayer(); // +1 à la vie du joueur

                isCollected = true; // On l'a collectée

                Destroy(gameObject); // On détruit l'objet

                // Pickup Effect : 
                Instantiate(pickupEffect, transform.position, transform.rotation); // Instantiate = créer une nouvelle copie d'un objet, à quelle position, quelle rotation
            }
        }
        
    }
}
