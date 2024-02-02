using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class QuestSO : ScriptableObject
{
    // ----- VARIABLES ----- //
    [field: SerializeField]
    public string QuestNPC { get; set; } // ou questId

    [field: SerializeField]
    public string QuestName { get; set; } 

    [field: SerializeField]
    public string QuestDescription { get; set; }

    [field: SerializeField]
    public bool isMainQuest;

    [field: SerializeField]
    public bool isStarted;

    [field: SerializeField]
    public bool isCompleted;

    [field: SerializeField]
    public List<CraftingItem> RequiredItems { get; private set; } // Items requis, list [ItemSO, quantity]

    [field: SerializeField]
    public CraftingItem ResultCraft { get; private set; } // Item en retour

    public bool CanBeCompleted = false;

    public static QuestSO instance;
    // ----- VARIABLES ----- //

    private void Awake()
    {
        instance = this;
    }

    public static QuestSO GetInstance()
    {
        return instance;
    }

}
