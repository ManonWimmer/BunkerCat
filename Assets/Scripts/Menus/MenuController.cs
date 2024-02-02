using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject optionsMenu;

    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField]
    private GameObject inputsMenu;
    // ----- VARIABLES ----- //

    private void Start()
    {
        OpenMainMenu();
    }

    private void CloseAllMenus()
    {
        player.SetActive(false);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        inputsMenu.SetActive(false);
    }

    private void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void OpenMainMenu()
    {
        CloseAllMenus();
        OpenMenu(mainMenu);
        player.SetActive(true);
    }

    public void OpenOptionsMenu()
    {
        CloseAllMenus();
        OpenMenu(optionsMenu);
    }

    public void OpenSettingsMenu()
    {
        CloseAllMenus();
        OpenMenu(settingsMenu);
    }

    public void OpenInputsMenu()
    {
        CloseAllMenus();
        OpenMenu(inputsMenu);
    }

}