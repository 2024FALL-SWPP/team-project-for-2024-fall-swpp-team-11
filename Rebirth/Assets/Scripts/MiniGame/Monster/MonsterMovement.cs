using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 3f; 

    private Vector3 startPosition; 
    private int direction = 1; 

    void Start()
    {
        startPosition = transform.position; 
    }

    void Update()
    {

        float moveZ = speed * direction * Time.deltaTime;


        transform.Translate(0, 0, moveZ);


        if (Vector3.Distance(startPosition, transform.position) > moveDistance)
        {
            direction *= -1; 
        }
    }
}
