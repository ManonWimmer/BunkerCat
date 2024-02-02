using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.UI;
using Inventory.Model;



namespace Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        // ----- VARIABLES ----- //
        [SerializeField]
        private UIInventoryPage inventoryUI; // Tout l'UI de l'Inventory

        [SerializeField]
        private InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem> (); // Items dans l'inventaire au début du jeu

        // SFX :
        [SerializeField]
        private AudioClip dropClip;

        [SerializeField]
        private AudioSource audioSource;
        // ----- VARIABLES ----- //

        private void Start()
        {
            PrepareUI(); // Création des items dans l'inventaire et les ajouter à une liste + events
            PrepareInventoryData(); 
        }

        private void PrepareInventoryData() 
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI; 
            foreach (InventoryItem item in initialItems) // Pour chaque item de l'inventaire de départ :
            {
                if (item.IsEmpty)
                {
                    continue;
                }
                else
                {
                    inventoryData.AddItem(item); 
                }
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState) 
        {
            Debug.Log("update inventory ui");
            inventoryUI.ResetAllItems(); // On met tous les items vides, à créer
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity); // On update tous les items
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);

            // Assignement des events :
            this.inventoryUI.OnDescriptionRequested += HandleDescriptionRequested;
            this.inventoryUI.OnSwapItems += HandleSwapItems;
            this.inventoryUI.OnStartDragging += HandleDragging;
            this.inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }
       
        // =
        private void OnDisable()
        {
            this.inventoryData.OnInventoryUpdated -= UpdateInventoryUI;
            this.inventoryUI.OnDescriptionRequested -= HandleDescriptionRequested;
            this.inventoryUI.OnSwapItems -= HandleSwapItems;
            this.inventoryUI.OnStartDragging -= HandleDragging;
            this.inventoryUI.OnItemActionRequested -= HandleItemActionRequest;
        }

        // Events :
        private void HandleDescriptionRequested(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.Name, item.Description);

        }

        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex); // On récupère l'item drag

            if (inventoryItem.IsEmpty)
            {
                return; // On ne veut pas drag un item vide
            }
            else // Sinon, on peut le drag !
            {
                inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity); 
            }
        }
        private void HandleItemActionRequest(int itemIndex) ///
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);

            if (inventoryItem.IsEmpty) // On vérifie que l'item ne soit pas vide
                return;

            IItemAction itemAction = inventoryItem.item as IItemAction;

            if (itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex); // Affichage de l'action menu
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;

            if (destroyableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity)); // Bouton pour drop l'item
            }
        }

        

    // Item action menu :
    public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);

            if (inventoryItem.IsEmpty) // On vérifie que l'item ne soit pas vide
                return;

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;

            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;

            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject); // Perform Action donc par exemple heal
                audioSource.PlayOneShot(itemAction.actionSFX);

                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                    inventoryUI.ResetSelection();
            }
        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity); // On jette l'item
            inventoryUI.ResetSelection(); // On le déselecte
            audioSource.PlayOneShot(dropClip); // Son de drop d'item
        }

        private void Update()
        {

            if (InputManager.GetInstance().GetInventoryPressed())
            {
                if (inventoryUI.isActiveAndEnabled == false) // Si il est caché
                {
                    inventoryUI.Show();
                    //
                    foreach (var item in inventoryData.GetCurrentInventoryState()) // item = (Key = index, Value = itemSO)
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                    }
                    //
                }
                else // Sinon, il est visible
                {
                    inventoryUI.Hide(); // On le cache
                }
            }
        }
    }

}
