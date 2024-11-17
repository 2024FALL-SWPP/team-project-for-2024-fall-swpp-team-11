using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;

    private Rigidbody2D rb;

    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); 
        movement.y = Input.GetAxisRaw("Vertical");  
        animator.SetFloat("speed", movement.sqrMagnitude);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("isBack", true);
            animator.SetBool("isFront", false);
            animator.SetBool("isLeft", false);
            animator.SetBool("isRight", false);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("isFront", true);
            animator.SetBool("isBack", false); 
            animator.SetBool("isLeft", false);
            animator.SetBool("isRight", false);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("isLeft", true);
            animator.SetBool("isBack", false); 
            animator.SetBool("isFront", false);
            animator.SetBool("isRight", false);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("isRight", true);
            animator.SetBool("isBack", false); 
            animator.SetBool("isFront", false);
            animator.SetBool("isLeft", false);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
