using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //

namespace Inventory.UI
{
    public class ItemActionPanel : MonoBehaviour
    {
        // ----- VARIABLES ----- //
        [SerializeField]
        private GameObject buttonPrefab;
        // ----- VARIABLES ----- //

        public void AddButton(string name, Action onClickAction)
        {
            GameObject button = Instantiate(buttonPrefab, transform); // Copie du bouton
            button.GetComponent<Button>().onClick.AddListener(() => onClickAction()); // Si click bouton -> appel de onClickAction
            button.GetComponentInChildren<TMPro.TMP_Text>().text = name; // On change son texte, ex : "eat"
        }
                                                                                                                                                         
        public void Toggle(bool val) // Affichage ou non de l'item action panel
        {
            if (val == true)
                RemoveOldButtons();
            gameObject.SetActive(val);
        }

        public void RemoveOldButtons()
        {
            foreach(Transform transformChildObjects in transform) // Pour chaque transform des enfants du panel
            {
                Destroy(transformChildObjects.gameObject); // On détruit les boutons
            }
        }
    }


}

