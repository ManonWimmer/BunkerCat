using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // Instance, lien avec DamagePlayer :
    public static PlayerHealthController instance; // Static = ne peut pas être modifié, même valeur si il est utilisé à différents endroits

    // Creation du système de santé : 
    public int currentHealth, maxHealth;

    // Invincibilité :
    public float invincibleLength; // Durée d'invincibilité
    private float invincibleCounter; // Compteur jusqu'à 0, <=0 : prendre des dégâts, >0 : invincible

    // Afficher l'invincibilité :
    private SpriteRenderer sr; // SpriteRenderer du joueur

    private PlayerController playerController;
    // ----- VARIABLES ----- //

    // Instance, lien avec DamagePlayer :
    private void Awake() // Juste avant Start()
    {
        instance = this; // Instance = version actuelle de PlayerHealthController, pour y avoir accès depuis d'autres scripts ! ;)
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public static PlayerHealthController GetInstance()
    {
        return instance;
    }

    void Start()
    {
        // Creation du système de santé : 
        currentHealth = maxHealth; // Initialisation de currentHealth

        // Afficher l'invincibilité :
        sr = GetComponent<SpriteRenderer>(); // Récupération du SpriteRenderer
    }

    void Update()
    {
        // Invincibilité :
        if(invincibleCounter > 0) // Si on est invicible
        {
            invincibleCounter -= Time.deltaTime; // Time.deltaTime = temps entre chaque frame, prend 1 seconde pour enlever 1 à invincibleCounter

            // Enlever l'affichage de l'invincibilité :
            if (invincibleCounter <= 0) // N'arrive qu'une seule fois
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f); // On remet l'opacité normale
            }
        }
    }

    public bool IsMaxHealth()
    {
        return currentHealth == maxHealth;
    }

    public void DealDamage()
    {
        // Invincibilté :
        if(invincibleCounter <= 0) // On peut prendre des dommages seulement si on est pas invincible
        {
            // Creation du système de santé :

            // Meilleure technique que "currentHealth -= 1;" :
            currentHealth--; // -- : -1, ++ : +1

            // Si le joueur est mort :
            if (currentHealth <= 0)
            {
                currentHealth = 0; // Pour éviter les bugs d'affichage d'UI

               // gameObject.SetActive(false); // On fait disparaitre le joueur, controlé par le LevelManager plutôt qu'ici

                // Respawn le joueur quand il meurt :
                LevelManager.instance.RespawnPlayer();
            }
            // Invincibilité
            else
            {
                invincibleCounter = invincibleLength; // On remet à la normale le compteur pour l'invincibilté

                // Afficher l'invincibilité :
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f); // Changement de l'opacité du joueur à 50%

                // KnockBack : 
                playerController.KnockBack();
            }

            UIController.instance.UpdateHealthDisplay(); // Update des coeurs à l'écran
        }
    }
    public void DealDamageWithoutKnockback()
    {
        // Invincibilté :
        if (invincibleCounter <= 0) // On peut prendre des dommages seulement si on est pas invincible
        {
            // Creation du système de santé :

            // Meilleure technique que "currentHealth -= 1;" :
            currentHealth--; // -- : -1, ++ : +1

            // Si le joueur est mort :
            if (currentHealth <= 0)
            {
                currentHealth = 0; // Pour éviter les bugs d'affichage d'UI

                // gameObject.SetActive(false); // On fait disparaitre le joueur, controlé par le LevelManager plutôt qu'ici

                // Respawn le joueur quand il meurt :
                LevelManager.instance.RespawnPlayer();
            }
            // Invincibilité
            else
            {
                invincibleCounter = invincibleLength; // On remet à la normale le compteur pour l'invincibilté

                // Afficher l'invincibilité :
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f); // Changement de l'opacité du joueur à 50%

                // KnockBack : 
                //playerController.KnockBack();
            }

            UIController.instance.UpdateHealthDisplay(); // Update des coeurs à l'écran
        }
    }


    // Heal (pickup) :
    public void HealPlayer()
    {
        currentHealth++; // On ajoute 1 à la vie du joueur

        if (currentHealth > maxHealth) // On vérifie que la valeur de la health soit pas supérieure à celle max
        {
            currentHealth = maxHealth; // Full health
        }

        UIController.instance.UpdateHealthDisplay(); // On update les coeurs à l'écran
    }
}
