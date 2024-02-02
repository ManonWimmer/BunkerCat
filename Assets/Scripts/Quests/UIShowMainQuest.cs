using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Rendering;

public class UIShowMainQuest : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private QuestsSO playerQuests;

    [SerializeField]
    private TMP_Text mainQuestTxt;

    [SerializeField]
    private Slider sliderItems;

    [SerializeField]
    private Image imageBackground;

    [SerializeField]
    private Image imageFillArea;

    private bool isOnScreen;
    // ----- VARIABLES ---- //

    private void Start()
    {
        HideMainQuest();
    }

    private void Update()
    {
        if (!isOnScreen)
        {
            bool fernandoStarted = playerQuests.GetQuestAt(0).isStarted;
            if (fernandoStarted)
            {
                ShowMainQuest();
            }
        }
    }

    private void HideMainQuest()
    {
        mainQuestTxt.enabled = false;
        imageBackground.enabled = false; // Background
        imageFillArea.enabled = false; // Fill area
        isOnScreen = false;
    }

    private void ShowMainQuest()
    {
        mainQuestTxt.enabled = true;
        imageBackground.enabled = true; // Background
        imageFillArea.enabled = true; // Fill area
        isOnScreen = true;
    }
}
