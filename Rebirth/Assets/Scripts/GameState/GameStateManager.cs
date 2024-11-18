using UnityEngine;
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public bool IsViewLocked { get; private set; }
    public bool IsMovementLocked { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UnlockView();
        UnlockMovement();
        UnlockMovement();
    }
    public void LockView()
    {
        IsViewLocked = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void UnlockView()
    {
        IsViewLocked = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LockMovement()
    {
        IsMovementLocked = false;
    }

    public void UnlockMovement()
    {
        IsMovementLocked = true;
    }
}
