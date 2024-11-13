using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterAnimation characterAnimation;
    private CharacterMovement characterMovement;
    private CharacterInteract characterInteract;
    private IInputHandler inputHandler;
    
    void Start()
    {
        characterAnimation = GetComponent<CharacterAnimation>();
        characterMovement = GetComponent<CharacterMovement>();
        characterInteract = GetComponent<CharacterInteract>();
        inputHandler = GetComponent<IInputHandler>();
    }

    void Update()
    {
        HandleRotation();
        HandleInteraction();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        if (inputHandler == null || !characterMovement || !characterAnimation) return;

        Vector3 moveInput = inputHandler.GetMoveInput();
        characterMovement.Move(moveInput);
        characterAnimation.PlayMoveAnimation(moveInput);
    }

    void HandleRotation()
    {
        if (inputHandler == null || !characterMovement) return;
        if (!GameStateManager.Instance.IsViewLocked) return;

        Quaternion viewRot = inputHandler.GetViewRot();
        characterMovement.Turn(viewRot);
    }

    void HandleJump()
    {
        if (!characterMovement || !characterAnimation) return;
        if (!inputHandler.IsJumpRequested()) return;

        if (characterMovement.IsJumpable())
        {
            characterMovement.Jump();
            characterAnimation.PlayJumpAnimation();
        }
    }

    private void HandleInteraction()
    {
        if (!inputHandler.IsInteractRequested()) return;
        
        characterInteract.TryInteract();
    }
}
