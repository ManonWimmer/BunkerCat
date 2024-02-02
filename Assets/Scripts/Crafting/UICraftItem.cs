using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.ShaderGraph;
#endif


public class UICraftItem : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private Image itemImage; // Icone de l'item

    [SerializeField]
    private Image insideColor; // Icone de l'item

    [SerializeField]
    private TMP_Text itemNameTxt; // Nom de l'item

    [SerializeField]
    private Image borderImage; // Border de l'image quand il est sélectionné

    public event Action<UICraftItem> OnItemClicked;

    private bool isEmpty = true; // Item vide ? pour savoir si on appelle les action du dessus
    // ----- VARIABLES ----- //

    public void Awake()
    {
        Deselect();
    }
    public void Select()
    {
        borderImage.enabled = true; // On affiche la border de l'image pour montrer qu'elle est sélectionnée
    }

    public void Deselect()
    {
        borderImage.enabled = false; // On cache la bordure de l'item
    }

    public void LockItem(RecipeSO recipe)
    {
        //Debug.Log("lock " + recipe.RecipeName);
        itemImage.sprite = recipe.RecipeImage; // On change l'image pour l'icon de la recipe
        itemImage.color = new Color(0f, 0f, 0f, 1f); // Noir
        itemNameTxt.GetComponent<TMP_Text>().text = "???"; // On change son nom pour ???

        // Changement de la couleur de fond :
        insideColor.color = new Color(0.14f, 0.14f, 0.14f, 1f);
    }


    public void UnlockItem(RecipeSO recipe)
    {
        //Debug.Log("unlock " + recipe.RecipeName);
        itemImage.sprite = recipe.RecipeImage; // On change l'image pour l'icon de la recipe
        itemImage.color = new Color(1f, 1f, 1f, 1f); // Blanc donc de couleur

        itemNameTxt.GetComponent<TMP_Text>().text = recipe.RecipeName; // On change son nom pour le nom de la recipe

        // Changement de la couleur de fond :
        insideColor.color = new Color(0, 0, 0, 1f);
    }

    public void SetData(Sprite sprite, string itemName)
    {
        this.itemImage.gameObject.SetActive(true); // On affiche l'icone et la quantity de l'item
        this.itemImage.sprite = sprite; // On change son icone
        this.itemNameTxt.text = itemName; // On change son nom
        this.isEmpty = false; // L'item n'est plus vide
    }


    public void OnPointerClick(BaseEventData data)
    {   
        PointerEventData pointerData = (PointerEventData)data; // On a besoin du pointer de la souris pour savoir quel clic
        if (pointerData.button == PointerEventData.InputButton.Left) // Clic gauche
        {
            //Debug.Log("click");
            OnItemClicked?.Invoke(this); // Appel de l'action event si il n'est pas déjà en cours
        }
            
    }
}

