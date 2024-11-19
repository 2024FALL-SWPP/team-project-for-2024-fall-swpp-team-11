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

    public void PlayMoveAnimation(Vector3 movement)
    {
        if (!animator) return;

        Vector2 movement2D = new Vector2(movement.x, movement.y);

        // speed
        animator.SetFloat("speed", movement2D.sqrMagnitude);

        // direction
        if (movement2D.sqrMagnitude == 0) return; // no movement. keep the last direction

        ResetAnimationParameters();

        if (movement2D.y > 0)
        {
            animator.SetBool("isBack", true);
        }
        else if (movement2D.y < 0)
        {
            animator.SetBool("isFront", true);
        }
        else if (movement2D.x < 0)
        {
            animator.SetBool("isLeft", true);
        }
        else if (movement2D.x > 0)
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
