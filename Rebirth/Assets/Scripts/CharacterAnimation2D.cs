using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation2D : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayMoveAnimation(Vector2 movement)
    {
        if (!animator) return;

        // speed
        animator.SetFloat("speed", movement.sqrMagnitude);

        // direction
        if (movement.sqrMagnitude == 0) return; // no movement. keep the last direction

        ResetAnimationParameters();

        if (movement.y > 0)
        {
            animator.SetBool("isBack", true);
        }
        else if (movement.y < 0)
        {
            animator.SetBool("isFront", true);
        }
        else if (movement.x < 0)
        {
            animator.SetBool("isLeft", true);
        }
        else if (movement.x > 0)
        {
            animator.SetBool("isRight", true);
        }
    }

    private void ResetAnimationParameters()
    {
        if (!animator) return;

        animator.SetBool("isBack", false);
        animator.SetBool("isFront", false);
        animator.SetBool("isLeft", false);
        animator.SetBool("isRight", false);
    }
}
