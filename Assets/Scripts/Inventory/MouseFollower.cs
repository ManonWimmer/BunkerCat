using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Inventory.UI;

public class MouseFollower : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private Canvas canvas; // Canvas de l'UI

    [SerializeField]
    private UIInventoryItem item; // Item qui doit suivre la souris lorsqu'on le drag
    // ----- VARIABLES ----- //

    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>(); // On récupère le Canvas de la scène
        item = GetComponentInChildren<UIInventoryItem>(); // On récupère l'item qui est en enfant de MouseFollower
    }

    public void SetData(Sprite sprite, int quantity)
    {
        item.SetData(sprite, quantity); // On attribue à l'itel son icone et sa quantité
    }

    private void Update()
    {
        Vector2 position;
        // Calcul de la position de la souris :
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position); // On déplace l'image à la position de la souris
    }

    public void Toggle(bool val) // Afficher ou nom le drag de l'item
    {
        gameObject.SetActive(val); // On active ou non le MouseFollower
    }


}
