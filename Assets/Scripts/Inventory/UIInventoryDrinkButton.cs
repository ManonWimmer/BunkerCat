using Inventory;
using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryDrinkButton : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private InventoryUIController inventoryUIController;

    [SerializeField]
    private UIInventoryDescription uiInventoryDescription;

    [SerializeField]
    private InventorySO inventoryData;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private AudioSource audioSource;

    private int itemSelectedIndex;
    // ----- VARIABLES ----- //

    public void HandleDrinkPotion()
    {
        itemSelectedIndex = uiInventoryDescription.itemSelectedIndex;
        Debug.Log("craft");
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemSelectedIndex);

        IItemAction itemAction = inventoryItem.item as IItemAction;

        if (itemAction != null)
        {
            itemAction.PerformAction(player); // Perform Action donc par exemple heal
            audioSource.PlayOneShot(itemAction.actionSFX);

            inventoryData.RemoveItem(itemSelectedIndex, 1);

            if (inventoryData.GetItemAt(itemSelectedIndex).IsEmpty)
                uiInventoryDescription.ResetDescription();
        }


    }
}
