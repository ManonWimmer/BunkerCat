using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using Newtonsoft.Json.Bson;
using Inventory.Model;
#if UNITY_EDITOR
using UnityEditor.ShaderGraph;
#endif


public class UIQuestItem : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private Sprite checkYes;

    [SerializeField]
    private Sprite checkNo;

    [SerializeField]
    private TMP_Text questNameTxt; 

    [SerializeField]
    private TMP_Text questDescriptionTxt;

    [SerializeField]
    private List<TMP_Text> questItemsTxt;

    [SerializeField]
    private Image borderImage; // Border de l'image

    [SerializeField]
    private Image borderRequestedItem; // Border de l'image

    [SerializeField]
    private Image fondImage; // Border de l'image

    [SerializeField]
    private UIQuestIngredient questResultCraft;

    [SerializeField]
    private Image checkStarted;

    [SerializeField]
    private Image checkCompleted;

    private bool isStarted;

    private bool isCompleted;
    // ----- VARIABLES ----- //

    public void Awake()
    {
        isStarted = false;
        isCompleted = false;
    }

    public void NotDiscoveredQuest(QuestSO quest)
    {
        borderImage.color = new Color(.5f, .5f, .5f, 1f); // Gris

        // Fond : 
        fondImage.color = new Color(0f, 0f, 0f, 1f); // Noir

        // Textes non découverts
        questNameTxt.GetComponent<TMP_Text>().text = "???";
        questDescriptionTxt.GetComponent<TMP_Text>().text = "???";

        // Textes en gris : 
        for (int i = 0; i < questItemsTxt.Count; i++)
        {
            questItemsTxt[i].color = new Color(.5f, .5f, .5f, 1f);
        }

        // Required Item
        questResultCraft.Reinitialize();

        // Pour les check ou pas check :
        checkStarted.sprite = checkNo;
        checkCompleted.sprite = checkNo;
    }


    public void DiscoveredQuest(QuestSO quest)
    {
        borderImage.color = new Color(1f, 1f, 1f, 1f); // Blanc

        // Fond : 
        fondImage.color = new Color(0.34f, 0.39f, 0.29f, 1f); // Vert pale

        // Textes découverts
        questNameTxt.GetComponent<TMP_Text>().text = quest.QuestName;
        questDescriptionTxt.GetComponent<TMP_Text>().text = quest.QuestDescription;

        // Required Item
        CraftingItem requiredItem = quest.RequiredItems[0];
        questResultCraft.SetIngredientSlot(requiredItem);

        // Textes en blanc : 
        for (int i = 0; i < questItemsTxt.Count; i++)
        {
            questItemsTxt[i].color = new Color(1f, 1f, 1f, 1f);
        }

        // Pour les check ou pas check :
        checkStarted.sprite = checkYes;
        checkCompleted.sprite = checkNo;
    }

    public void FinishedQuest(QuestSO quest)
    {
        borderImage.color = new Color(1f, 1f, 1f, 1f); // Blanc

        // Fond : 
        fondImage.color = new Color(0.8f, 0.7f, 0.26f, 1f); // Jaune

        // Textes découverts :
        questNameTxt.GetComponent<TMP_Text>().text = quest.QuestName;
        questDescriptionTxt.GetComponent<TMP_Text>().text = quest.QuestDescription;

        // Required Item
        CraftingItem requiredItem = quest.RequiredItems[0];
        questResultCraft.SetIngredientSlot(requiredItem);

        // Textes en blanc : 
        for (int i = 0; i < questItemsTxt.Count; i++)
        {
            questItemsTxt[i].color = new Color(1f, 1f, 1f, 1f);
        }

        // Pour les check ou pas check :
        checkStarted.sprite = checkYes;
        checkCompleted.sprite = checkYes;
    }

    public void SetData(QuestSO quest)
    {
        Debug.Log("set data item");
        this.isStarted = quest.isStarted;
        this.isCompleted = quest.isCompleted;

        if (!isStarted && !isCompleted)
        {
            NotDiscoveredQuest(quest);
        }

        if (isStarted && !isCompleted)
        {
            DiscoveredQuest(quest);
        }

        if (isStarted && isCompleted)
        {
            FinishedQuest(quest);
        }
    }
}

