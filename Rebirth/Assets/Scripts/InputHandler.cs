using UnityEngine;

public class InputHandler : MonoBehaviour, IInputHandler
{
    private float hSensitivity = 5f; 
    private float vSensitivity = 10f; 
    private float Yaxis;
    private float Xaxis;
    private float vRotMin = -20f;
    private float vRotMax = 80f;
    private float smoothTime = 0.12f;
    private Vector3 viewDir;
    private Vector3 moveDir;
    private Vector3 currentVel;

    void Update()
    {
        Yaxis += Input.GetAxis("Mouse X") * vSensitivity; 
        Xaxis += -Input.GetAxis("Mouse Y") * hSensitivity; 
        Xaxis = Mathf.Clamp(Xaxis, vRotMin, vRotMax); 
        viewDir = Vector3.SmoothDamp(viewDir, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveDir = new Vector3(horizontal, 0, vertical).normalized;
    }

    public Vector3 GetMoveDir()
    {
        return moveDir;
    }

    public Vector3 GetViewDir()
    {
        return viewDir;
    }
}
