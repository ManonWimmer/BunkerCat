using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [field: SerializeField]
    public ItemSO InventoryItem { get; private set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    [SerializeField]
    private AudioSource audioSource; // Audio lorsqu'on ramasse l'objet

    [SerializeField]
    private float duration = 0.3f; // Temps de l'animation

    // Pickup Effect :
    public GameObject pickupEffect;

    public CircleCollider2D circleCollider;
    public SpriteRenderer spriteRenderer;

    // Progress bar Items
    [SerializeField]
    private SliderItemsController sliderItemsController;
    // ----- VARIABLES ----- //
    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    void Start() 
    {
        circleCollider.enabled = true;
        spriteRenderer.sprite = InventoryItem.ItemImage; // On change recupere l'image de l'item avec celle de l'itemSO associé
    }

    internal void DestroyItem() 
    {
        Debug.Log("destroy item");
        circleCollider.enabled = false; // On desactive son collider
        StartCoroutine(AnimateItemPickup()); // On déclenche son animation de Pickup
    }

    private IEnumerator AnimateItemPickup() 
    {
        Debug.Log("animate item pickup");
        audioSource.Play(); // On joue l'audio
        Instantiate(pickupEffect, transform.position, transform.rotation);

        // Progress bar items :
        sliderItemsController.UpdateProgressItems(Quantity);

        // Animation : on change sa scale pour qu'il soit comme "aspiré" par le joueur
        Vector3 startScale = transform.localScale; // Sa scale actuelle
        Vector3 endScale = Vector3.zero; // Scale finale ou on ne voit plus l'item
        float currentTime = 0; // Compteur

        while (currentTime < duration) // Tant que le temps d'animation n'est pas atteint
        {
            currentTime += Time.deltaTime; // On ajoute le temps écoulé
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration); // On réduit en fonction du temps écoulé sa scale
            yield return null;
        }
        
    }
}
