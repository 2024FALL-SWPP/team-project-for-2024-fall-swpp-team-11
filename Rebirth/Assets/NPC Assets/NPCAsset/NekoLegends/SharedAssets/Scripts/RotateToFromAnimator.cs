using System.Collections;
using UnityEngine;

namespace NekoLegends
{
    public class RotateToFromAnimator : MonoBehaviour
    {
        public bool isEnabled = false; // Toggle the rotation animation
        public float startDelay = 0f; // Delay before the rotation starts
        public float duration = 1f; // Duration of the rotation
        public Vector3 fromRotation = Vector3.zero; // Starting rotation in Euler angles
        public Vector3 toRotation = Vector3.zero; // Ending rotation in Euler angles
        public bool useEasing = true; // Use easing for smoother rotation
        public bool rotateClockwise = true; // Direction of rotation
        public bool setFromRotationImmediately = true; // Set fromRotation immediately even with a delay

        private float timeElapsed; // Time elapsed since the start of the rotation
        private bool isStarted = false; // Tracks if the delay has passed and rotation has started

        void Update()
        {
            if (isEnabled)
            {
                if (!isStarted)
                {
                    // Set the fromRotation immediately if the option is enabled, even with a delay
                    if (setFromRotationImmediately && timeElapsed == 0)
                    {
                        transform.rotation = Quaternion.Euler(fromRotation);
                    }

                    if (timeElapsed >= startDelay)
                    {
                        isStarted = true; // Start the rotation after the delay
                        timeElapsed = 0; // Reset the timer to start the rotation duration
                    }
                    else
                    {
                        timeElapsed += Time.deltaTime; // Increment the timer during the delay
                        return;
                    }
                }

                if (timeElapsed < duration)
                {
                    float fraction = timeElapsed / duration;

                    // Optionally use easing for the interpolation
                    if (useEasing)
                    {
                        fraction = Mathf.SmoothStep(0.0f, 1.0f, fraction);
                    }

                    // Calculate the current rotation based on the fraction
                    Vector3 currentRotation = Vector3.Lerp(
                        rotateClockwise ? fromRotation : toRotation,
                        rotateClockwise ? toRotation : fromRotation,
                        fraction
                    );
                    transform.rotation = Quaternion.Euler(currentRotation);

                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    // Ensure the final rotation is set exactly and disable the component
                    transform.rotation = Quaternion.Euler(toRotation);
                    isEnabled = false;
                }
            }
        }

        // Call this method to start the rotation
        public void StartRotation()
        {
            timeElapsed = 0; // Reset the timer
            isStarted = false; // Reset the start flag
            isEnabled = true; // Enable the rotation
        }
    }
}
