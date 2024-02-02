using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatMenu : MonoBehaviour
{
    public GameObject defeatMenu;
    public ShowVisualCues showVisualCues;
    public Loading loading;
    public static DefeatMenu instance;

    private bool defeatOpen = false;

    // ----- VARIABLES ----- //

    private void Awake()
    {
        instance = this;
    }

    public static DefeatMenu GetInstance()
    {
        return instance;
    }
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (defeatOpen)
        {
            showVisualCues.device = InputManager.GetInstance().GetDevice();
            showVisualCues.ActivateCueForDevice();
            CheckInput();
        }
    }

    private void CheckInput()
    {
        if (InputManager.GetInstance().GetSubmitPressed())
        {
            // On recommence la partie
            Debug.Log("input de defeat");
            Hide();
            loading.LoadScene();
        }
    }

    public void Show()
    {
        Time.timeScale = 0f;
        defeatMenu.SetActive(true);
        defeatOpen = true;
    }

    public void Hide()
    {
        Time.timeScale = 1f;
        defeatMenu.SetActive(false);
        defeatOpen = false;
    }


}
