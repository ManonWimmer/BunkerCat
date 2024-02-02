using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUIController : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private UIRecipesPage recipesUI; // Tout l'UI de l'Inventory

    [field : SerializeField]
    public RecipesSO RecipesData { get; set; }

    public static CraftingUIController instance;

    // ----- VARIABLES ----- //

    private void Awake()
    {
        instance = this;
    }

    public static CraftingUIController GetInstance()
    {
        return instance;
    }

    public RecipesSO GetPlayerRecipes()
    {
        return RecipesData;
    }

    private void Start()
    {
        PrepareUI(); // Création des items dans l'inventaire et les ajouter à une liste + events
    }

    private void PrepareUI()
    {
        recipesUI.InitializeRecipesUI();
        this.recipesUI.OnDescriptionRequested += HandleDescriptionRequested;
    }

    // Events :
    private void HandleDescriptionRequested(int itemIndex)
    {
        RecipeSO recipeItem = RecipesData.GetItemAt(itemIndex);
        recipesUI.UpdateDescription(itemIndex, recipeItem);
    }



}
