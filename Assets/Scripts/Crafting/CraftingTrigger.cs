using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTrigger : MonoBehaviour
{
    // ----- VARIABLES ----- //
    private bool isPlayerInRange; // Joueur dans le trigger ?
    private ShowVisualCues showVisualCues;

    [SerializeField]
    public TextAsset textCraftingNotLearned;

    private InputManager inputManager;
    private CraftingController craftingController;
    // ----- VARIABLES ----- //

    private void Awake()
    {
        isPlayerInRange = false; // Le joueur ne se trouve pas dans le trigger au début du jeu
        showVisualCues = GetComponent<ShowVisualCues>();
        showVisualCues.DesactivateAllCues();
        
        inputManager = GameObject.Find("Managers").transform.GetChild(0).GetComponent<InputManager>();
        craftingController = GameObject.Find("CraftingTable").GetComponent<CraftingController>();

    }

    private void Update()
    {
        if (isPlayerInRange) // Le joueur est dans le trigger 
        {
            showVisualCues.device = InputManager.GetInstance().GetDevice();
            showVisualCues.ActivateCueForDevice();


            if (inputManager.GetInteractPressed()) { // touche
                Debug.Log(craftingController.GetCraftingLearned());
                if (craftingController.GetCraftingLearned()) // On ouvre la table de craft
                {
                    if (!craftingController.isCraftingTableOpen) // la table de craft n'est pas déjà ouverte
                    {
                        craftingController.OpenCraftingTable();
                    }
                    else // la table est déjà ouverte
                    {
                        craftingController.CloseCraftingTable();
                    }
                }
                else // Dialogue texte 
                {
                    DialogueManager.GetInstance().EnterDialogueMode(textCraftingNotLearned);
                }
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
