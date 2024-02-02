using System;
# if UNITY_EDITOR
using UnityEditor.Hardware;
using UnityEditorInternal;
# endif
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamepadCursor : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField]
    private RectTransform cursorTransform;

    public GameObject cursor;

    [SerializeField]
    private RectTransform canvasRectTransform;

    public GameObject player;

    [SerializeField]
    private float cursorSpeed = 1000;


    private Mouse virtualMouse;

    private bool previousMouseState;

    public static GamepadCursor instance;

    [SerializeField]
    private PlayerInput playerInput;


    // ----- VARIABLES ----- //
    private void Awake()
    {
        instance = this;
        cursor.SetActive(false);
        gameObject.SetActive(false);
    }

    public static GamepadCursor GetInstance()
    {
        return instance;
    }

    private void OnEnable()
    {
        Debug.Log("enable virtual mouse");

        cursor.SetActive(true);

        Debug.Log(virtualMouse?.allControls);
        if (virtualMouse == null)
        {
            virtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        // Pair the device to the user to use the PlayerInput component with the Event System & the Virtual Mouse
        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        //UnityEngine.Cursor.visible = false;
        InputSystem.onAfterUpdate += UpdateMotion;
    }

    private void OnDisable()
    {
        Debug.Log("on disable cursor");
        cursor?.SetActive(false);
        InputSystem.onAfterUpdate -= UpdateMotion;
        if (virtualMouse != null && virtualMouse.added)
        {
            InputSystem.RemoveDevice(virtualMouse);
        }
    }

    private void UpdateMotion()
    {
        //if (virtualMouse == null || Gamepad.current == null)
        if (virtualMouse == null || InputManager.GetInstance().GetDevice() == "Keyboard")
        {
            Debug.Log("return");
            return;
        }

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        //Vector2 deltaValue = InputManager.GetInstance().GetMoveDirection();
        deltaValue *= cursorSpeed * Time.unscaledDeltaTime;
        

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        //Debug.Log(virtualMouse.position.ReadValue());
        Vector2 newPosition = currentPosition + deltaValue;

        // On clamp pour pas que le cursor puisse sortir de l'écran
        newPosition.x = Mathf.Clamp(newPosition.x, 0f, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0f, Screen.height);


        // On change la position de la souris
        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        // Pour le click :
        //bool aButtonPressed = Gamepad.current.aButton.IsPressed;
        bool aButtonPressed = InputManager.GetInstance().GetAttackPressed();
        //Debug.Log(aButtonPressed);

        if (previousMouseState != aButtonPressed && InputManager.GetInstance().GetDevice() != "Keyboard")
        {
            Debug.Log("ici");
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonPressed);
            InputState.Change(virtualMouse, mouseState);
            Debug.Log(mouseState.ToString());
            previousMouseState = aButtonPressed;
        }
        AnchorCursor(newPosition);
    }

    // Changement de l'emplacement du cursor sur l'écran
    private void AnchorCursor(Vector2 position)
    {
        // Screen coordinate position -> RectTransform coordinate
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, null, out anchoredPosition);
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, mainCamera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
        //Debug.Log(anchoredPosition);
    }
}
