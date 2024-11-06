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
        if (Input.GetKeyDown(KeyCode.F))
        {
            characterInteract.TryInteract(transform.position);
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDir = inputHandler.GetKeyDirection();
        characterMovement.Move(moveDir);
        characterAnimation.PlayMoveAnimation(moveDir);

        Vector3 mouseDir = inputHandler.GetMouseDirection();
        characterMovement.Turn(mouseDir);
    }
}
