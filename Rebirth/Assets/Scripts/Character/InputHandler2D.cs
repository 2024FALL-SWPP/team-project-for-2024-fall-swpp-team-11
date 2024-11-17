using UnityEngine;

public class InputHandler2D : MonoBehaviour, IInputHandler
{
    private Vector2 moveInput;
    private bool interactRequested;

    void Update()
    {
        UpdateInputState();
        UpdateMovement();
    }

    private void UpdateInputState()
    {
        interactRequested = Input.GetKeyDown(KeyCode.F);
    }

    private void UpdateMovement()
    {
        if (GameStateManager.Instance.IsMovementLocked)
        {
            moveInput = Vector2.zero;
            return;
        }
        
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
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
