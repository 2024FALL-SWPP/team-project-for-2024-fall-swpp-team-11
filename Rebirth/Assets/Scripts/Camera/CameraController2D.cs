using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float minX, maxX;
    public float minY, maxY;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            // Boundaries for Hero
            float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}
