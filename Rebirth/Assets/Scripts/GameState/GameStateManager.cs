using UnityEngine;
public class GameStateManager : SingletonManager<GameStateManager>
{
    public bool IsViewLocked { get; private set; }
    public bool IsMovementLocked { get; private set; }

    void Start()
    {
        UnlockView();
        UnlockMovement();
        UnlockMovement();
    }
    public void LockView()
    {
        IsViewLocked = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void UnlockView()
    {
        IsViewLocked = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
