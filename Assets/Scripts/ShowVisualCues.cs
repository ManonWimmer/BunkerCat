using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowVisualCues : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [Header("Visual Cues")]

    [SerializeField]
    private GameObject visualCueKeyboard;

    [SerializeField]
    private GameObject visualCueXBOX;

    [SerializeField]
    private GameObject visualCuePS;

    public string device { get; set; } // Keyboard, ControllerXBOX ou ControllerPS
    // ----- VARIABLES ----- //

    private void Awake()
    {
        DesactivateAllCues();
    }

    public void ActivateCueForDevice()
    {
        DesactivateAllCues();
        if (device == "Keyboard")
        {
            visualCueKeyboard.SetActive(true); // Cue clavier E
        }
        else if (device == "ControllerXBOX")
        {
            visualCueXBOX.SetActive(true); // Cue XBOX X
        }
        else if (device == "ControllerPS")
        {
            visualCuePS.SetActive(true); // Cue PS Carré
        }
        else
        {
            Debug.LogError("Device demandé non trouvé pour l'affichage du cue");
        }
    }

    public void DesactivateAllCues()
    {
        visualCueKeyboard.SetActive(false);
        visualCueXBOX.SetActive(false);
        visualCuePS.SetActive(false);
    }
}
