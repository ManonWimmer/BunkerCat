using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestsSO : ScriptableObject
{
    // ----- VARIABLES ----- //
    [field: SerializeField]
    public List<QuestSO> Quests { get; set; }

    public static QuestsSO instance;
    // ----- VARIABLES ----- //

    private void Awake()
    {
        instance = this;
    }

    public static QuestsSO GetInstance()
    {
        return instance;
    }

    public Dictionary<int, QuestSO> GetCurrentQuestsState() // Int = clé du dictionnaire, index dans la liste d'items
    {
        Dictionary<int, QuestSO> returnValue = new Dictionary<int, QuestSO>();

        for (int i = 0; i < Quests.Count; i++) // Pour chaque slot de l'inventory
        {
            returnValue[i] = Quests[i]; // On renvoie l'item non vide

        }

        return returnValue; // Renvoie un dictionnaire avec tous les items de l'inventaire (clé : index, valeur : item)
    }

    public QuestSO GetQuestAt(int itemIndex)
    {
        //Debug.Log(itemIndex);
        return Quests[itemIndex];
    }


}
