using UnityEngine;

public class InputHandler3D : MonoBehaviour, IInputHandler
{
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

    void Update()
    {
        UpdateInputState();
        UpdateMovement();
        UpdateViewRot();
    }

    private void UpdateInputState()
    {
        bool jump = currentState.jumpRequested || Input.GetKey(KeyCode.Space);
        bool interact = currentState.interactRequested || Input.GetKeyDown(KeyCode.F);
        currentState = new InputState(jump, interact);
    }

    private void UpdateMovement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");
    }

    private void UpdateViewRot()
    {
        if (GameStateManager.Instance.IsViewLocked) return;
        
        viewRot.y += Input.GetAxis("Mouse X") * verticalSensitivity; 
        viewRot.x += -Input.GetAxis("Mouse Y") * horizontalSensitivity; 
        viewRot.x = Mathf.Clamp(viewRot.x, minVerticalAngle, maxVerticalAngle); 
    }

    public Vector3 GetMoveInput()
    {
        return moveInput.normalized;
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
