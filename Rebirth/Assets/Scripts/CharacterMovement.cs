using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float turnSpeed = 20f;
    private float jumpForce = 5f;
    private Rigidbody rb;
    private bool canJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb && rb.velocity.y <= 0 && IsGrounded())
        {
            canJump = true;
        }
    }

    public void Move(Vector3 moveDir)
    {
        if (!rb || moveDir == Vector3.zero) return;

        Vector3 moveDirection = transform.right * moveDir.x + transform.forward * moveDir.z;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    public void Turn(Vector3 viewDir)
    {
        Quaternion targetRotation = Quaternion.Euler(0, viewDir.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
    }

    public void Jump()
    {
        if (!rb) return;

        if (IsGrounded() && canJump)
        {
            canJump = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        float rayDist = 0.2f;

        return Physics.Raycast(rayOrigin, -Vector3.up, rayDist);
    }

    public bool IsJumpable()
    {
        return IsGrounded() && canJump;
    }
}