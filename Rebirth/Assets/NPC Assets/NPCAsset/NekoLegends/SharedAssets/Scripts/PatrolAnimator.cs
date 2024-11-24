using System.Collections;
using UnityEngine;

namespace NekoLegends
{
    public class PatrolAnimator : MonoBehaviour
    {
        public Vector3[] Positions;  // Array of positions for animation
        public EasingType EasingTypes;  // Enum for different easing types
        public float RotationSpeed = 180.0f;  // Rotation speed in degrees per second
        public float PatrolSpeed = 1f;
        private int currentTargetIndex = 0;  // Index of the current target position
        private Quaternion initialRotation;  // Initial rotation of the character

        private void Start()
        {
            currentTargetIndex = 0;  // Start with the first position
            initialRotation = transform.rotation;
        }

        private void Update()
        {
            if (currentTargetIndex < Positions.Length)
            {
                Vector3 targetDirection = Positions[currentTargetIndex] - transform.position;
                if (targetDirection.magnitude > 0.1f) // Check if the direction vector is not close to zero
                {
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
                }
            }
        }


        private void OnEnable()
        {
            StartCoroutine(AnimatePositions());
        }

        private void OnDisable()
        {
            StopCoroutine(AnimatePositions());
        }

        private IEnumerator AnimatePositions()
        {
            while (true)
            {
                Vector3 currentTargetPosition = Positions[currentTargetIndex];
                float distanceToTarget = Vector3.Distance(transform.localPosition, currentTargetPosition);

                // Move the character towards the current target position
                while (distanceToTarget > 0.1f)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentTargetPosition, PatrolSpeed * Time.deltaTime);
                    yield return null;
                    distanceToTarget = Vector3.Distance(transform.localPosition, currentTargetPosition);
                }
                // Pause for a brief moment at each corner
                //yield return new WaitForSeconds(1.0f); // Adjust the duration as needed

                // Switch to the next target position in a loop
                currentTargetIndex = (currentTargetIndex + 1) % Positions.Length;
                 
                yield return null;
            }
        }

        // Enum for different easing types
        public enum EasingType
        {
            Linear,
            EaseIn,
            EaseOut,
            EaseInOut
        }

        private float EaseIn(float t)
        {
            return t * t;
        }

        private float EaseOut(float t)
        {
            return 1f - Mathf.Pow(1f - t, 2f);
        }

        private float EaseInOut(float t)
        {
            if (t < 0.5f)
            {
                return 2f * t * t;
            }
            else
            {
                return 1f - 2f * Mathf.Pow(1f - t, 2f);
            }
        }
    }
}
