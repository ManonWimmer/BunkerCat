using Ink.Parsed;
using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingController : MonoBehaviour
{
    // ----- VARIABLES ----- //
    private Dictionary<int, InventoryItem> inventoryItems;

    [SerializeField]
    private InventorySO inventory;

    public bool isCraftingTableOpen { get; set; }

    public CraftingController instance;

    [SerializeField]
    private RecipesSO playerRecipes;

    private Dictionary<ItemSO, int> ingredientsInInventory;

    private List<CraftingItem> requiredItems;
    private List<ItemSO> requiredItemsOnly;

    public UIRecipesPage uiRecipesPage;

    public bool craftingLearned = false;
    // ----- VARIABLES ----- //

    public CraftingController GetInstance() 
    {
        return this;
    }

    public InventorySO GetInventory()
    {
        return inventory;
    }

    private void Awake()
    {
        instance = this;
        isCraftingTableOpen = false;
    }

    public bool GetCraftingLearned()
    {
        return craftingLearned;
    }

    private void Start()
    {
        playerRecipes = CraftingUIController.GetInstance().GetPlayerRecipes();
    }

    private void UpdateInventoryItems()
    {
        inventoryItems = inventory.GetCurrentInventoryState();
    }

    public void SearchRecipeIngredientsInInventory(RecipeSO recipe)
    {
        UpdateInventoryItems();

        requiredItems = recipe.RequiredItems;

        recipe.CanBeCrafted = false;
        ingredientsInInventory = new Dictionary<ItemSO, int>();

        // On regarde chaque (cl� : index, valeur : InventoryIndex (item, quantity, isEmpty)) des �l�ments de l'inventaire
        // On cr�e un dictionnaire avec seulement les ingr�dients pour la recipe

        GetListRequiredItems(recipe); // List avec seulement les ItemSO de la recette (pas leur quantit�)

        if (inventoryItems.Count > 0)
        {
            for (int i = 0; i < inventory.Size; i++)
            {
                if (inventoryItems.ContainsKey(i) && !inventoryItems[i].IsEmpty)
                {
                    if (requiredItemsOnly.Contains(inventoryItems[i].item)) // Meme item qu'un ingr�dient
                    { // On l'ajoute au dict

                        if (ingredientsInInventory.ContainsKey(inventoryItems[i].item)) // Si il est d�j� dans le dict
                        {
                            ingredientsInInventory[inventoryItems[i].item] += inventoryItems[i].quantity; // On ajoute la quantit�
                        }
                        else // Sinon on le cr�e dans le dict
                        {
                            ingredientsInInventory[inventoryItems[i].item] = inventoryItems[i].quantity;
                        }
                    }
                } // Sinon si empty, on fait rien
            }
        }
        
        CheckIngredientsQuantities(recipe, ingredientsInInventory);
    }

    public int GetIngredientInventoryQuantity(CraftingItem ingredient) 
    {
        UpdateInventoryItems();
        int ingredientQuantityInventory = 0;

        if (inventory.Size > 0)
        {
            for (int i = 0; i < inventory.Size; i++)
            {
                Debug.Log(i);
                if (inventoryItems.ContainsKey(i) && !inventoryItems[i].IsEmpty)
                {
                    if (ingredient.Item == inventoryItems[i].item) // Meme item que l'ingr�dient
                    { 
                        ingredientQuantityInventory += inventoryItems[i].quantity; // On ajoute la quantit�
                    }
                } // Sinon si empty, on fait rien
            }
        }

        return ingredientQuantityInventory;
    }

    private void CheckIngredientsQuantities(RecipeSO recipe, Dictionary<ItemSO, int> ingredientsInInventory)
    {
        // On parcourt le dict (si il est rempli, sinon false) - d�s qu'une des quantit�s est inf�rieure � celle demand�, canCraft = false, sinon true

        requiredItems = recipe.RequiredItems;

        foreach (CraftingItem itemRecipe in requiredItems) // Pour chaque item requis de la recette
        {
            ItemSO recipeItem = itemRecipe.Item; // On r�cup�re l'Item SO des ingr�dients n�cessaires

            if (ingredientsInInventory.ContainsKey(recipeItem)) // Ingr�dient dans l'inventaire
            {
                int quantityInventory = ingredientsInInventory[recipeItem];

                if (quantityInventory >= itemRecipe.Quantity) // Ingr�dient dans l'inventaire avec une quantit� suffisante pour craft : craftable pour l'instant
                {
                    recipe.CanBeCrafted = true;
                    //Debug.Log(recipeItem.Name + ":" + quantityInventory.ToString() + "/" + itemRecipe.Quantity.ToString());
                }
                else // Ingr�dient dans l'inventaire mais pas assez de quantit� : false
                {
                    recipe.CanBeCrafted = false;
                    //Debug.Log(recipeItem.Name + ":" + quantityInventory.ToString() + "/" + itemRecipe.Quantity.ToString());
                    //Debug.Log("return false");
                    return;
                }
            }
            else // D�s qu'un ingr�dient n�cessaire � la recette n'est pas dans l'inventaire, c'est incraftable
            {
                recipe.CanBeCrafted = false;
                //Debug.Log("return false");
                return;
            } 
        } 
    }

    private void GetListRequiredItems(RecipeSO recipe)
    {
        requiredItemsOnly = new List<ItemSO>();

        foreach(CraftingItem item in recipe.RequiredItems)
        {
            requiredItemsOnly.Add(item.Item); // On ajoute que les ItemSO (sans leur quantit�) dans le 
        }
    }

    public void OpenCraftingTable()
    {
        playerRecipes = CraftingUIController.GetInstance().GetPlayerRecipes();
        isCraftingTableOpen = true;
        uiRecipesPage.Show();

        foreach (RecipeSO recipe in playerRecipes.Recipes)
        {
            SearchRecipeIngredientsInInventory(recipe); // Check des ingr�dients de chaque recipe   
        }
    }

    public void CloseCraftingTable() //
    {
        isCraftingTableOpen = false;
        uiRecipesPage.Hide();
        UIRecipeDescription.GetInstance().ResetDescription();
    }
}
