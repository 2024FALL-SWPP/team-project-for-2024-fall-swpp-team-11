using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    private IInputHandler inputHandler;
    private float dist = 2f;

    void Start()
    {
        inputHandler = GetComponent<IInputHandler>();
    }

    void LateUpdate()
    {
        this.transform.eulerAngles = inputHandler.GetMouseDirection();
        transform.position = target.position - transform.forward * dist + transform.up * dist;
    }
}
