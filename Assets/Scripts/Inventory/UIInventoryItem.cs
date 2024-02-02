using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
# if UNITY_EDITOR
using UnityEditor;
# endif

namespace Inventory.UI
{
    
    public class UIInventoryItem : MonoBehaviour
    {
        // ----- VARIABLES ----- //
        public Image itemImage; // Icone de l'item

        public TMP_Text quantityTxt; // Nombre de l'item dans l'inventaire

        public Image borderImage; // Border de l'image quand il est sélectionné

        public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRigthMouseBtnClick;
        // On veut pouvoir cliquer sur l'item, le relacher, le drag et afficher des options supplémentaires quand on clic droit (ex : utiliser, lacher)

        private bool isEmpty = true; // Item vide ? pour savoir si on appelle les action du dessus
        // ----- VARIABLES ----- //

        public void Awake()
        {
            SetValues();
            ResetData();
            Deselect();
        }

        private void SetValues()
        {
            itemImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            borderImage = transform.GetChild(0).GetComponent<Image>();
        }

        public void ResetData()
        {
            Debug.Log(itemImage);
            itemImage?.gameObject?.SetActive(false); // On cache l'icone et la quantity
            isEmpty = true; // L'item est vide
        }

        public void Deselect()
        {
            borderImage.enabled = false; // On cache la bordure de l'item
        }

        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true); // On affiche l'icone et la quantity de l'item
            itemImage.sprite = sprite; // On change son icone
            quantityTxt.text = quantity.ToString(); // On change sa quantité
            isEmpty = false; // L'item n'est plus vide
        }

        public void Select()
        {
            borderImage.enabled = true; // On affiche la border de l'image pour montrer qu'elle est sélectionnée
        }

        public void OnBeginDrag()
        {
            if (isEmpty)
            {
                return; // On veut pas le drag si il est vide
            }
            OnItemBeginDrag?.Invoke(this); // Appel de l'action event si il n'est pas déjà en cours
        }
        public void OnEndDrag()
        {
            OnItemEndDrag?.Invoke(this); // Appel de l'action event si il n'est pas déjà en cours
        }

        public void OnDrop()
        {
            OnItemDroppedOn?.Invoke(this); // Appel de l'action event si il n'est pas déjà en cours
        }

        public void OnPointerClick(BaseEventData data)
        {
            Debug.Log("click");
            OnItemClicked?.Invoke(this);

            /*
            PointerEventData pointerData = (PointerEventData)data; // On a besoin du pointer de la souris pour savoir quel clic
            if (pointerData.button == PointerEventData.InputButton.Right) // Clic droit
            {
                OnRigthMouseBtnClick?.Invoke(this); // Appel de l'action event si il n'est pas déjà en cours
            }
            else if (pointerData.button == PointerEventData.InputButton.Left) // Clic gauche
            {
                OnItemClicked?.Invoke(this); // Appel de l'action event si il n'est pas déjà en cours
            }*/
        }
    }
}
