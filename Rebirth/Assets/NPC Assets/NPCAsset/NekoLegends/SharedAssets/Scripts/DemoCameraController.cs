using System;
using UnityEngine;

namespace NekoLegends
{
    public class DemoCameraController : MonoBehaviour
    {
        [SerializeField] public Transform AutoDOFTarget;  // The target you want to keep in focus used for auto DOF

        public Camera mainCamera;
        public Transform target;
        public float rotationSpeed = 1.0f;
        public float zoomSpeed = .75f;
        public float panSpeed = 1.0f;
        public float zoomInMax = 0.25f;
        public float zoomOutMax = 1f;
        public bool invertAxisY = false;

        public bool useNewInputSystem = false;  // Manually set this based on your project setup

        private Vector3 cameraOffset;
        private Vector3 panLastPosition;
        private int previousTouchCount = 0;  // To track touch count changes

        // New variable for touch rotation speed multiplier
        public float touchRotationMultiplier = 0.2f;  // Adjust this value to control touch rotation sensitivity

        void Start()
        {
            if (mainCamera == null)
            {
                Debug.LogError("Camera not assigned!");
                return;
            }

            cameraOffset = mainCamera.transform.position - target.position;
        }

        void Update()
        {
            if (Input.touchCount > 0)
            {
                HandleTouchInput();
            }
            else
            {
                HandleMouseInput();
            }

            // Adjust DOF to keep AutoDOFTarget in focus
            if (AutoDOFTarget)
            {
                AdjustDOFForTarget(AutoDOFTarget);
            }

            // Update camera position and look at target
            mainCamera.transform.position = target.position + cameraOffset;
            mainCamera.transform.LookAt(target.position);
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                // Reset panLastPosition if touch count changed or touch began
                if (previousTouchCount != 1 || touch.phase == TouchPhase.Began)
                {
                    panLastPosition = mainCamera.ScreenToViewportPoint(touch.position);
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    Vector3 currentPosition = mainCamera.ScreenToViewportPoint(touch.position);
                    Vector3 deltaPosition = currentPosition - panLastPosition;

                    // Adjust panning direction based on camera's orientation
                    Vector3 horizontalDirection = Vector3.Cross(mainCamera.transform.forward, Vector3.up).normalized;

                    // Invert only the Y-component based on invertAxisY
                    float adjustedDeltaY = invertAxisY ? -deltaPosition.y : deltaPosition.y;
                    Vector3 moveDirection = deltaPosition.x * horizontalDirection + adjustedDeltaY * Vector3.up;
                    Vector3 move = moveDirection * panSpeed;

                    target.position += move;

                    panLastPosition = currentPosition;
                }
            }
            else if (Input.touchCount == 2)
            {
                // Two-finger touch for zooming and rotating
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Reset variables if touch count changed
                if (previousTouchCount != 2)
                {
                    panLastPosition = Vector3.zero;  // Reset pan position
                }

                // Store both touches.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

                // Zoom based on the delta magnitude difference
                float zoomAmount = deltaMagnitudeDiff * zoomSpeed * 0.01f; // Adjusted zoom direction

                float cameraDistance = cameraOffset.magnitude;
                cameraDistance -= zoomAmount;  // Subtract to correct zoom direction
                cameraDistance = Mathf.Clamp(cameraDistance, zoomInMax, zoomOutMax);
                cameraOffset = cameraOffset.normalized * cameraDistance;

                // Calculate rotation
                float prevAngle = AngleBetweenTouches(touchZeroPrevPos, touchOnePrevPos);
                float currentAngle = AngleBetweenTouches(touchZero.position, touchOne.position);
                float deltaAngle = Mathf.DeltaAngle(prevAngle, currentAngle);

                // Apply rotation around the target's up axis with adjusted speed
                float touchRotationSpeed = rotationSpeed * touchRotationMultiplier;
                Quaternion rotation = Quaternion.Euler(0, -deltaAngle * touchRotationSpeed, 0);
                cameraOffset = rotation * cameraOffset;
            }

            // Update previous touch count
            previousTouchCount = Input.touchCount;
        }

        private float AngleBetweenTouches(Vector2 touch1, Vector2 touch2)
        {
            return Mathf.Atan2(touch2.y - touch1.y, touch2.x - touch1.x) * Mathf.Rad2Deg;
        }

