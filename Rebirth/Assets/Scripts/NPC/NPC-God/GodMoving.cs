using System.Collections;
using UnityEngine;

public class GodMoving : MonoBehaviour
{
    public float speed = 5f; 
    public float targetMaxY = 10f; 
    public float targetMinY = 10f;
    public float moveDistance = 10f; 

    private Coroutine moveCoroutine; 

    public void MovingUp()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveUpLimitedCoroutine());
    }

    public void MovingDown()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveDownLimitedCoroutine());
    }

    private IEnumerator MoveUpLimitedCoroutine()
    {
        float startY = transform.position.y; 

        while (transform.position.y < startY + moveDistance)
        {
            if (transform.position.y >= targetMaxY)
                break;

            transform.position += Vector3.up * speed * Time.deltaTime;
            yield return null; 
        }

        moveCoroutine = null;
    }

    private IEnumerator MoveDownLimitedCoroutine()
    {
        float startY = transform.position.y; 

        while (transform.position.y < startY + moveDistance)
        {
            if (transform.position.y <= targetMinY)
                break;

            transform.position -= Vector3.up * speed * Time.deltaTime;
            yield return null; 
        }

        moveCoroutine = null;
    }
}
