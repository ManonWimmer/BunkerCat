using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PickupInventory : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private InventorySO inventoryData;
    // ----- VARIABLES ----- //

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>(); // On récupère l'item
        

        if (item != null) // On le ramasse
        {
            Debug.Log("pickup item : " + item.name);

            int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity); // = quantité restante de l'item

            if(reminder == 0) // Si toute la quantité de l'objet a été ajoutée à l'inventaire
            {
                item.DestroyItem(); // On le détruit
            }
            else
            {
                item.Quantity = reminder; // Sinon on le laisse avec sa quantité restante
            }
        }
    }
}
