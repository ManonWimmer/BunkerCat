using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRecipeDescription : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private Image itemImage; // Icone de l'item

    [SerializeField]
    private TMP_Text title; // Nom de l'item

    [SerializeField]
    private TMP_Text description; // Description de l'item

    [SerializeField]
    private List<UIRecipeIngredient> listOfIngredientSlots = new List<UIRecipeIngredient>(); // List des items dans l'inventory

    private RecipeSO recipeSelected;

    public static UIRecipeDescription instance;
    // ----- VARIABLES ----- //

    public void Awake()
    {
        ResetDescription();
        instance = this;
    }

    public static UIRecipeDescription GetInstance()
    {
        return instance;
    }

    public void ResetDescription()
    {
        this.itemImage.gameObject.SetActive(false); // On cache l'icone de l'item
        this.title.text = "???"; // On met le nom de l'item vide
        this.description.text = "???"; // On met la description de l'item vide

        foreach(UIRecipeIngredient ingredientSlot in listOfIngredientSlots)
        {
            ingredientSlot.Reinitialize();
        }

        recipeSelected = null;
    }

    public void SetDescription(RecipeSO recipe)
    {
        recipeSelected = recipe;

        this.itemImage.gameObject.SetActive(true); // On affiche l'icone de l'item
        this.itemImage.sprite = recipe.RecipeImage; // On change l'icone de l'item
        this.title.text = recipe.RecipeName; // On change le nom de l'item
        this.description.text = recipe.RecipeDescription; // On change la description de l'item

        
        int numberOfIngredientsInRecipe = 0;
        foreach(CraftingItem ingredient in recipe.RequiredItems)
        {
            numberOfIngredientsInRecipe++;
        }

        int indexIngredientSlot = 0;
        for (indexIngredientSlot = 0; indexIngredientSlot < numberOfIngredientsInRecipe; indexIngredientSlot++)
        {
            listOfIngredientSlots[indexIngredientSlot].SetIngredientSlot(recipe.RequiredItems[indexIngredientSlot]);
        }
        while (indexIngredientSlot < 4) // Cacher les autres ingrédients non utilisés
        {
            listOfIngredientSlots[indexIngredientSlot].HiddenIngredientSlot();
            indexIngredientSlot++;
        }
        
        //Debug.Log(numberOfIngredientsInRecipe);
    }

    public RecipeSO GetRecipeSelected()
    {
        return recipeSelected;
    }


}