        private void HandleMouseInput()
        {
            int invertY = invertAxisY ? 1 : -1;

            Vector2 mouseDelta;
            float scrollData;

            if (useNewInputSystem)
            {
                // New Input System (Using Reflection)
                var mouseType = Type.GetType("UnityEngine.InputSystem.Mouse, Unity.InputSystem");
                if (mouseType != null)
                {
                    var deltaProperty = mouseType.GetProperty("delta");
                    var scrollProperty = mouseType.GetProperty("scroll");

                    var mouseInstance = mouseType.GetProperty("current").GetValue(null);
                    mouseDelta = (Vector2)deltaProperty.GetValue(mouseInstance);
                    scrollData = ((Vector2)scrollProperty.GetValue(mouseInstance)).y;
                }
                else
                {
                    Debug.LogError("New Input System not detected. Please ensure it's installed.");
                    return;
                }
            }
            else
            {
                // Old Input System
                mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                scrollData = Input.GetAxis("Mouse ScrollWheel");
            }

            // Handle rotation (right mouse button)
            if (IsRightMouseButtonPressed())
            {
                Quaternion camTurnAngle = Quaternion.Euler(mouseDelta.y * rotationSpeed, mouseDelta.x * rotationSpeed, 0);
                cameraOffset = camTurnAngle * cameraOffset;
            }

            // Handle zoom (mouse scroll wheel and middle mouse button drag)
            float zoomAmount = scrollData * zoomSpeed;
            if (IsMiddleMouseButtonPressed())
            {
                zoomAmount = -mouseDelta.y * zoomSpeed * 0.1f; // Multiplied by 0.1 to make it less sensitive than regular scroll
            }

            // Apply zoom
            float cameraDistance = cameraOffset.magnitude;
            cameraDistance += zoomAmount;
            cameraDistance = Mathf.Clamp(cameraDistance, zoomInMax, zoomOutMax);
            cameraOffset = cameraOffset.normalized * cameraDistance;

            // Handle pan (left mouse button)
            if (IsLeftMouseButtonPressed())
            {
                if (Input.GetMouseButtonDown(0)) // Check if left mouse button was just pressed
                {
                    panLastPosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
                }

                Vector3 currentPosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
                Vector3 deltaPosition = currentPosition - panLastPosition;

                // Adjust panning direction based on camera's orientation
                Vector3 horizontalDirection = Vector3.Cross(mainCamera.transform.forward, Vector3.up).normalized;

                Vector3 moveDirection = deltaPosition.x * horizontalDirection + -deltaPosition.y * Vector3.up;
                Vector3 move = moveDirection * panSpeed;

                target.position += move;

                panLastPosition = currentPosition;
            }
        }

        private void AdjustDOFForTarget(Transform autoDOFTarget)
        {
            float dynamicFocusDistance = Vector3.Distance(mainCamera.transform.position, autoDOFTarget.position);

            // Determine the current zoom level as a ratio between 0 (max zoom in) and 1 (max zoom out)
            float zoomRatio = (cameraOffset.magnitude - zoomInMax) / (zoomOutMax - zoomInMax);

            // Decide on min and max aperture values
            float minAperture = 1.4f; // max bokeh
            float maxAperture = 16.0f; // min bokeh

            // Interpolate based on the zoom ratio
            float currentAperture = Mathf.Lerp(minAperture, maxAperture, zoomRatio);

            DemoScenes.Instance.SetDOFImmediate(dynamicFocusDistance, currentAperture);
        }

        bool IsLeftMouseButtonPressed()
        {
            if (useNewInputSystem)
            {
                var mouseType = Type.GetType("UnityEngine.InputSystem.Mouse, Unity.InputSystem");
                if (mouseType != null)
                {
                    var leftButtonProperty = mouseType.GetProperty("leftButton");
                    var isPressedProperty = leftButtonProperty.PropertyType.GetProperty("isPressed");

                    var mouseInstance = mouseType.GetProperty("current").GetValue(null);
                    var leftButtonInstance = leftButtonProperty.GetValue(mouseInstance);
                    return (bool)isPressedProperty.GetValue(leftButtonInstance);
                }
                else
                {
                    Debug.LogError("New Input System not detected. Please ensure it's installed.");
                    return false;
                }
            }
            else
            {
                return Input.GetMouseButton(0);
            }
        }

        bool IsMiddleMouseButtonPressed()
        {
            if (useNewInputSystem)
            {
                // New Input System (Using Reflection)
                var mouseType = Type.GetType("UnityEngine.InputSystem.Mouse, Unity.InputSystem");
                if (mouseType != null)
                {
                    var middleButtonProperty = mouseType.GetProperty("middleButton");
                    var isPressedProperty = middleButtonProperty.PropertyType.GetProperty("isPressed");

                    var mouseInstance = mouseType.GetProperty("current").GetValue(null);
                    var middleButtonInstance = middleButtonProperty.GetValue(mouseInstance);
                    return (bool)isPressedProperty.GetValue(middleButtonInstance);
                }
                else
                {
                    Debug.LogError("New Input System not detected. Please ensure it's installed.");
                    return false;
                }
            }
            else
            {
                return Input.GetMouseButton(2);
            }
        }

        bool IsRightMouseButtonPressed()
        {
            if (useNewInputSystem)
            {
                var mouseType = Type.GetType("UnityEngine.InputSystem.Mouse, Unity.InputSystem");
                if (mouseType != null)
                {
                    var rightButtonProperty = mouseType.GetProperty("rightButton");
                    var isPressedProperty = rightButtonProperty.PropertyType.GetProperty("isPressed");

                    var mouseInstance = mouseType.GetProperty("current").GetValue(null);
                    var rightButtonInstance = rightButtonProperty.GetValue(mouseInstance);
                    return (bool)isPressedProperty.GetValue(rightButtonInstance);
                }
                else
                {
                    Debug.LogError("New Input System not detected. Please ensure it's installed.");
                    return false;
                }
            }
            else
            {
                return Input.GetMouseButton(1);
            }
        }
    }
}
