using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MenuControllerGame : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject optionsMenu;

    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField]
    private GameObject inputsMenu;
    // ----- VARIABLES ----- //

    private void Start()
    {
        CloseAllMenus();
    }

    private void CloseAllMenus()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        inputsMenu.SetActive(false);
    }

    private void OpenMenuGame(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void OpenPauseMenu()
    {
        CloseAllMenus();
        OpenMenuGame(pauseMenu);
    }

    public void OpenOptionsMenuGame()
    {
        CloseAllMenus();
        OpenMenuGame(optionsMenu);
    }

    public void OpenSettingsMenuGame()
    {
        CloseAllMenus();
        OpenMenuGame(settingsMenu);
    }

    public void OpenInputsMenuGame()
    {
        CloseAllMenus();
        OpenMenuGame(inputsMenu);
    }

}