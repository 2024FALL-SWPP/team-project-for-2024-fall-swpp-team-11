using System.Collections;
using UnityEngine;

namespace NekoLegends
{
    public class Shake : MonoBehaviour
    {
        [Header("Shake Settings")]

        [Range(0.1f, 2f)] // Adjust the range as needed
        public float shakeAmount = 0.5f; // magnitude of shake
        public float shakeDuration = 0.5f; // duration of shake, adjust as needed

        private void Start()
        {

            StartCoroutine(ShakeSelf());
        }



        private IEnumerator ShakeSelf()
        {

            Vector3 originalPosition = this.transform.position;

            float elapsed = 0.0f;

            while (elapsed < shakeDuration)
            {
                float x = Random.Range(-1f, 1f) * shakeAmount;
                float y = Random.Range(-1f, 1f) * shakeAmount;

                this.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
                elapsed += Time.deltaTime;

                yield return null;
            }

            this.transform.position = originalPosition;
        }

    }
}
