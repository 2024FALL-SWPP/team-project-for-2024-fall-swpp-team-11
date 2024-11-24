using System.Collections;
using UnityEngine;

namespace NekoLegends
{
    public class PositionAnimator : MonoBehaviour
    {
        private bool toggleStopResume = true;
        public Vector3 StartPosition = new Vector3(0f, 0f, 0f);
        public Vector3 EndPosition = new Vector3(1f, 0f, 0f);
        public float Duration = 2f;

        public float startDelay = 0f; // Delay before the position animation starts
        public bool UseLocalPosition = false;
        public bool Loop = true;
        public bool BounceBack = true;  // Checkbox for returning from B to A
        public bool setStartPositionImmediately = true; // Set StartPosition immediately even with a delay
        public bool useEasing = true; // Enable easing for smoother transitions

        private Coroutine animationCoroutine;
        private bool isAnimating;
        private Vector3 resumeStartPosition;

        private void OnEnable()
        {
            if (toggleStopResume)
            {
                StartAnimation();
            }
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        private void StartAnimation()
        {
            if (isAnimating)
            {
                return;
            }

            StopAnimation();  // Ensure any existing animation is stopped before starting a new one
            animationCoroutine = StartCoroutine(AnimatePosition());
            isAnimating = true;
        }

        private void StopAnimation()
        {
            if (animationCoroutine != null)
            {
                // Update the resumeStartPosition before stopping the animation
                resumeStartPosition = transform.position;
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
                isAnimating = false;
            }
        }

        private IEnumerator AnimatePosition()
        {
            // Set the start position immediately if the option is enabled
            if (setStartPositionImmediately)
            {
                if (UseLocalPosition)
                {
                    transform.localPosition = StartPosition;
                }
                else
                {
                    transform.position = StartPosition;
                }
            }

            // Start delay before the actual animation
            if (startDelay > 0)
            {
                yield return new WaitForSeconds(startDelay);
            }

            do
            {
                yield return AnimateFromTo(StartPosition, EndPosition, Duration);
                if (BounceBack)
                {
                    yield return AnimateFromTo(EndPosition, StartPosition, Duration);
                }

                if (!toggleStopResume)
                {
                    yield break;
                }
            } while (Loop);
        }

        private IEnumerator AnimateFromTo(Vector3 startPos, Vector3 endPos, float duration)
        {
            float elapsed = 0f;
            Vector3 initialPos = UseLocalPosition ? transform.localPosition : transform.position;

            while (elapsed < duration)
            {
                float t = elapsed / duration;

                // Apply easing if enabled
                if (useEasing)
                {
                    t = Mathf.SmoothStep(0.0f, 1.0f, t);
                }

                Vector3 newPos = Vector3.Lerp(startPos, endPos, t);

                if (UseLocalPosition)
                {
                    transform.localPosition = newPos;
                }
                else
                {
                    transform.position = newPos;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure the position is set to the endPos when the animation is done
            if (UseLocalPosition)
            {
                transform.localPosition = endPos;
            }
            else
            {
                transform.position = endPos;
            }
        }

        public void ToggleAnimation()
        {
            toggleStopResume = !toggleStopResume;

            if (toggleStopResume)
            {
                // Use resumeStartPosition as the new StartPosition if resuming the animation
                if (isAnimating)
                {
                    StartPosition = resumeStartPosition;
                }
                StartAnimation();
            }
            else
            {
                StopAnimation();
            }
        }
    }
}
