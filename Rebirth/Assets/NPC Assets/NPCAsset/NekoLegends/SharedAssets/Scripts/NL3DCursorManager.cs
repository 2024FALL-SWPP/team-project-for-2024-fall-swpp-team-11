using UnityEngine;
using System.Collections;

namespace NekoLegends
{

    public class NL3DCursorManager : MonoBehaviour
    {
        [Header("Cursor Settings")]
        [Tooltip("The custom cursor prefab to use.")]
        public GameObject cursorPrefab;

        [Tooltip("Distance from the camera to place the cursor.")]
        public float cursorDistance = 1f;

        [Tooltip("Cursor follow speed. Higher values make the cursor follow the mouse faster.")]
        public float cursorSpeed = 50f;

        [Tooltip("Enable billboarding to make the cursor always face the camera.")]
        public bool enableBillboarding = true;

        public bool hideNativeCursor = true;
        private float delayTimeInSeconds = .5f;

        // Reference to the instantiated cursor
        private GameObject instantiatedCursor;

        void Start()
        {
            if (cursorPrefab != null)
            {
                // Instantiate the cursor prefab at the initial cursor position
                instantiatedCursor = Instantiate(cursorPrefab, GetCursorWorldPosition(), Quaternion.identity, this.transform);

                // Optionally, set the cursor as a child of this manager for better organization
                instantiatedCursor.transform.SetParent(this.transform, false);
                // Hide the default system cursor
                StartCoroutine(SetCursorVisibilityAfterDelay(delayTimeInSeconds)); //override other code states
            }
        }

        private IEnumerator SetCursorVisibilityAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Cursor.visible = !hideNativeCursor;
        }

        void Update()
        {
            if (instantiatedCursor != null)
            {
                // Get the current mouse position
                Vector3 mousePos = Input.mousePosition;

                // Convert the mouse position to world space
                Vector3 cursorWorldPos = GetCursorWorldPosition();

                // Smoothly move the cursor towards the new position
                instantiatedCursor.transform.position = Vector3.Lerp(instantiatedCursor.transform.position, cursorWorldPos, Time.deltaTime * cursorSpeed);

                // Optional: Make the cursor face the camera (billboarding)
                if (enableBillboarding)
                {
                    Camera mainCam = Camera.main;
                    if (mainCam != null)
                    {
                        instantiatedCursor.transform.rotation = Quaternion.LookRotation(instantiatedCursor.transform.position - mainCam.transform.position);
                    }
                }
            }
        }

        /// <summary>
        /// Converts the current mouse position to a world position based on the camera.
        /// </summary>
        /// <returns>World position where the cursor should be placed.</returns>
        Vector3 GetCursorWorldPosition()
        {
            Camera mainCam = Camera.main;

            if (mainCam != null)
            {
                // Create a ray from the camera to the mouse position
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

                // Calculate the point at cursorDistance units from the camera
                Vector3 worldPos = ray.origin + ray.direction * cursorDistance;

                return worldPos;
            }
            else
            {
                return Vector3.zero;
            }
        }

        private void OnDisable()
        {
            // Show the default system cursor when this script is disabled
            Cursor.visible = true;

            // Destroy the instantiated cursor
            if (instantiatedCursor != null)
            {
                Destroy(instantiatedCursor);
            }
        }
        /*
        // Optional: Handle mouse click events to trigger cursor animations or effects
        private void OnGUI()
        {
            // Detect mouse clicks
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                // Trigger a method or animation in your cursor prefab
                CustomCursorBehavior cursorBehavior = instantiatedCursor.GetComponent<CustomCursorBehavior>();
                if (cursorBehavior != null)
                {
                    cursorBehavior.OnClick();
                }
            }
        }
        
         using UnityEngine;

        public class CustomCursorBehavior : MonoBehaviour
        {
            [Header("Particle Effects")]
            [Tooltip("Particle system to play on click.")]
            public ParticleSystem clickParticles;

            [Header("Animator")]
            [Tooltip("Animator component for cursor animations.")]
            public Animator cursorAnimator;

            /// <summary>
            /// Called by the Cursor Manager when the cursor is clicked.
            /// </summary>
            public void OnClick()
            {
                // Play particle effects
                if (clickParticles != null)
                {
                    clickParticles.Play();
                }

                // Trigger an animation
                if (cursorAnimator != null)
                {
                    cursorAnimator.SetTrigger("Click");
                }
            }
        }

         */
    }
}