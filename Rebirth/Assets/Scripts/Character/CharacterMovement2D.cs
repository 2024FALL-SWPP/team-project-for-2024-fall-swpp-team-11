using UnityEngine;

public class CharacterMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector3 moveInput)
    {
        movement = new Vector2(moveInput.x, moveInput.y);
    }

    void FixedUpdate()
    {
        if (GameStateManager.Instance.IsMovementLocked)
        {
            movement = Vector2.zero;
            return;
        }
        
        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
