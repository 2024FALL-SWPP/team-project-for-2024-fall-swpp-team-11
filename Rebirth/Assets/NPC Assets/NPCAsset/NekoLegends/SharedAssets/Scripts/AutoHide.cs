using System.Collections;
using UnityEngine;

namespace NekoLegends
{
    public class AutoHide : MonoBehaviour
    {
        [Tooltip("Time in seconds before the GameObject auto-hides after being enabled.")]
        public float autoHideDelay = 2f;

        // Reference to the running coroutine, if any
        private Coroutine hideCoroutine;

        private void OnEnable()
        {
            // If a hide coroutine is already running, stop it to prevent multiple coroutines
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }

            // Start the hide coroutine
            hideCoroutine = StartCoroutine(AutoHideCoroutine());
        }

        private void OnDisable()
        {
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
                hideCoroutine = null;
            }
        }

        private IEnumerator AutoHideCoroutine()
        {
            // Wait for the specified delay
            yield return new WaitForSeconds(autoHideDelay);

            // Deactivate the GameObject
            gameObject.SetActive(false);
        }
    }
}
