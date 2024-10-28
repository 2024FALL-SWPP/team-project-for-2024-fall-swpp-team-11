using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; 
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");  
        float moveVertical = Input.GetAxis("Vertical");     

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized * moveSpeed;

        rb.MovePosition(transform.position + movement * Time.deltaTime);
    }
}


