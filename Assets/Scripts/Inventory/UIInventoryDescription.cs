using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Tracing;
using Inventory.Model;

namespace Inventory.UI
{
    public class UIInventoryDescription : MonoBehaviour
    {
        // ----- VARIABLES ----- //
        [SerializeField]
        private Image itemImage; // Icone de l'item

        [SerializeField]
        private TMP_Text title; // Nom de l'item

        [SerializeField]
        private TMP_Text description; // Description de l'item

        [SerializeField]
        private GameObject buttonBoire;

        [SerializeField]
        private InventorySO inventory;

        public int itemSelectedIndex;
        // ----- VARIABLES ----- //

        public void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            this.itemImage.gameObject.SetActive(false); // On cache l'icone de l'item
            this.title.text = ""; // On met le nom de l'item vide
            this.description.text = ""; // On met la description de l'item vide
            itemSelectedIndex = 0;
        }

        public void SetDescription(Sprite sprite, string itemName, string itemDescription, int itemIndex)
        {
            this.itemImage.gameObject.SetActive(true); // On affiche l'icone de l'item
            this.itemImage.sprite = sprite; // On change l'icone de l'item
            this.title.text = itemName; // On change le nom de l'item
            this.description.text = itemDescription; // On change la description de l'item
            itemSelectedIndex = itemIndex;
            InventoryItem inventoryItem = inventory.GetItemAt(itemIndex);

            IItemAction itemAction = inventoryItem.item as IItemAction;

            if (itemAction != null)
            {
                // On affiche le menu
                buttonBoire.SetActive(true);
            }
            else
            {
                // On affiche pas le menu
                buttonBoire?.SetActive(false);
            }
        }

        


    }
}

