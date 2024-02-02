using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using Newtonsoft.Json.Bson;

public class InkExternalFunctions
{
    public void Bind(Story story)
    {
        story.BindExternalFunction("startQuest", (int questId) => StartQuest(questId));
        story.BindExternalFunction("completeQuest", (int questId) => CompleteQuest(questId));
        story.BindExternalFunction("checkCanCompleteQuest", (int questId) => CheckCanCompleteQuest(questId));
        
    }
        

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("startQuest");
        story.UnbindExternalFunction("completeQuest");
        story.UnbindExternalFunction("checkCanCompleteQuest");
    }

    public void StartQuest(int questId)
    {
        QuestsController.GetInstance().StartQuest(questId);
    }

    public bool CheckCanCompleteQuest(int questId)
    {
        return QuestsController.GetInstance().CheckCanCompleteQuest(questId); ;
    }

    public void CompleteQuest(int questId)
    {
        QuestsController.GetInstance().CompleteQuest(questId);
    }
}