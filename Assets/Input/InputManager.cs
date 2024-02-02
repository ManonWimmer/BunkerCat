using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //


public class InputManager : MonoBehaviour
{
    // ----- VARIABLES ----- //
    private Vector2 moveDirection = Vector2.zero;
    private bool jumpPressed = false;
    private bool inventoryPressed = false;
    private bool interactPressed = false;
    private bool attackPressed = false;
    private bool submitPressed = false;
    private bool scanPressed = false; //
    private bool climbingPressed = false; //
    private bool sneakingPressed = false; //
    private bool dashingPressed = false; //
    private bool questsPressed = false; //
    private bool pausePressed = false; //
    private Vector2 uiNavigatePressed = Vector2.zero;
    private Vector2 uiPointDirection = Vector2.zero;

    [SerializeField]
    private PlayerInput playerInput;

    private string deviceUsed { get; set; }

    private static InputManager instance;
    // ----- VARIABLES ----- //

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Plus d'un Input Manager dans la scène");
        }

        instance = this;
    }

    public static InputManager GetInstance()
    {
        return instance;
    }

    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
    }

    public void JumpPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
        else if (context.canceled)
        {
            jumpPressed = false;
        }
    }

    
    public void InventoryButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inventoryPressed = true;
        }
        else if (context.canceled)
        {
            inventoryPressed = false;
        }
    }

    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }

    public void AttackPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attackPressed = true;
        }
        else if (context.canceled)
        {
            attackPressed = false;
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }

    public void ScanPressed(InputAction.CallbackContext context) // Player -> Player Input -> Events -> Game -> mettre Scan
    {
        if (context.performed)
        {
            //Debug.Log("scan pressed");
            scanPressed = true;
        }
        else if (context.canceled)
        {
            scanPressed = false;
        }
    }

    public void ClimbingPressed(InputAction.CallbackContext context) // Player -> Player Input -> Events -> Game -> mettre Scan
    {
        if (context.performed)
        {
            //Debug.Log("climbing pressed");
            climbingPressed = true;
        }

        else if (context.canceled)
        {
            //Debug.Log("climbing stop pressed");
            climbingPressed = false;
        }
    }

    public void SneakingPressed(InputAction.CallbackContext context) // Player -> Player Input -> Events -> Game -> mettre Scan
    {
        if (context.performed)
        {
            Debug.Log("sneaking pressed");
            sneakingPressed = true;
        }

        else if (context.canceled)
        {
            Debug.Log("sneaking stop pressed");
            sneakingPressed = false;
        }
    }

    public void DashingPressed(InputAction.CallbackContext context) // Player -> Player Input -> Events -> Game -> mettre Scan
    {
        if (context.performed)
        {
            dashingPressed = true;
        }

        else if (context.canceled)
        {
            dashingPressed = false;
        }
    }

    public void QuestsPressed(InputAction.CallbackContext context) // Player -> Player Input -> Events -> Game -> mettre Scan
    {
        if (context.performed)
        {
            questsPressed = true;
        }

        else if (context.canceled)
        {
            questsPressed = false;
        }
    }

    public void PausePressed(InputAction.CallbackContext context) // Player -> Player Input -> Events -> Game -> mettre Scan
    {
        if (context.performed)
        {
            pausePressed = true;
        }

        else if (context.canceled)
        {
            pausePressed = false;
        }
    }

    public void UINavigatePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            uiNavigatePressed = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            uiNavigatePressed = context.ReadValue<Vector2>();
        }
    }


    public void UIPointPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            uiPointDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            uiPointDirection = context.ReadValue<Vector2>();
        }
    }



    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    // Pour les Get d'après, si on les demande c'est qu'on les utilise donc ça devient false (sauf ceux pour l'animator)

    public bool GetJumpPressed()
    {
        bool result = jumpPressed;
        jumpPressed = false;
        return result;
    }

    public bool GetInventoryPressed()
    {
        bool result = inventoryPressed;
        inventoryPressed = false;
        return result;
    }

    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public bool GetAttackPressedAnim() // Sans le mettre à false
    {
        bool result = attackPressed;
        return result;
    }

    public bool GetAttackPressed()
    {
        bool result = attackPressed;
        attackPressed = false;
        return result;
    }

    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    public bool GetScanPressed() //
    {
        bool result = scanPressed;
        scanPressed = false;
        return result;
    }

    public bool GetClimbingPressed() //
    {
        return climbingPressed;
    }
    

    public bool GetSneakingPressed() //
    {
        return sneakingPressed;
    }

    public bool GetDashingPressed() //
    {
        return dashingPressed;
    }

    public bool GetQuestsPressed() //
    {
        bool result = questsPressed;
        questsPressed = false;
        return result;
    }

    public bool GetPausePressed() //
    {
        bool result = pausePressed;
        pausePressed = false;
        return result;
    }
    public string GetDevice()
    {
        return deviceUsed;
    }

    public Vector2 GetUINavigatePressed()
    {
        return uiNavigatePressed;
    }

    public Vector2 GetUIPointPressed()
    {
        return uiPointDirection;
    }

    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }

    private void UpdateDevice()
    {
        deviceUsed = playerInput.currentControlScheme;
    }

    public void OnControlsChanged()
    {
        Debug.Log("controls changed");
        UpdateDevice();
    }

    

}
