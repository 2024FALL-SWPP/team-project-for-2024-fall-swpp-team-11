using UnityEngine;

public class SortingInOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public float yOffset = 0.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.sortingOrder = -(int)((transform.position.y - yOffset) * 100);
    }
}
