using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif


public class UIRecipeIngredient : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private Image ingredientImage;

    [SerializeField]
    private Image ingredientBordure;

    [SerializeField]
    private TMP_Text ingredientName; // Nom de l'item

    [SerializeField]
    private TMP_Text ingredientRatio; // X/X

    private CraftingController craftingController;
    // ----- VARIABLES ----- //
    private void Awake()
    {
        craftingController = GameObject.Find("CraftingTable").GetComponent<CraftingController>();
    }

    public void SetIngredientSlot(CraftingItem ingredient)
    {
        gameObject.SetActive(true);

        ItemSO item = ingredient.Item;

        ingredientImage.enabled = true;
        ingredientImage.sprite = item.ItemImage;
        ingredientName.text = item.Name;

        int ingredientQuantityRequested = ingredient.Quantity;
        int ingredientQuantityInventory = craftingController.GetIngredientInventoryQuantity(ingredient);

        // Couleur bordure :
        if (ingredientQuantityInventory >= ingredientQuantityRequested)
        {
            ingredientBordure.color = new Color(0.47f, 0.62f, 0.39f, 1f); // Vert
        }
        else
        {
            ingredientBordure.color = new Color(0.6f, 0.1f, 0.16f, 1f); // Rouge
        }


        ingredientRatio.text = ingredientQuantityInventory.ToString() + "/" + ingredientQuantityRequested.ToString();
    }

    public void Reinitialize()
    {
        gameObject.SetActive(true);
        ingredientImage.enabled = false;
        ingredientName.text = "???";
        ingredientRatio.text = "X/X";
        ingredientBordure.color = new Color(1f, 1f, 1f, 1f); // Blanc
    }

    public void HiddenIngredientSlot()
    {
        gameObject.SetActive(false);
    }


}

