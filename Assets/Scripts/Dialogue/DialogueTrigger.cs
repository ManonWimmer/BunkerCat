using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor.ShaderGraph.Drawing;
#endif

using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [Header("Ink JSON")]
    [SerializeField]
    private TextAsset inkJSON;

    private bool isPlayerInRange; // Joueur dans le trigger ?

    private ShowVisualCues showVisualCues;

    // ----- VARIABLES ----- //

    private void Awake()
    {
        isPlayerInRange = false; // Le joueur ne se trouve pas dans le trigger au début du jeu
        showVisualCues = GetComponent<ShowVisualCues>();
        showVisualCues.DesactivateAllCues();
    }

    private void Update()
    {
        if (isPlayerInRange && !DialogueManager.GetInstance().dialogueIsPlaying) // Le joueur est dans le trigger et le dialogue n'est pas déjà en cours
        {
            showVisualCues.device = InputManager.GetInstance().GetDevice();
            showVisualCues.ActivateCueForDevice(); // On affiche le visual cue

            //if (Input.GetKeyDown(KeyCode.E)) // Touche E, input temp
            if (InputManager.GetInstance().GetInteractPressed())
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON); 
            }
        }
        else // Le joueur n'est pas / plus dans le trigger
        {
            showVisualCues.DesactivateAllCues(); // On cache le visual cue
        }
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
