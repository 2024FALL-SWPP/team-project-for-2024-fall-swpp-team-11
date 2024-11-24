using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement2D : MonoBehaviour
{
    public float speed = 2f; 
    public float moveDistance = 3f;
    public bool isVertical = true; 

    private Vector3 startPosition; 
    private int direction = 1; 

    private Rigidbody2D rb; 

    void Start()
    {
        startPosition = transform.position; 
        rb = GetComponent<Rigidbody2D>(); 

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D가 필요합니다!");
        }

        rb.gravityScale = 0;
    }

    void FixedUpdate()
    {

        float traveledDistance = isVertical
            ? Mathf.Abs(transform.position.y - startPosition.y)
            : Mathf.Abs(transform.position.x - startPosition.x);

        if (traveledDistance >= moveDistance)
        {
            direction *= -1; 
            Vector3 clampedPosition = isVertical
                ? new Vector3(transform.position.x, startPosition.y + moveDistance * direction, transform.position.z)
                : new Vector3(startPosition.x + moveDistance * direction, transform.position.y, transform.position.z);
            transform.position = clampedPosition;
        }

        rb.velocity = isVertical
            ? new Vector2(0, speed * direction) 
            : new Vector2(speed * direction, 0); 
    }

}

