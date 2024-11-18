using UnityEngine;

public class CharacterMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    // private Vector2 moveDir2D;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector3 moveDir)
    {
        if (!rb || moveDir == Vector3.zero) return;
        Vector2 moveDir2D = new Vector2(moveDir.x, moveDir.y);
        Vector2 moveDirection = transform.right * moveDir2D.x + transform.up * moveDir2D.y;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}
