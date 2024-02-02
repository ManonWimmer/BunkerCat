using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        // ----- VARIABLES ----- //
        // Inventory Content :
        [SerializeField]
        private UIInventoryItem itemPrefab;

        [SerializeField]
        private RectTransform contentPanel;

        // Inventory Description :
        [SerializeField]
        private UIInventoryDescription itemDescription;

        // Drag :
        [SerializeField]
        private MouseFollower mouseFollower;

        List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>(); // List des items dans l'inventory

        // Swap :
        private int currentlyDraggedItemIndex = -1; // -1 pour que ce soit en dehors de la liste et que du coup on ne drag aucun item

        // Event actions : (int = index de l'item)
        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
        public event Action<int, int> OnSwapItems;

        // Item action menu :
        [SerializeField]
        private ItemActionPanel actionPanel;

        public GamepadCursor gamepadCursor;

        public PlayerController playerController;
        // ----- VARIABLES ----- //

        // Inventory Description :

        private void Awake()
        {
            itemDescription.ResetDescription();
            //listOfUIItems.Clear();

            // Drag :
            mouseFollower.Toggle(false);
            Hide();
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        }
        
        private void Update()
        {
            if(gameObject.activeSelf) // inventaire ouvert
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

        // Inventory Content :
        public void InitializeInventoryUI(int inventorySize)
        {
            Debug.Log("initialize inventory ui");
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity); // On crée un nouvel item (Quaternion identity = no rotation)
                
                uiItem.transform.SetParent(contentPanel); // On le met dans le panel InventoryContent
                uiItem.transform.localScale = new Vector3(1, 1, 1);
                listOfUIItems.Add(uiItem); // On l'ajoute à la liste
                Debug.Log(listOfUIItems);

                // Appel des méthodes pour chaque action :
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnRigthMouseBtnClick += HandleShowItemActions;
            }
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity) // Changement de l'icone et de la quantity d'un item à un index de la liste d'items
        {
            Debug.Log("update data");
            if (listOfUIItems.Count > itemIndex) // Index dans la liste des items
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity); // On update sa data
            }
        }

        private void HandleShowItemActions(UIInventoryItem inventoryItemUI) 
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI); // On récupère l'index de l'item que l'on drag

            if (index == -1) // Si on drag rien, hors de la liste des items
            {
                return; // On arrete tout
            }

            OnItemActionRequested?.Invoke(index);
        }

        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            // Swap :
            int index = listOfUIItems.IndexOf(inventoryItemUI); // On récupère l'index de l'item que l'on drag

            if (index == -1) // Si on drag rien, hors de la liste des items
            {
                return; // On arrete tout
            }

            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index); // Swap des items aux 2 indexs
            HandleItemSelection(inventoryItemUI); // Conserver la sélection de l'objet bougé ///
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false); // On arrete d'afficher le mouseFollower donc on arrete le drag
            currentlyDraggedItemIndex = -1;
        }

        public void CreateDraggedItem(Sprite sprite, int quantity) // Activation du mouseFollower quand on drag un item
        {
            mouseFollower.Toggle(true); // On active le mouseFollower
            mouseFollower.SetData(sprite, quantity); // On affiche la bonne icone et la bonne quantité de l'item dragged
        }

        private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            // Swap :
            int index = listOfUIItems.IndexOf(inventoryItemUI); // On récupère l'index de l'item que l'on drag

            if (index == -1) // Si on drag rien, hors de la liste des items
                return; // On arrete tout

            currentlyDraggedItemIndex = index; // On a vérifié que l'item drag n'était pas vide donc on peut prendre son index

            // Selection :
            HandleItemSelection(inventoryItemUI);

            // Drag :
            OnStartDragging?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            // Drag :
            ResetDraggedItem();
        }


        private void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI); // On récupère l'index de l'item que l'on drag

            if (index == -1) // Si on drag rien, hors de la liste des items
                return; // On arrete tout

            // Description :
            OnDescriptionRequested?.Invoke(index);
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        // Item action menu :
        public void ShowItemAction(int itemIndex) 
        {
            actionPanel.Toggle(true); // On affiche l'action panel
            actionPanel.transform.position = listOfUIItems[itemIndex].transform.position; // On change sa position pour le slot d'inventaire sélectionné
        }

        // Item action menu :
        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButton(actionName, performAction); // On ajoute le boutton dans l'action panel
        }

        public void DeselectAllItems() // Enlever la bordure de chaque item de l'inventory
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }

            // Item action menu :
            actionPanel.Toggle(false); // On cache l'action item menu
        }

        public void Show()
        {
            gameObject.SetActive(true); // On rend visible l'Inventory
            playerController.ChangeUIOPen(true);
            ResetSelection(); // Plus de bordures ni de description
            
            if (InputManager.GetInstance().GetDevice() != "Keyboard") // Gamepad
            {
                gamepadCursor.gameObject.SetActive(true);
            }
            else // Cursor souris pc
            {
                Cursor.visible = true;
            }
        }

        public void Hide()
        {
            playerController.ChangeUIOPen(false);
            actionPanel.Toggle(false);
            gameObject.SetActive(false); // On cache l'Inventory
            ResetDraggedItem(); // Si jamais on ferme l'inventaire quand on drag un item

            Cursor.visible = false;
            gamepadCursor?.gameObject?.SetActive(false);
        }

        public void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description, itemIndex);   // Update des valeurs de la description
            DeselectAllItems();
            listOfUIItems[itemIndex].Select(); // On montre les bordures car l'item est sélectionné si on voit sa description
        }

        public void ResetAllItems() ///
        {
            foreach (var item in listOfUIItems) // Pour chaque item de l'inventaire
            {
                item.ResetData(); // On le rend vide
                item.Deselect(); // On le déselectionne
            }
        }
    }

}
