using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStateManager : SingletonManager<GameStateManager>
{
    private static string logPrefix = "[GameStateManager] ";

    public bool IsViewLocked { get; private set; }
    public bool IsMovementLocked { get; private set; }
    private Vector2 savedMousePosition;

    protected override void Awake()
    {
        base.Awake();

        // Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.Full);
        // Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.Full);
        // Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
        // Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);
        // Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
            
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            UnlockView();
            UnlockMovement();
        }
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

        // Debug.Log(logPrefix + "LockView");
    }

    public void UnlockView()
    {
        IsViewLocked = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Vector2 currentPos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        savedMousePosition = currentPos;

        // Debug.Log(logPrefix + "UnlockView");
    }

    public void LockMovement()
    {
        IsMovementLocked = true;

        // Debug.Log(logPrefix + "LockMovement");
    }

    public void UnlockMovement()
    {
        IsMovementLocked = false;

        // Debug.Log(logPrefix + "UnlockMovement");
    }
}
