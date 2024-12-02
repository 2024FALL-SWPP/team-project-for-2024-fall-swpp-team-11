using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandlerWithInputSystem3D : MonoBehaviour, IInputHandler
{
    private static readonly string logPrefix = "[InputHandlerWithInputSystem3D] ";

    [Header("Mouse Sensitivity")]
    [SerializeField] private float horizontalSensitivity = 5f;
    [SerializeField] private float verticalSensitivity = 10f;

    [Header("Vertical Rotation Limits")]
    [SerializeField] private float minVerticalAngle = -20f;
    [SerializeField] private float maxVerticalAngle = 80f;

    private Vector2 viewRot;
    private Vector3 moveInput;
    private InputState currentState;

    private readonly struct InputState
    {
        public readonly bool jumpRequested;
        public readonly bool interactRequested;

        public InputState(bool jump, bool interact)
        {
            jumpRequested = jump;
            interactRequested = interact;
        }
    }

    public bool jumpKeyTriggered = false;
    public bool interactKeyTriggered = false;


    public void Update()
    {
        UpdateInputState();
        // UpdateMovement();
        // UpdateViewRot();
    }

    private void UpdateInputState()
    {
        bool jump = currentState.jumpRequested || jumpKeyTriggered;
        bool interact = currentState.interactRequested || interactKeyTriggered;
        currentState = new InputState(jump, interact);
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Debug.Log(logPrefix + "Movement input: " + input);
        moveInput.x = input.x;
        moveInput.z = input.y;
    }

    public void OnRotateInput(InputAction.CallbackContext context)
    {
        // TODO context phase
        if (GameStateManager.Instance.IsViewLocked) return;
        Vector2 input = context.ReadValue<Vector2>();
        // Debug.Log(logPrefix + "Phase: " + context.phase + " Rotate input: " + input);
        viewRot.y += input.x * verticalSensitivity;
        viewRot.x += -input.y * horizontalSensitivity;
        viewRot.x = Mathf.Clamp(viewRot.x, minVerticalAngle, maxVerticalAngle);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(logPrefix + "Jump key triggered");
            jumpKeyTriggered = true;
        }

        if (context.canceled)
        {
            Debug.Log(logPrefix + "Jump key released");
            jumpKeyTriggered = false;
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactKeyTriggered = true;
        }

        if (context.canceled)
        {
            interactKeyTriggered = false;
        }
    }


    public Vector3 GetMoveInput()
    {
        return moveInput.normalized;
    }

    public Vector2 GetMoveInput2D()
    {
        return new Vector2(moveInput.x, moveInput.z).normalized;
    }

    public Quaternion GetViewRot()
    {
        return Quaternion.Euler(viewRot.x, viewRot.y, 0f);
    }

    public bool IsJumpRequested()
    {
        bool wasRequested = currentState.jumpRequested;
        currentState = new InputState(false, currentState.interactRequested);
        return wasRequested;
    }

    public bool IsInteractRequested()
    {
        bool wasRequested = currentState.interactRequested;
        currentState = new InputState(currentState.jumpRequested, false);
        return wasRequested;
    }
}