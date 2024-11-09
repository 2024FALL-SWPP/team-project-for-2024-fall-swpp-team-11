using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterAnimation characterAnimation;
    private CharacterMovement characterMovement;
    private CharacterInteract characterInteract;
    private IInputHandler inputHandler;
    private bool spacePressed = false;
    
    void Start()
    {
        characterAnimation = GetComponent<CharacterAnimation>();
        characterMovement = GetComponent<CharacterMovement>();
        characterInteract = GetComponent<CharacterInteract>();
        inputHandler = GetComponent<IInputHandler>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacePressed = true;
        }
    }

    void FixedUpdate()
    {
        Move();
        Turn();
        if (spacePressed)
        {
            Jump();
            spacePressed = false;
        }
    }

    void Move()
    {
        if (inputHandler == null || !characterMovement || !characterAnimation) return;

        Vector3 moveDir = inputHandler.GetMoveDir();
        characterMovement.Move(moveDir);
        characterAnimation.PlayMoveAnimation(moveDir);
    }

    void Turn()
    {
        if (inputHandler == null || !characterMovement) return;

        Vector3 viewDir = inputHandler.GetViewDir();
        characterMovement.Turn(viewDir);
    }

    void Jump()
    {
        if (!characterMovement || !characterAnimation) return;

        if (characterMovement.IsJumpable())
        {
            characterMovement.Jump();
            characterAnimation.PlayJumpAnimation();
        }
    }

    void Interact()
    {
        if (!characterInteract) return;

        characterInteract.TryInteract(transform.position);
    }
}
