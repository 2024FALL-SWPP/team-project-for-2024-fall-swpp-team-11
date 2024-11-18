using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2D : MonoBehaviour
{
    private CharacterAnimation2D characterAnimation;
    private CharacterMovement2D characterMovement;
    private InputHandler inputHandler;

    void Start()
    {
        characterAnimation = GetComponent<CharacterAnimation2D>();
        characterMovement = GetComponent<CharacterMovement2D>();
        inputHandler = GetComponent<InputHandler>();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (inputHandler == null || !characterMovement || !characterAnimation) return;

        Vector2 moveInput = inputHandler.GetMoveInput2D();
        characterMovement.Move(moveInput);
        characterAnimation.PlayMoveAnimation(moveInput);
    }
}
