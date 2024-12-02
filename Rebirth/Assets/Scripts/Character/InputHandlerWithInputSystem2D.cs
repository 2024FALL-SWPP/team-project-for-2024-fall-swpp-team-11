using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandlerWithInputSystem2D : MonoBehaviour, IInputHandler
{
    private static readonly string logPrefix = "[InputHandlerWithInputSystem2D] ";

    private Vector2 moveInput;
    private bool interactRequested;

    public bool interactKeyTriggered = false;

    void Update()
    {
        UpdateInputState();
        // UpdateMovement();
    }

    private void UpdateInputState()
    {
        interactRequested = interactKeyTriggered;
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Debug.Log(logPrefix + "Movement input: " + input);
        moveInput.x = input.x;
        moveInput.y = input.y;
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
        return new Vector3(moveInput.x, moveInput.y, 0).normalized;
    }

    public Quaternion GetViewRot()
    {
        return Quaternion.identity;
    }

    public bool IsJumpRequested()
    {
        return false;
    }

    public bool IsInteractRequested()
    {
        return interactRequested;
    }
}
