using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStatic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ici");


        // QUETES
        QuestsSO playerQuests = QuestsController.GetInstance().GetPlayerQuests();
        Debug.Log(playerQuests.Quests.Count);
        for (int i = 0; i < playerQuests.Quests.Count; i++)
        {
            playerQuests.Quests[i].isStarted = false;
            playerQuests.Quests[i].isCompleted = false;
        }


    }


}
