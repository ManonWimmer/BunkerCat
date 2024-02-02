using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICraftButton : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private Image buttonBordure;

    [SerializeField]
    private TMP_Text buttonText;

    private CraftingController craftingController;
    // ----- VARIABLES ----- //
    private void Awake()
    {
        craftingController = GameObject.Find("CraftingTable").GetComponent<CraftingController>();
    }


    public void HandleItemCreation()
    {
        Debug.Log("craft");
        CheckCraft();
    }

    public void CheckCraft()
    {
        // Get recipe selected : 
        RecipeSO recipeSelected = UIRecipeDescription.GetInstance().GetRecipeSelected();
        // Probl�me de recipeSelected : NullReferenceException si aucune recipe n'est s�lectionn�e donc on v�rifie qu'elle ne soit pas null : 
        if (recipeSelected != null)
        {
            //Debug.Log("recipe selected : " + recipeSelected.name);
            craftingController.SearchRecipeIngredientsInInventory(recipeSelected); // Check des ingr�dients de la recipe (au cas ou)
            if (recipeSelected.CanBeCrafted) // Si on a les ingr�dients pour la craft (+ click bouton) :
            {
                //Debug.Log("craft r�ussi");
                InventorySO inventory = craftingController.GetInventory();
                // On v�rifie la place dans l'inventaire :
                if (recipeSelected.ResultCraft.Item.IsStackable) // Si le le result craft est stackable :
                {
                    
                    Debug.Log(recipeSelected.ResultCraft.Item);
                    if (inventory.AddStackableItem(recipeSelected.ResultCraft.Item, recipeSelected.ResultCraft.Quantity) == 0)
                    {
                        // Retirer items de l'inventaire :

                        // On r�cup�re la liste des items de l'inventory
                        List<InventoryItem> inventoryItems = new List<InventoryItem>(inventory.GetInventoryList()); 

                        // On r�cup�re la liste des items required par la recipe
                        List<CraftingItem> requiredItems = new List<CraftingItem>(recipeSelected.RequiredItems);

                        // Boucle pour chaque item de l'inventory
                        //foreach (InventoryItem inventoryItem in inventoryItems)
                        for (int indexItem = 0; indexItem < inventoryItems.Count; indexItem++)
                        {
                            InventoryItem inventoryItem = inventoryItems[indexItem];
                            // On assigne l'item et sa quantit� pour l'item de l'inventory
                            ItemSO item = inventoryItem.item;
                            int quantity = inventoryItem.quantity;


                            if (item != null) // On v�rifie qu'il ne soit pas null et que donc sa quantit� n'est pas �gale � 0
                            {
                                bool itemFound = false;

                                List<CraftingItem> newRequiredItems = new List<CraftingItem>(requiredItems);

                                // On regarde si l'item de l'inventory est dans la liste de required item
                                //foreach (CraftingItem craftingItem in requiredItems)
                                for (int indexRequiredItem = 0; indexRequiredItem < requiredItems.Count; indexRequiredItem++)
                                {
                                    CraftingItem craftingItem = requiredItems[indexRequiredItem];
                                    //Debug.Log("For each Crafting Item : " + craftingItem.Item.Name);
                                    // On assigne l'item et sa quantit� pour l'item required de la recipe
                                    ItemSO itemRequired = craftingItem.Item;
                                    int quantityRequired = craftingItem.Quantity;

                                    if (item == itemRequired && !itemFound)
                                    {
                                        if (quantity < quantityRequired) // Pas assez de quantit�
                                        {
                                            // On enl�ve la quantit� de l'item de l'inventory
                                            inventory.RemoveItem(indexItem, quantity);

                                            // On change la quantit� restante � enlever de l'inventory pour le required item
                                            CraftingItem newCraftingItem;
                                            newCraftingItem.Item = item;
                                            newCraftingItem.Quantity = quantityRequired - quantity;
                                            newRequiredItems[indexRequiredItem] = newCraftingItem;
                                            break;
                                        }
                                        else // Assez de quantit�, sup�rieure ou �gale � celle demand�e
                                        {
                                            // On enl�ve la quantit� de l'item de la recipe 
                                            inventory.RemoveItem(indexItem, quantityRequired);

                                            // On supprime l'item required de la liste d'item required car il est totalement suppr de l'inventory
                                            requiredItems.RemoveAt(indexRequiredItem);
                                        }
                                        itemFound = true; // Pour �viter de le comparer avec les autres items required (uniques)
                                    }
                                }
                                requiredItems = newRequiredItems;
                            }
                        }
                        // Craft
                        //Debug.Log("CRAFT ITEM INVENTAIRE PAS PLEIN");

                        // Update inventory dans la recipe ouverte pour changer les ingr�dients restants
                        UIRecipeDescription.GetInstance().SetDescription(recipeSelected);
                    }
                    else
                    {
                        //Debug.Log("CRAFT ITEM INVENTAIRE PLEIN");
                        // Msg erreur pas assez place inventaire � ajouter
                    }
                }
                else // Si le le result craft n'est pas stackable :
                {
                    if (inventory.AddNonStackableItem(recipeSelected.ResultCraft.Item, recipeSelected.ResultCraft.Quantity) == 0)
                    {
                        // Retirer items de l'inventaire (pareil que pour stackable item, � mettre en fonction ?) :

                        // On r�cup�re la liste des items de l'inventory
                        List<InventoryItem> inventoryItems = new List<InventoryItem>(inventory.GetInventoryList());

                        // On r�cup�re la liste des items required par la recipe
                        List<CraftingItem> requiredItems = new List<CraftingItem>(recipeSelected.RequiredItems);

                        // Boucle pour chaque item de l'inventory
                        //foreach (InventoryItem inventoryItem in inventoryItems)
                        for (int indexItem = 0; indexItem < inventoryItems.Count; indexItem++)
                        {
                            InventoryItem inventoryItem = inventoryItems[indexItem];
                            // On assigne l'item et sa quantit� pour l'item de l'inventory
                            ItemSO item = inventoryItem.item;
                            int quantity = inventoryItem.quantity;


                            if (item != null) // On v�rifie qu'il ne soit pas null et que donc sa quantit� n'est pas �gale � 0
                            {
                                bool itemFound = false;

                                List<CraftingItem> newRequiredItems = new List<CraftingItem>(requiredItems);

                                // On regarde si l'item de l'inventory est dans la liste de required item
                                //foreach (CraftingItem craftingItem in requiredItems)
                                for (int indexRequiredItem = 0; indexRequiredItem < requiredItems.Count; indexRequiredItem++)
                                {
                                    CraftingItem craftingItem = requiredItems[indexRequiredItem];
                                    //Debug.Log("For each Crafting Item : " + craftingItem.Item.Name);
                                    // On assigne l'item et sa quantit� pour l'item required de la recipe
                                    ItemSO itemRequired = craftingItem.Item;
                                    int quantityRequired = craftingItem.Quantity;

                                    if (item == itemRequired && !itemFound)
                                    {
                                        if (quantity < quantityRequired) // Pas assez de quantit�
                                        {
                                            // On enl�ve la quantit� de l'item de l'inventory
                                            inventory.RemoveItem(indexItem, quantity);

                                            // On change la quantit� restante � enlever de l'inventory pour le required item
                                            CraftingItem newCraftingItem;
                                            newCraftingItem.Item = item;
                                            newCraftingItem.Quantity = quantityRequired - quantity;
                                            newRequiredItems[indexRequiredItem] = newCraftingItem;
                                            break;
                                        }
                                        else // Assez de quantit�, sup�rieure ou �gale � celle demand�e
                                        {
                                            // On enl�ve la quantit� de l'item de la recipe 
                                            inventory.RemoveItem(indexItem, quantityRequired);

                                            // On supprime l'item required de la liste d'item required car il est totalement suppr de l'inventory
                                            requiredItems.RemoveAt(indexRequiredItem);
                                        }
                                        itemFound = true; // Pour �viter de le comparer avec les autres items required (uniques)
                                    }
                                }
                                requiredItems = newRequiredItems;
                            }
                        }
                    }
                    else
                    {
                        // Msg erreur pas assez place inventaire � ajouter
                    }      
                }      
            }
            else // Recipe peut pas �tre craft
            {
                //Debug.Log("craft loup�");
                // Mettre un texte d'erreur pas assez d'ingr�dients ou juste bordure gris�e !! ?
            }

        }
    }
    // Change border quand open crafting :
    // Can Be Crafted true -> Bordure blanche :
    // buttonBordure.color = new Color(1f, 1f, 1f, 1f);
    // Can Be Crafted false -> Bordure grise : + Mettre gris�e au start + quand on s�lectionne un item
    // buttonBordure.color = new Color(0.14f, 0.14f, 0.14f, 1f);
}

