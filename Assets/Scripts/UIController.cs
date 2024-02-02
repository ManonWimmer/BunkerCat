using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Pour accéder aux éléments de l'UI, comme Image
using TMPro; // TextMeshPro

public class UIController : MonoBehaviour
{
    // ----- VARIABLES ----- //
    // Instance :
    public static UIController instance;

    // Coeurs :
    public Image heart1, heart2, heart3; // Les 3 coeurs à l'écran
    public Sprite heartFull, heartEmpty, heartHalf; // Les différents "types" de coeurs

    // Gems :
    public TMP_Text gemText;
    // ----- VARIABLES ----- //

    private void Awake()
    {
        // Instance :
        instance = this;
    }


    void Start()
    {
        UpdateGemCount();
    }


    void Update()
    {
        
    }

    // Coeurs :
    public void UpdateHealthDisplay()
    {
        switch (PlayerHealthController.instance.currentHealth) // Switch est comme un if sans répéter la même variable
        {
            case 6: // if currentHealth == 6
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;
                break; // finir le case 6

            case 5: // if currentHealth == 5
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartHalf;
                break; // finir le case 5

            case 4: // if currentHealth == 4
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartEmpty;
                break; // finir le case 4

            case 3: // if currentHealth == 3
                heart1.sprite = heartFull;
                heart2.sprite = heartHalf;
                heart3.sprite = heartEmpty;
                break; // finir le case 3

            case 2: // if currentHealth == 2
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                break; // finir le case 2

            case 1: // if currentHealth == 2
                heart1.sprite = heartHalf;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                break; // finir le case 2

            case 0: // if currentHealth == 0
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                break; // finir le case 0

            default: // if currentHealth < 0
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                break; // finir le default
        }
    }

    // Gems :
    public void UpdateGemCount()
    {
        //gemText.text = PlayerController.instance.gemsCollected.ToString(); // Affichage en str du nombre de gems collectés
    }
}
