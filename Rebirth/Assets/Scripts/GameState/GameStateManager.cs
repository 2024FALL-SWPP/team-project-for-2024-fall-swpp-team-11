using UnityEngine;
using UnityEngine.InputSystem;
public class GameStateManager : SingletonManager<GameStateManager>
{
    public bool IsViewLocked { get; private set; }
    public bool IsMovementLocked { get; private set; }
    private Vector2 savedMousePosition;

    protected override void Awake()
    {
        base.Awake();
        
        UnlockView();
        UnlockMovement();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Respawn();
        }
    }

    public Vector3 GetAnchorPosition()
    {
        GameObject player;
        DimensionManager.Instance.FindCurrentPlayer(out player);
        Anchor anchor;
        DimensionManager.Instance.FindCurrentAnchor(player, out anchor);
        return anchor.transform.position;
    }

    public void Respawn()
    {
        SceneTransitionManager.Instance.SetPlayerPosition(GetAnchorPosition());
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
