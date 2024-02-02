using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateTrigger : MonoBehaviour
{
    // ----- VARIABLES ----- //
    private bool isPlayerInRange; // Joueur dans le trigger ?
    private bool isPlayerInCrate;

    private ShowVisualCues showVisualCues;

    private GameObject crate;

    public GameObject player;
    private Vector2 playerPosition;

    private Shaker shaker;
    public float shakeDelay = 1f;
    private bool waitingBeforeShaking = false;

    [SerializeField]
    private PlayerController playerController;
    // ----- VARIABLES ----- //

    private void Awake()
    {
        isPlayerInRange = false; // Le joueur ne se trouve pas dans le trigger au début du jeu
        showVisualCues = GetComponent<ShowVisualCues>();
        showVisualCues.DesactivateAllCues();
        playerPosition = Vector2.zero;
        crate = transform.parent.gameObject;
        shaker = crate.GetComponent<Shaker>();
    }

    private void Update()
    {
        if (isPlayerInRange && !isPlayerInCrate) // Le joueur est dans le trigger et le dialogue n'est pas déjà dans la boite
        {
            showVisualCues.device = InputManager.GetInstance().GetDevice();
            showVisualCues.ActivateCueForDevice(); // On affiche le visual cue en fonction du device

            //if (Input.GetKeyDown(KeyCode.E)) // Touche E, input temp
            if (InputManager.GetInstance().GetInteractPressed())
            {
                // Chat rentre dans la boîte :
                isPlayerInCrate = true;

                playerController.canMove = false;
                //PlayerController.GetInstance().playerInCrate = true; // On bloque ses mouvements, ou faire canMove = false ?
                player.GetComponent<PlayerController>().enabled = false;

                playerPosition = player.transform.position; // On enregistre sa position ?
                int layerHidden = LayerMask.NameToLayer("PlayerHidden");
                player.layer = layerHidden; // On change le layer du chat pour PlayerHidden
                crate.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f); // Changement de couleur de la boîte

                player.GetComponent<SpriteRenderer>().enabled = false; // On le cache

                player.transform.position = transform.position; // On le met à la position de la boite pour la cam ?

            }
        }

        if (isPlayerInCrate) // Le chat est dans la boîte 
        {
            //showVisualCues.DesactivateAllCues();

            if (InputManager.GetInstance().GetInteractPressed())
            {
                // Chat sort de la boite :
                isPlayerInCrate = false;
                playerController.canMove = true;
                int layerVisible = LayerMask.NameToLayer("Player");
                player.layer = layerVisible; // On change sa layer de PlayerHidden à Player
                crate.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // On remet la couleur de la boite

                player.GetComponent<SpriteRenderer>().enabled = true; // On l'affiche

                //PlayerController.GetInstance().playerInCrate = false; // On lui remet le controle de ses mouvement -> canMove du PlayerController = true
                player.GetComponent<PlayerController>().enabled = true;

                player.transform.position = playerPosition; // On lui remet sa position d'avant ou de l'autre coté de la boite ?
            }

            // On secoue la boite avec un delai
            if (!waitingBeforeShaking)
            {
                shaker.ShakeObject();
                StartCoroutine(waitShake());
            }
        } 
        
        if (!isPlayerInRange) // Le joueur n'est pas / plus dans le trigger
        {
            //visualCue.SetActive(false); // On cache le visual cue
            showVisualCues.DesactivateAllCues();
        }
    }

    IEnumerator waitShake()
    {
        waitingBeforeShaking = true;
        yield return new WaitForSeconds(shakeDelay);
        waitingBeforeShaking = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerInRange = true; // Le joueur est dans le trigger
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerInRange = false; // Le joueur n'est plus dans le trigger
        }
    }


}
