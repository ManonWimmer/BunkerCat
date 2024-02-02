using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Mono.Cecil.Cil;
using UnityEngine.InputSystem.Utilities;

public class AutoScroll : MonoBehaviour
{
    [SerializeField]
    private float speed = 100f;

    [SerializeField]
    private float textPosBegin;

    [SerializeField]
    private float boundaryTextEnd;

    [SerializeField]
    private RectTransform txtRectTransform;

    [SerializeField]
    private PauseController pauseController;

    [SerializeField]
    private Loading loading;

    [SerializeField]
    private PanelFader panelFader;

    private bool isScrolling = false;

    private void Start()
    {
        pauseController.LockPauseWhenGameFinished();
        panelFader.Fade();
    }

    private void Update()
    {
        if (panelFader.isFaded && !isScrolling)
        {
            StartCoroutine(AutoScrollText());
        }

        bool interactPressed = InputManager.GetInstance().GetInteractPressed();
        if (interactPressed)
        {
            // Retour menu :
            loading.LoadScene();
        }
    }

    IEnumerator AutoScrollText()
    {
        isScrolling = true;
        while(txtRectTransform.localPosition.y < boundaryTextEnd) // Tant que le texte n'est pas à sa position en haut
        {
            txtRectTransform.Translate(Vector3.up * speed * Time.deltaTime); // On déplace le texte
            yield return null;  
        }
    }


}
