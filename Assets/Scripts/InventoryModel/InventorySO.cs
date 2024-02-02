using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq; // Pour Where()
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        // ----- VARIABLES ----- //
        [SerializeField]
        private List<InventoryItem> inventoryItems { get; set; } // Liste de tous les items (SO) de l'inventaire

        [field: SerializeField]
        public int Size { get; private set; } = 24; // Taille de l'inventaire (nombre de slots pour les items) qu'on ne set pas publiquement mais qu'on get seulement

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public static InventorySO instance;

        // Ce qui est bien avec struct : la valeur de InventorySO peut être null
        // ----- VARIABLES ----- //
        /*
        private void Awake()
        {
            instance = this;
        }

        public InventorySO GetInstance()
        {
            return this;
        }
        */
        public List<InventoryItem> GetInventoryList()
        {
            return inventoryItems;
        }

        public void Initialize()
        {
            Debug.Log("inventory initialize");
            inventoryItems = new List<InventoryItem>(); // Création de la liste
            for (int i = 0; i < Size; i++) // Pour chaque slot d'item de l'inventory
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem()); // On recup dans la liste les slots vides (donc tous au départ)
            }
        }

        public int AddItem(ItemSO item, int quantity)
        {
            Debug.Log("add item " + item.Name + quantity);
            if (item.IsStackable == false)
            {
                for (int i = 0; i < inventoryItems.Count; i++) // Pour chaque slot de l'inventory
                {
                    while(quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddNonStackableItem(item, 1);
                    }
                    
                    InformAboutChange(); // Update
                    return quantity;
                } 
            }
            // Stack :
            quantity = AddStackableItem(item, quantity); // On ajoute l'objet a un stack
            InformAboutChange(); // Update
            return quantity;
        }

        public int AddNonStackableItem(ItemSO item, int quantity)
        {
            InventoryItem newItem = new InventoryItem // Copie de l'item
            {
                item = item,
                quantity = quantity
            };

            for (int i = 0; i < inventoryItems.Count; i++) 
            {
                if (inventoryItems[i].IsEmpty) // On cherche dans la liste des slots de l'inventaire le premier vide
                {
                    inventoryItems[i] = newItem; // On y met l'item
                    return quantity; 
                }
            }

            return 0; // On a pas trouvé de slot vide donc on renvoie sa quantity = 0 car il ne se trouve pas dans l'inventaire
        }

        private bool IsInventoryFull() => inventoryItems.Where(item => item.IsEmpty).Any() == false; // Foreach loop des items ou on regarde si ils sont empty ou pas
        // Si ils sont tous pas empty, l'inventaire est donc complet

        // Stack :
        public int AddStackableItem(ItemSO item, int quantity)
        {
            Debug.Log("add stackable item");
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty) // Si on tombe sur un slot vide, ce n'est pas ce qu'on recherche
                {
                    continue;
                }

                if (inventoryItems[i].item.ID == item.ID) // Meme ID donc meme item (ex : 2 gems)
                {
                    int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity; // Place dans le stack max dispo
                                                                                                                 // en fonction de la quantity de l'item a stack
                    
                    if(quantity > amountPossibleToTake) // Si on depasse la place disponible pour le stack 
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize); // On met la quantite au max au stack existant
                        quantity -= amountPossibleToTake; // On change la quantite restante de l'item
                    }
                    else // Si il y a assez de place pour toute la quantite de l'item dans le stack deja present dans l'inventaire
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity); // On ajoute la quantite de l'item au stack deja
                                                                                                                     // créé sans qu'il excede le stack max 
                        InformAboutChange(); // Update
                        return 0; // quantity = 0 car la quantite de l'item a été complètement ajoutée à un stack, ou plusieurs
                    }
                }
            }

            while(quantity > 0 && IsInventoryFull() == false) // Tant qu'il reste de la quantite a ajouter et qu'il y a de la place pour dans l'inventaire
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize); // Clamp pour pas qu'il dépasse la quantity du stack max !
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity); // On crée un nouveau stack
            }

            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity)
        {
            InventoryItem newItem = new InventoryItem // Copie de l'item
            {
                item = item,
                quantity = quantity
            };

            for (int i = 0; i < inventoryItems.Count; i++) // Boucle dans l'ordre de l'inventaire
            {
                if (inventoryItems[i].IsEmpty) // Dès qu'un slot est vide
                {
                    inventoryItems[i] = newItem; // On y met l'item
                    return quantity;
                }
            }

            return 0; // Quantity = 0 car il n'y a pas de place
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState() // Int = clé du dictionnaire, index dans la liste d'items
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();

            for (int i = 0; i < inventoryItems.Count; i++) // Pour chaque slot de l'inventory
            {
                if (inventoryItems[i].IsEmpty) // Si le slot est vide
                {
                    continue; // On continue la boucle 
                }
                else
                {
                    returnValue[i] = inventoryItems[i]; // On renvoie l'item non vide
                }
            }
            return returnValue; // Renvoie un dictionnaire avec tous les items de l'inventaire (clé : index, valeur : item)
        }

        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void AddItem(InventoryItem item) 
        {
            AddItem(item.item, item.quantity);
        }

        public void SwapItems(int itemIndex1, int itemIndex2) 
        {
            InventoryItem item1 = inventoryItems[itemIndex1]; // On récup l'item 1
            inventoryItems[itemIndex1] = inventoryItems[itemIndex2]; // On met l'item 2 à l'emplacement de l'item 1
            inventoryItems[itemIndex2] = item1; // On met l'item 1 à l'emplacement de l'item 2
            InformAboutChange(); // Update data
        }

        private void InformAboutChange() // Update data
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState()); // Génération du nouveau dictionnaire
        }

        public void RemoveItem(int itemIndex, int quantity)
        {
            if (inventoryItems.Count > itemIndex) // Index dans la liste des items de l'inventaire
            {
                if (inventoryItems[itemIndex].IsEmpty)
                    return; // Si il est vide, on fait rien

                int reminder = inventoryItems[itemIndex].quantity - quantity;
                if (reminder <= 0) // Si on doit enlever + que ce qu'on a comme quantité ou autant
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem(); // On vide le slot 
                else // Il reste une certaine quantité apres le remove
                    inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(reminder); // On change sa quantité

                InformAboutChange(); // Update
            }
        }
    }

    [Serializable]
    public struct InventoryItem // Mieux que class pour des petites data structures avec juste juste des valeurs, pas les valeurs des objets
    {
        // ----- VARIABLES ----- //
        public int quantity;
        public ItemSO item;
        public bool IsEmpty => item == null;
        // ----- VARIABLES ----- //

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = this.item, // Duplication de l'item
                quantity = newQuantity // Changement de sa quantité
            };
        }

        public static InventoryItem GetEmptyItem() => new InventoryItem // Unity doc : Static variables are shared across all instances of a class !!!
        {
            item = null,
            quantity = 0
        };
    }
}
