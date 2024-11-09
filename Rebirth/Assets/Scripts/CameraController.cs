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

    void LateUpdate()
    {
        this.transform.eulerAngles = inputHandler.GetViewDir();
        transform.position = target.position - transform.forward * hDist + transform.up * vDist;
    }
}
