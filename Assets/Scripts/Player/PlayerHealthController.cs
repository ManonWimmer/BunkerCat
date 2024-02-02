using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // Instance, lien avec DamagePlayer :
    public static PlayerHealthController instance; // Static = ne peut pas �tre modifi�, m�me valeur si il est utilis� � diff�rents endroits

    // Creation du syst�me de sant� : 
    public int currentHealth, maxHealth;

    // Invincibilit� :
    public float invincibleLength; // Dur�e d'invincibilit�
    private float invincibleCounter; // Compteur jusqu'� 0, <=0 : prendre des d�g�ts, >0 : invincible

    // Afficher l'invincibilit� :
    private SpriteRenderer sr; // SpriteRenderer du joueur

    private PlayerController playerController;
    // ----- VARIABLES ----- //

    // Instance, lien avec DamagePlayer :
    private void Awake() // Juste avant Start()
    {
        instance = this; // Instance = version actuelle de PlayerHealthController, pour y avoir acc�s depuis d'autres scripts ! ;)
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public static PlayerHealthController GetInstance()
    {
        return instance;
    }

    void Start()
    {
        // Creation du syst�me de sant� : 
        currentHealth = maxHealth; // Initialisation de currentHealth

        // Afficher l'invincibilit� :
        sr = GetComponent<SpriteRenderer>(); // R�cup�ration du SpriteRenderer
    }

    void Update()
    {
        // Invincibilit� :
        if(invincibleCounter > 0) // Si on est invicible
        {
            invincibleCounter -= Time.deltaTime; // Time.deltaTime = temps entre chaque frame, prend 1 seconde pour enlever 1 � invincibleCounter

            // Enlever l'affichage de l'invincibilit� :
            if (invincibleCounter <= 0) // N'arrive qu'une seule fois
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f); // On remet l'opacit� normale
            }
        }
    }

    public bool IsMaxHealth()
    {
        return currentHealth == maxHealth;
    }

    public void DealDamage()
    {
        // Invincibilt� :
        if(invincibleCounter <= 0) // On peut prendre des dommages seulement si on est pas invincible
        {
            // Creation du syst�me de sant� :

            // Meilleure technique que "currentHealth -= 1;" :
            currentHealth--; // -- : -1, ++ : +1

            // Si le joueur est mort :
            if (currentHealth <= 0)
            {
                currentHealth = 0; // Pour �viter les bugs d'affichage d'UI

               // gameObject.SetActive(false); // On fait disparaitre le joueur, control� par le LevelManager plut�t qu'ici

                // Respawn le joueur quand il meurt :
                LevelManager.instance.RespawnPlayer();
            }
            // Invincibilit�
            else
            {
                invincibleCounter = invincibleLength; // On remet � la normale le compteur pour l'invincibilt�

                // Afficher l'invincibilit� :
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f); // Changement de l'opacit� du joueur � 50%

                // KnockBack : 
                playerController.KnockBack();
            }

            UIController.instance.UpdateHealthDisplay(); // Update des coeurs � l'�cran
        }
    }
    public void DealDamageWithoutKnockback()
    {
        // Invincibilt� :
        if (invincibleCounter <= 0) // On peut prendre des dommages seulement si on est pas invincible
        {
            // Creation du syst�me de sant� :

            // Meilleure technique que "currentHealth -= 1;" :
            currentHealth--; // -- : -1, ++ : +1

            // Si le joueur est mort :
            if (currentHealth <= 0)
            {
                currentHealth = 0; // Pour �viter les bugs d'affichage d'UI

                // gameObject.SetActive(false); // On fait disparaitre le joueur, control� par le LevelManager plut�t qu'ici

                // Respawn le joueur quand il meurt :
                LevelManager.instance.RespawnPlayer();
            }
            // Invincibilit�
            else
            {
                invincibleCounter = invincibleLength; // On remet � la normale le compteur pour l'invincibilt�

                // Afficher l'invincibilit� :
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f); // Changement de l'opacit� du joueur � 50%

                // KnockBack : 
                //playerController.KnockBack();
            }

            UIController.instance.UpdateHealthDisplay(); // Update des coeurs � l'�cran
        }
    }


    // Heal (pickup) :
    public void HealPlayer()
    {
        currentHealth++; // On ajoute 1 � la vie du joueur

        if (currentHealth > maxHealth) // On v�rifie que la valeur de la health soit pas sup�rieure � celle max
        {
            currentHealth = maxHealth; // Full health
        }

        UIController.instance.UpdateHealthDisplay(); // On update les coeurs � l'�cran
    }
}
