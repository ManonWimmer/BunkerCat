using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIRecipesPage : MonoBehaviour
{
    // ----- VARIABLES ----- //

    // Inventory Description :
    [SerializeField]
    private UIRecipeDescription recipeDescription;

    [SerializeField]
    private List<UICraftItem> listOfCrafts = new List<UICraftItem>(); // Les Slots d'ingrédients

    // Event actions : (int = index de l'item)
    public event Action<int> OnDescriptionRequested;

    public static UIRecipesPage instance;

    public GamepadCursor gamepadCursor;

    private PlayerController playerController;
    // ----- VARIABLES ----- //
    private void Awake()
    {
        //itemDescription.ResetDescription();
        instance = this;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (gameObject.activeSelf) // inventaire ouvert
        {
            if (InputManager.GetInstance().GetDevice() != "Keyboard") // Gamepad
            {
                gamepadCursor.gameObject.SetActive(true);
                Cursor.visible = false;
            }
            else
            {
                gamepadCursor.gameObject.SetActive(false); // Souris
                Cursor.visible = true;
            }
        }
    }

    public static UIRecipesPage GetInstance()
    {
        return instance;
    }

    public void InitializeRecipesUI()
    {
        int i = 0;
        foreach(UICraftItem recipeSlot in listOfCrafts)
        {
            //Debug.Log(recipeSlot.name);
            //Debug.Log(CraftingUIController.GetInstance().recipesData);
            //Debug.Log(CraftingUIController.GetInstance().recipesData.GetItemAt(i));
            RecipeSO recipeSelected = CraftingUIController.GetInstance().RecipesData.GetItemAt(i);
            //Debug.Log(recipeSelected + recipeSelected.isDiscovered.ToString());
            if (recipeSelected.isDiscovered)
            {
                recipeSelected.CanBeCrafted = false;
                //Debug.Log(recipeSlot + "discovered" + recipeSelected);
                recipeSlot.OnItemClicked += HandleItemSelection;
                recipeSlot.UnlockItem(recipeSelected);
            } else
            {
                //Debug.Log(recipeSlot + "not discovered" + recipeSelected);
                recipeSlot.LockItem(recipeSelected);
            }
            
            i++;
        }
    }

    private void HandleItemSelection(UICraftItem recipeSlot)
    {
        int index = listOfCrafts.IndexOf(recipeSlot); // On récupère l'index de l'item que l'on a selectionné

        // Description :
        OnDescriptionRequested?.Invoke(index);
    }

    public void Show()
    {
        gameObject.SetActive(true); // On rend visible l'Inventory
        DeselectAllItems(); // Plus de bordures ni de description
        InitializeRecipesUI(); // On recommence l'initialize au cas ou entre temps une recette a été découverte

        playerController.UIOpen = true;

        if (InputManager.GetInstance().GetDevice() != "Keyboard") // Gamepad
        {
            gamepadCursor.gameObject.SetActive(true);
        }
        else
        {
            Cursor.visible = true;
        }
    }


    public void Hide()
    {
        playerController.UIOpen = false;
        gameObject.SetActive(false); // On cache l'Inventory
        if (InputManager.GetInstance().GetDevice() != "Keyboard") // Gamepad
        {
            gamepadCursor.gameObject.SetActive(false);
        }
        else
        {
            Cursor.visible = false;
        }
    }

    public void DeselectAllItems() // Enlever la bordure de chaque item de l'inventory
    {
        foreach (UICraftItem item in listOfCrafts)
        {
            item.Deselect();
        }
    }

    public void UpdateDescription(int itemIndex, RecipeSO recipe)
    {
        recipeDescription.SetDescription(recipe);   // Update des valeurs de la description
        DeselectAllItems();
        listOfCrafts[itemIndex].Select(); // On montre les bordures car l'item est sélectionné si on voit sa description
    }


}
