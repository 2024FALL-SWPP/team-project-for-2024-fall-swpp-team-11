using UnityEngine;

public class Character3D : MonoBehaviour
{
    private CharacterAnimation3D characterAnimation;
    private CharacterMovement3D characterMovement;
    private CharacterInteract3D characterInteract;
    private IInputHandler inputHandler;

    void Start()
    {
        characterAnimation = GetComponent<CharacterAnimation3D>();
        characterMovement = GetComponent<CharacterMovement3D>();
        characterInteract = GetComponent<CharacterInteract3D>();
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
        if (GameStateManager.Instance.IsMovementLocked) return;

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
        if (GameStateManager.Instance.IsMovementLocked) return;
        if (!inputHandler.IsJumpRequested()) return;

        if (characterMovement.IsJumpable())
        {
            characterMovement.Jump();
            characterAnimation.PlayJumpAnimation();
        }
    }

    private void HandleInteraction()
    {
        if (inputHandler == null || characterInteract == null) return;
        if (GameStateManager.Instance.IsMovementLocked) return;

        if (inputHandler.IsInteractRequested())
        {
            characterInteract.TryInteract();
        }
    }
}
