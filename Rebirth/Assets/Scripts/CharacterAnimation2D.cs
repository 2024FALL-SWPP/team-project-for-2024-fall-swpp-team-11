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

        animator.SetBool("isBack", false);
        animator.SetBool("isFront", false);
        animator.SetBool("isLeft", false);
        animator.SetBool("isRight", false);

        animator.SetFloat("speed", movement.sqrMagnitude);

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

}
