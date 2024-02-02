using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Pour accéder aux éléments de l'UI, comme Image
using TMPro; // TextMeshPro

public class TextTrigger : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // Text on Trigger :
    public string textToDisplay;

    public GameObject triggerTextGameObject;
    private TMP_Text textInstance;
    private GameObject textInstanceGameObject;

    public Canvas UI_Canvas;

    // ----- VARIABLES ----- //

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Le joueur entre dans le trigger
        {
            textInstanceGameObject = Instantiate(triggerTextGameObject); // Copie du texte vide
            textInstanceGameObject.transform.SetParent(UI_Canvas.transform, false); // On met la copie en enfant du Canvas
            textInstance = textInstanceGameObject.GetComponent<TMP_Text>(); // On récupère le component TMP_Text pour la ligne d'après
            textInstance.text = textToDisplay; // On met à jour son texte
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Le joueur qui le trigger
        {
            Destroy(textInstanceGameObject); // On détruit le texte
        }
    }

}