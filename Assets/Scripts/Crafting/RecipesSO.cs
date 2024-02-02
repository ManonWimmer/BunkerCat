using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipesSO : ScriptableObject
{
    // ----- VARIABLES ----- //
    [field: SerializeField]
    public List<RecipeSO> Recipes { get; set; }

    public static RecipesSO instance;
    // ----- VARIABLES ----- //

    private void Awake()
    {
        instance = this;
    }

    public static RecipesSO GetInstance()
    {
        return instance;
    }

    public Dictionary<int, RecipeSO> GetCurrentRecipesState() // Int = clé du dictionnaire, index dans la liste d'items
    {
        Dictionary<int, RecipeSO> returnValue = new Dictionary<int, RecipeSO>();
        
        for (int i = 0; i < Recipes.Count; i++) // Pour chaque slot de l'inventory
        {
        returnValue[i] = Recipes[i]; // On renvoie l'item non vide
            
        }

        return returnValue; // Renvoie un dictionnaire avec tous les items de l'inventaire (clé : index, valeur : item)
    }

    public RecipeSO GetItemAt(int itemIndex)
    {
        //Debug.Log(itemIndex);
        return Recipes[itemIndex];
    }


}
