using UnityEngine;

public class InputHandler : MonoBehaviour, IInputHandler
{
    private float rotSensitive = 10f; 
    private float Yaxis;
    private float Xaxis;
    private float RotationMin = -10f;
    private float RotationMax = 80f;
    private float smoothTime = 0.12f;
    private Vector3 targetRotation;
    private Vector3 currentVel;

    void Update()
    {
        Yaxis = Yaxis + Input.GetAxis("Mouse X") * rotSensitive; 
        Xaxis = Xaxis - Input.GetAxis("Mouse Y") * rotSensitive; 

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax); 
    }

    public Vector3 GetKeyDirection()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        return new Vector3(horizontal, 0, vertical).normalized;
    }

    public Vector3 GetMouseDirection()
    {
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);

        return targetRotation;
    }
}
