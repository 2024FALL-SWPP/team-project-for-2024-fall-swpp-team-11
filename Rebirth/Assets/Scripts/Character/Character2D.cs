using UnityEngine;

public class Character2D : MonoBehaviour
{
    private CharacterMovement2D characterMovement;
    private CharacterInteract2D characterInteract;
    private IInputHandler inputHandler;

    void Start()
    {
        characterMovement = GetComponent<CharacterMovement2D>();
        characterInteract = GetComponent<CharacterInteract2D>();
        inputHandler = GetComponent<IInputHandler>();
    }

    void Update()
    {
        HandleInteraction();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (inputHandler == null || characterMovement == null) return;
        if (GameStateManager.Instance.IsMovementLocked) return;

        Vector3 moveInput = inputHandler.GetMoveInput();
        characterMovement.Move(moveInput);
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
