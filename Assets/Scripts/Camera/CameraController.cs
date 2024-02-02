using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // R�cup�ration de la cible � suivre
    public Transform target;

    // Parallax Horizontale & Verticale
    public Transform farBackground, middleBackground, frontBackground;
    /* private float lastXPos; // Position de la cible */
    private Vector2 lastPos;

    // Bloquer la cam�ra verticalement � une hauteur min et max
    public float minHeight, maxHeight; // Hauteur min et max de la cam�ra
    // ----- VARIABLES ----- //

    void Start()
    {
        // Parallax Horizontale et Verticale :
        /* lastXPos = transform.position.x; // Initialisation de la derni�re position x de la cible */ 
        lastPos = transform.position; // Initialisation de la derni�re position de la cible (x, y) car Vector2
    }

    void Update()
    {
        /* Premi�re version d�placement cam�ra :
     
        // D�placement horizontal de la cam�ra � la position de la cible (sans changer position y et z)
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Bloquer la cam�ra verticalement � une hauteur min et max :
        float clampedY = Mathf.Clamp(transform.position.y, minHeight, maxHeight); // On v�rifie que la position verticale de la cam�ra soit au-dessus de minHeight et en-dessous de maxHeight
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z); */

        // Deuxi�me version d�placement cam�ra :
        transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);

        // Parallax Horizontale et Verticale :
        /* float amountToMoveX = transform.position.x - lastXPos; // Distance � parcourir pour que le far background soit au m�me endroit que la cam�ra */
        Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y); // Distance � parcourir pour que le far background soit au m�me endroit que la cam�ra (x et y)

        farBackground.position += new Vector3(amountToMove.x, amountToMove.y * -0.1f, 0f) * 0.75f; ; 
        middleBackground.position += new Vector3(amountToMove.x, amountToMove.y * -0.1f, 0f) * 0.5f;
        frontBackground.position += new Vector3(amountToMove.x, amountToMove.y * -0.1f, 0f) * 0.25f;

        /* lastXPos = transform.position.x; // Update de la position */
        lastPos = transform.position;
    }
}
