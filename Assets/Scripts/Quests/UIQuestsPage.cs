using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIQuestsPage : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private List<UIQuestItem> listOfQuestsUI = new List<UIQuestItem>(); // Les Slots d'ingrédients

    public static UIQuestsPage instance;

    public QuestsSO playerQuests;

    public GamepadCursor gamepadCursor;

    public bool isQuestsOpen;

    private PlayerController playerController;
    // ----- VARIABLES ----- //
    private void Awake()
    {
        //itemDescription.ResetDescription();
        instance = this;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        Hide();
        InitializeQuestsUI();
    }

    private void Update()
    {
        if (gameObject.activeSelf) // inventaire ouvert
        {
            if (InputManager.GetInstance().GetDevice() != "Keyboard") // Gamepad
            {
                gamepadCursor.gameObject.SetActive(true);
                Cursor.visible = false;
            }
            else
            {
                gamepadCursor.gameObject.SetActive(false); // Souris
                Cursor.visible = true;
            }
        }
    }

    public static UIQuestsPage GetInstance()
    {
        return instance;
    }

    public void InitializeQuestsUI()
    {
        Debug.Log("initialize quests");
        int i = 0;
        foreach(UIQuestItem questSlot in listOfQuestsUI)
        {
            Debug.Log(i);
            QuestSO questSelected = playerQuests.GetQuestAt(i);
            Debug.Log(questSelected.QuestName);

            questSlot.SetData(questSelected);

            i++;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true); // On rend visible l'Inventory
        InitializeQuestsUI(); // On recommence l'initialize au cas ou entre temps une recette a été découverte
        isQuestsOpen = true;
        playerController.UIOpen = true;

        if (InputManager.GetInstance().GetDevice() != "Keyboard") // Gamepad
        {
            gamepadCursor.gameObject.SetActive(true);
        }
        else
        {
            Cursor.visible = true;
        }   
    }

    public void Hide()
    {
        playerController.UIOpen = false;
        isQuestsOpen = false;
        gameObject.SetActive(false); // On cache l'Inventory
        if (InputManager.GetInstance().GetDevice() != "Keyboard") // Gamepad
        {
            gamepadCursor.gameObject.SetActive(false);
            
        }
        else
        {
            Cursor.visible = false;
        }
    }
}
