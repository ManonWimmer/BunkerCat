using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsController : MonoBehaviour
{
    // ----- VARIABLES ----- //
    private Dictionary<int, InventoryItem> inventoryItems;

    [SerializeField]
    private InventorySO inventory;

    public static QuestsController instance;

    [SerializeField]
    public QuestsSO playerQuests;

    private QuestSO quest;

    private Dictionary<ItemSO, int> itemsInInventory;

    private List<CraftingItem> requiredItems;
    private List<ItemSO> requiredItemsOnly;

    private RecipesSO playerRecipes;
    private RecipeSO recipeSceptre;
    private RecipeSO recipeEpeeOr;

    private CraftingController craftingController;

    [SerializeField]
    private GameObject winMenu;

    [SerializeField]
    private PlayerController playerController;

    // ----- VARIABLES ----- //

    public static QuestsController GetInstance()
    {
        return instance;
    }

    public InventorySO GetInventory()
    {
        return inventory;
    }

    public QuestsSO GetPlayerQuests()
    {
        return playerQuests;
    }

    private void Awake()
    {
        instance = this;
        craftingController = GameObject.Find("CraftingTable").GetComponent<CraftingController>();
    }

    private void Start()
    {
        playerRecipes = CraftingUIController.GetInstance().GetPlayerRecipes();

        recipeSceptre = playerRecipes.GetItemAt(5);
        recipeEpeeOr = playerRecipes.GetItemAt(4);

        recipeSceptre.isDiscovered = false;
        recipeEpeeOr.isDiscovered = false;

        // On met toutes les quetes � 0
        for (int i = 0; i < playerQuests.Quests.Count; i++)
        {
            playerQuests.GetQuestAt(i).isStarted = false;
            playerQuests.GetQuestAt(i).isCompleted = false;
        }
    }

    public void GetQuestState(int questId, bool isStarted, bool isCompleted)
    {
        quest = playerQuests.GetQuestAt(questId);
        Debug.Log("Quest NPC: " + quest.QuestNPC + " started : " +  isStarted + " completed : " + isCompleted);
    }

    public QuestSO GetQuestAt(int index)
    {
        return playerQuests.GetQuestAt(index);
    }

    public void StartQuest(int questId)
    {
        quest = playerQuests.GetQuestAt(questId);
        Debug.Log("Start Quest NPC : " + quest.QuestNPC);
        quest.isStarted = true;

        if (questId == 1) // Si on parle � Gustavo
        {
            // On active le craft
            craftingController.craftingLearned = true;    
        }

        if (questId == 2) // Si on parle avec Julia
        {
            // On d�bloque le craft de l'�p�e en or 
            recipeEpeeOr.isDiscovered = true;

        }
    }

    public bool CheckCanCompleteQuest(int questId)
    {
        quest = playerQuests.GetQuestAt(questId);
        SearchQuestItemsInInventory(quest);
        Debug.Log("check : " + quest.CanBeCompleted);
        return quest.CanBeCompleted;
    }
        

    public void CompleteQuest(int questId)
    {
        quest = playerQuests.GetQuestAt(questId);
        Debug.Log("Complete Quest NPC : " + quest.QuestNPC);
        // Si c'est appel� c'est que canBeCompleted est forc�ment �gal � true donc on a pas besoin de v�rifier
        
        if (questId == 0)
        {
            Debug.Log("fin du jeu");
            playerController.gameFinished = true;
            winMenu.SetActive(true);
        }


        if (questId == 1) // Quest de Gustavo, on d�bloque la recette de craft du sceptre de saphir
        {
            recipeSceptre.isDiscovered = true;

            // OUBLI : MARTEAU A ENLEVE
            Debug.Log(recipeSceptre.isDiscovered);

            SearchQuestItemsInInventory(quest); // Check des ingr�dients de la recipe (au cas ou)
            if (quest.CanBeCompleted) // Normalement c'est oui mais au cas ou on check
            {
                Debug.Log("craft r�ussi");
                InventorySO inventory = craftingController.GetInventory();
                // On v�rifie la place dans l'inventaire :
                // Retirer items de l'inventaire :

                        // On r�cup�re la liste des items de l'inventory
                        List<InventoryItem> inventoryItems = new List<InventoryItem>(inventory.GetInventoryList());

                        // On r�cup�re la liste des items required par la recipe
                        List<CraftingItem> requiredItems = new List<CraftingItem>(quest.RequiredItems);

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
                        
                        // Craft
                        //Debug.Log("CRAFT ITEM INVENTAIRE PAS PLEIN");

                        // Update inventory dans la recipe ouverte pour changer les ingr�dients restants

                }
            }
            else // Recipe peut pas �tre craft
            {
                //Debug.Log("craft loup�");
                // Mettre un texte d'erreur pas assez d'ingr�dients ou juste bordure gris�e !! ?
            }

        }


        if (questId == 2) // Quest de Julia, pour craft la cl� en fer
        {
            //on enleve de l'invezntory les items + on y ajoute le result craft (la cl�)

            SearchQuestItemsInInventory(quest); // Check des ingr�dients de la recipe (au cas ou)
            if (quest.CanBeCompleted) // Normalement c'est oui mais au cas ou on check
            {
                Debug.Log("craft r�ussi");
                InventorySO inventory = craftingController.GetInventory();
                // On v�rifie la place dans l'inventaire :
                if (quest.ResultCraft.Item.IsStackable) // Si le le result craft est stackable :
                {
                    if (inventory.AddStackableItem(quest.ResultCraft.Item, quest.ResultCraft.Quantity) == 0)
                    {
                        // Retirer items de l'inventaire :

                        // On r�cup�re la liste des items de l'inventory
                        List<InventoryItem> inventoryItems = new List<InventoryItem>(inventory.GetInventoryList());

                        // On r�cup�re la liste des items required par la recipe
                        List<CraftingItem> requiredItems = new List<CraftingItem>(quest.RequiredItems);

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
                        //UIRecipeDescription.GetInstance().SetDescription(recipeSelected);
                    }
                    else
                    {
                        //Debug.Log("CRAFT ITEM INVENTAIRE PLEIN");
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
        quest.isCompleted = true;
    }

    private void UpdateInventoryItems()
    {
        inventoryItems = inventory.GetCurrentInventoryState();
    }

    public void SearchQuestItemsInInventory(QuestSO quest)
    {
        UpdateInventoryItems();

        requiredItems = quest.RequiredItems;

        quest.CanBeCompleted = false;
        itemsInInventory = new Dictionary<ItemSO, int>();

        // On regarde chaque (cl� : index, valeur : InventoryIndex (item, quantity, isEmpty)) des �l�ments de l'inventaire
        // On cr�e un dictionnaire avec seulement les ingr�dients pour la recipe

        GetListRequiredItems(quest); // List avec seulement les ItemSO de la recette (pas leur quantit�)

        if (inventory.Size > 0)
        {
            for (int i = 0; i < inventory.Size; i++)
            {
                if (inventoryItems.ContainsKey(i) && !inventoryItems[i].IsEmpty)
                {
                    if (requiredItemsOnly.Contains(inventoryItems[i].item)) // Meme item qu'un ingr�dient
                    { // On l'ajoute au dict

                        if (itemsInInventory.ContainsKey(inventoryItems[i].item)) // Si il est d�j� dans le dict
                        {
                            itemsInInventory[inventoryItems[i].item] += inventoryItems[i].quantity; // On ajoute la quantit�
                        }
                        else // Sinon on le cr�e dans le dict
                        {
                            itemsInInventory[inventoryItems[i].item] = inventoryItems[i].quantity;
                        }
                    }
                } // Sinon si empty, on fait rien
            }
        }

        CheckItemsQuantities(quest, itemsInInventory);
    }

    public int GetItemInventoryQuantity(CraftingItem item)
    {
        UpdateInventoryItems();
        int itemQuantityInventory = 0;

        if (inventoryItems.Count > 0)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (!inventoryItems[i].IsEmpty)
                {
                    if (item.Item == inventoryItems[i].item) // Meme item que l'ingr�dient
                    {
                        itemQuantityInventory += inventoryItems[i].quantity; // On ajoute la quantit�
                    }
                } // Sinon si empty, on fait rien
            }
        }

        return itemQuantityInventory;
    }

    private void CheckItemsQuantities(QuestSO quest, Dictionary<ItemSO, int> itemsInInventory)
    {
        // On parcourt le dict (si il est rempli, sinon false) - d�s qu'une des quantit�s est inf�rieure � celle demand�, canCraft = false, sinon true

        requiredItems = quest.RequiredItems;

        foreach (CraftingItem itemQuest in requiredItems) // Pour chaque item requis de la recette
        {
            ItemSO questItem = itemQuest.Item; // On r�cup�re l'Item SO des ingr�dients n�cessaires

            if (itemsInInventory.ContainsKey(questItem)) // Ingr�dient dans l'inventaire
            {
                int quantityInventory = itemsInInventory[questItem];

                if (quantityInventory >= itemQuest.Quantity) // Ingr�dient dans l'inventaire avec une quantit� suffisante pour craft : craftable pour l'instant
                {
                    quest.CanBeCompleted = true;
                    //Debug.Log(recipeItem.Name + ":" + quantityInventory.ToString() + "/" + itemRecipe.Quantity.ToString());
                }
                else // Ingr�dient dans l'inventaire mais pas assez de quantit� : false
                {
                    quest.CanBeCompleted = false;
                    //Debug.Log(recipeItem.Name + ":" + quantityInventory.ToString() + "/" + itemRecipe.Quantity.ToString());
                    //Debug.Log("return false");
                    return;
                }
            }
            else // D�s qu'un ingr�dient n�cessaire � la recette n'est pas dans l'inventaire, c'est incraftable
            {
                quest.CanBeCompleted = false;
                //Debug.Log("return false");
                return;
            }
        }
    }

    private void GetListRequiredItems(QuestSO quest)
    {
        requiredItemsOnly = new List<ItemSO>();

        foreach (CraftingItem item in quest.RequiredItems)
        {
            requiredItemsOnly.Add(item.Item); // On ajoute que les ItemSO (sans leur quantit�) dans le 
        }
    }
}
