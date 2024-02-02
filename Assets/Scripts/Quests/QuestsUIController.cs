using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsUIController : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private UIQuestsPage uiQuestsPage;

    public bool isQuestsOpen = false;

    public static QuestsUIController instance;

    private void Awake()
    {
        instance = this;
    }

    public static QuestsUIController GetInstance() { return instance; }


    private void Update()
    {

        if (InputManager.GetInstance().GetQuestsPressed())
        {
            if (uiQuestsPage.isActiveAndEnabled == false) // Si il est caché
            {
                uiQuestsPage.Show();
                isQuestsOpen = true;
                
            }
            else // Sinon, il est visible
            {
                uiQuestsPage.Hide(); // On le cache
                isQuestsOpen = false;
            }
        }
    }
}
