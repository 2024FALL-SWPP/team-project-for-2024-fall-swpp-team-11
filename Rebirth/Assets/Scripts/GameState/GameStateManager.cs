using UnityEngine;
using UnityEngine.InputSystem;
public class GameStateManager : SingletonManager<GameStateManager>
{
    public bool IsViewLocked { get; private set; }
    public bool IsMovementLocked { get; private set; }
    private Vector2 savedMousePosition;

    void Start()
    {
        LockView();
        LockMovement();
    }

    public void LockView()
    {
        IsViewLocked = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Vector2 screenPosition = new Vector2(
            savedMousePosition.x * Screen.width,
            savedMousePosition.y * Screen.height
        );
        Mouse.current.WarpCursorPosition(screenPosition);
    }

    public void UnlockView()
    {
        IsViewLocked = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Vector2 currentPos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        savedMousePosition = currentPos;
    }

    public void LockMovement()
    {
        IsMovementLocked = true;
    }

    public void UnlockMovement()
    {
        IsMovementLocked = false;
    }
}
