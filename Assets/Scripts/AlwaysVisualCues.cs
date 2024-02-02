using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysVisualCues : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [SerializeField]
    private ShowVisualCues showVisualCues;
    // ----- VARIABLES ----- //

    private void Update()
    {
        showVisualCues.device = InputManager.GetInstance().GetDevice();
        showVisualCues.ActivateCueForDevice(); // On affiche le visual cue en fonction du device
    }
}
