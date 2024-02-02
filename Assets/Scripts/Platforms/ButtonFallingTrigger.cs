using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFallingTrigger : MonoBehaviour
{
    // ----- VARIABLES ----- //
    public List<GameObject> listPlatforms;
    public List<FallingPlatform> listFallingPlatforms;

    //public OneWayMovement movementLaser;
    public Scaler scaleLight;

    private bool isPlayerInRange; // Joueur dans le trigger ?
    private bool firstApparition = true;

    private ShowVisualCues showVisualCues;
    // ----- VARIABLES ----- //

    private void Awake()
    {
        isPlayerInRange = false; // Le joueur ne se trouve pas dans le trigger au début du jeu
        showVisualCues = GetComponent<ShowVisualCues>();
        showVisualCues.DesactivateAllCues();
    }

    /* Marche pas
    private void Start()
    {
        
        for (int i = 0; i < listPlatforms.Count; i++)
        {
            FallingPlatform fallingPlatform = listPlatforms[i].transform.GetChild(0).GetComponent<FallingPlatform>();
            listFallingPlatforms.Add(fallingPlatform);
        }

        Debug.Log(listFallingPlatforms);
    }*/
    

    private void Update()
    {
        if (isPlayerInRange) // Le joueur est dans le trigger
        {
            showVisualCues.device = InputManager.GetInstance().GetDevice();
            showVisualCues.ActivateCueForDevice();

            if (InputManager.GetInstance().GetInteractPressed())
            {
                Debug.Log("bouton");
                //movementLaser.StartMovement();
                scaleLight.StartScale();

                // On les affiche
                if (firstApparition)
                {
                    Debug.Log("first apparition");
                    for (int i = 0; i < listPlatforms.Count; i++)
                    {
                        listPlatforms[i].SetActive(true);
                    }
                    firstApparition = false;
                }
                else
                {
                    Debug.Log("else");

                    Debug.Log(listFallingPlatforms.Count);
                    for (int i = 0; i < listFallingPlatforms.Count; i++)
                    {
                        Debug.Log("spawn " + i);
                        listFallingPlatforms[i].SpawnPlatform();
                    }
                }
                
            }
        }
        else // Le joueur n'est pas / plus dans le trigger
        {
            showVisualCues.DesactivateAllCues();
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
