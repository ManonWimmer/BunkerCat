using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused;
    public GamepadCursor gamepadCursor;
    private bool gameFinished;

    private void Start()
    {
        DeactivatePause();
        gameFinished = false;
    }

    private void Update()
    {
        
        if (!gameFinished)
        {
            if (pauseMenu.activeSelf) // pause ouvert
            {
                if (InputManager.GetInstance().GetDevice() != "Keyboard") // Gamepad
                {
                    gamepadCursor.gameObject.SetActive(true);
                    Cursor.visible = false;
                }
                else
                {
                    gamepadCursor.gameObject.SetActive(false);
                    Cursor.visible = true;
                }
            }

            bool pausePressed = InputManager.GetInstance().GetPausePressed();
            if (pausePressed && !isPaused)
            {
                ActivatePause();
            }
            else if (pausePressed && isPaused)
            {
                DeactivatePause();
            }
        }
    }

    public void ActivatePause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Freeze du jeu
    }

    public void DeactivatePause()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        gamepadCursor?.gameObject.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void LockPauseWhenGameFinished()
    {
        gameFinished = true;
        Debug.Log(gameFinished);
    }
}
