using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyScratchingTrigger : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private GameObject objectToDestroy;
    private bool isPlayerInTrigger;
    private bool isAttacking;
    private bool destroyed;

    private PlayerController playerController;

    [SerializeField]
    private ShowVisualCues showVisualCues;
    // ----- VARIABLES ----- //
    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        destroyed = false;
    }
    private void Start()
    {
        isPlayerInTrigger = false;
        //DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        isAttacking = playerController.isAttacking;
        //if (isPlayerInTrigger && InputManager.GetInstance().GetAttackPressed()) // + Raycast que le joueur est dans la bonne direction ?

        if (isPlayerInTrigger && isAttacking)
        {
            StartCoroutine(DestroyObject());
        }

        if (!destroyed)
        {
            showVisualCues.device = InputManager.GetInstance().GetDevice();
            showVisualCues.ActivateCueForDevice(); // On affiche le visual cue en fonction du device
        }
        else
        {
            showVisualCues.DesactivateAllCues();
        }
        
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.2f); // Pour le détruire en même temps que l'anim
        Destroy(objectToDestroy);
        destroyed = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("dans trigger");
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }


    


}
