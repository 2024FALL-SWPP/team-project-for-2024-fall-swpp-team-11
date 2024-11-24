using UnityEngine;

namespace NekoLegends
{
    public class ScrollingBackground : MonoBehaviour
    {
        [Header("Scroll Settings")]

        public Transform[] backgroundPlanes;   // Assign your 3 plane transforms here
        public float scrollSpeed = 2.0f;       // Base scroll speed for the background

        [Header("Parallax Settings")]
        public Transform character;            // The game object (e.g., character) whose vertical movement influences parallax
        public float parallaxFactor = 0.25f;   // Factor for parallax effect (higher value means more depth effect)

        private float planeWidth;     // Width of each plane
        private int totalPlanes;      // Total number of planes
        private float limitPositionX; // X position to trigger repositioning

        private Vector3 previousCharacterPosition;  // Store the previous local position of the character

        void Start()
        {
            // Ensure there are planes assigned
            if (backgroundPlanes == null || backgroundPlanes.Length == 0)
            {
                Debug.LogError("No background planes assigned.");
                return;
            }

            totalPlanes = backgroundPlanes.Length;

            // Get the plane width in local space
            MeshFilter mf = backgroundPlanes[0].GetComponent<MeshFilter>();
            if (mf != null)
            {
                // Mesh bounds are in local space, so we multiply by the local scale
                planeWidth = mf.mesh.bounds.size.x * backgroundPlanes[0].localScale.x;
            }
            else
            {
                Debug.LogError("MeshFilter not found on background planes.");
                return;
            }

            // Set limit position based on plane width (in local space)
            limitPositionX = -planeWidth * 1.5f;

            // Adjust initial positions to ensure planes are side by side (using local positions)
            for (int i = 0; i < totalPlanes; i++)
            {
                backgroundPlanes[i].localPosition = new Vector3((i - 1) * planeWidth, backgroundPlanes[i].localPosition.y, backgroundPlanes[i].localPosition.z);
            }

            // Store the initial local position of the character if character is set
            if (character != null)
            {
                previousCharacterPosition = backgroundPlanes[0].parent.InverseTransformPoint(character.position);
            }
        }

        void Update()
        {
            // Scroll all planes to the left with a base scroll speed (using local positions)
            foreach (Transform plane in backgroundPlanes)
            {
                plane.localPosition += Vector3.left * scrollSpeed * Time.deltaTime;
            }

            // Check if the character is assigned before applying parallax effect
            if (character != null)
            {
                // Get the character's position relative to the parent
                Vector3 currentCharacterPosition = backgroundPlanes[0].parent.InverseTransformPoint(character.position);

                // Parallax effect: Move planes vertically based on character's vertical movement
                float deltaY = currentCharacterPosition.y - previousCharacterPosition.y;
                foreach (Transform plane in backgroundPlanes)
                {
                    float parallaxSpeed = deltaY * parallaxFactor;
                    plane.localPosition += new Vector3(0, parallaxSpeed, 0);
                }

                // Update the previous character position for the next frame
                previousCharacterPosition = currentCharacterPosition;
            }

            // Reposition planes that have moved completely off-screen
            foreach (Transform plane in backgroundPlanes)
            {
                // Calculate the right edge of the plane (using local positions)
                float rightEdge = plane.localPosition.x + (planeWidth / 2);

                // If the right edge is less than or equal to limitPositionX, reposition the plane
                if (rightEdge <= limitPositionX)
                {
                    RepositionPlane(plane);
                }
            }
        }

        void RepositionPlane(Transform plane)
        {
            // Find the rightmost plane (using local positions)
            float maxX = float.MinValue;
            foreach (Transform p in backgroundPlanes)
            {
                if (p != plane && p.localPosition.x > maxX)
                {
                    maxX = p.localPosition.x;
                }
            }

            // Reposition the plane to the right of the rightmost plane (using local positions)
            plane.localPosition = new Vector3(maxX + planeWidth, plane.localPosition.y, plane.localPosition.z);
        }
    }
}
