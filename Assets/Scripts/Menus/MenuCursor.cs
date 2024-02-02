using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCursor : MonoBehaviour
{
    public GamepadCursorMenu gamepadCursor;

    private void Update()
    {
        if (InputManager.GetInstance().GetDevice() != "Keyboard") // Gamepad
        {
            gamepadCursor.gameObject.SetActive(true);
            Cursor.visible = false;
        }
        else
        {
            
            gamepadCursor.gameObject.SetActive(false); // Souris
            Cursor.visible = true;
        }
    }
}
