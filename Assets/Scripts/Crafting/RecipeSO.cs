using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
#if UNITY_EDITOR
using UnityEditor.PackageManager.Requests;
#endif

using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct CraftingItem // Pour avoir des listes [ItemSO, quantity] pour chaque index
{
    public ItemSO Item;
    public int Quantity;
}

[CreateAssetMenu] // !!!
public class RecipeSO : ScriptableObject
{
    // ----- VARIABLES ----- //
    [field: SerializeField]
    public string RecipeName { get; set; } // Nom de l'item

    [field: SerializeField]
    public string RecipeDescription { get; set; } // Nom de l'item

    [field: SerializeField]
    public Sprite RecipeImage { get; set; } // Image de l'item

    [field: SerializeField]
    public bool isDiscovered; // Est que le joueur a trouvé le recipe, pour l'utiliser ?

    [field: SerializeField]
    public List<CraftingItem> RequiredItems { get; private set; } // Items requis, list [ItemSO, quantity]

    [field: SerializeField]
    public CraftingItem ResultCraft { get; private set; } // Item craft

    public bool CanBeCrafted = false; // ?

    public static RecipeSO instance;
    // ----- VARIABLES ----- //

    private void Awake()
    {
        instance = this;
    }

    public static RecipeSO GetInstance()
    {
        return instance;
    }

}


