using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    private IInputHandler inputHandler;
    private float vDist = 2f;
    private float hDist = 4f;

    void Start()
    {
        inputHandler = GetComponent<IInputHandler>();
    }

    void Update()
    {
        transform.rotation = inputHandler.GetViewRot();
        transform.position = target.position - transform.forward * hDist + transform.up * vDist;
    }
}
