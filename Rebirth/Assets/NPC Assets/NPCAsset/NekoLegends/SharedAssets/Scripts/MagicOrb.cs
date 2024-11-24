using System.Collections;
using UnityEngine;

namespace NekoLegends
{
    public class MagicOrb : MonoBehaviour
    {
        public Vector3 From, To;
        public float duration = 2f;

        public bool isLoop = true;

        private void Start()
        {
            StartFloatingAnimation();
        }

        private IEnumerator AnimatePosition()
        {
            while (isLoop)
            {
                yield return AnimateFromTo(From, To, duration);
                yield return AnimateFromTo(To, From, duration);
            }
        }

        private IEnumerator AnimateFromTo(Vector3 startPos, Vector3 endPos, float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                transform.localPosition = Vector3.Lerp(startPos, endPos, t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure the position is set to the endPos when the animation is done
            transform.localPosition = endPos;
        }
        
        public void StopFloatingAnimation()
        {
            isLoop = false;
            StopCoroutine(AnimatePosition());
            SetVisible(false);
        }

        public void StartFloatingAnimation()
        {
            SetVisible(true);  // Activate the game object first
            isLoop = true;
            StartCoroutine(AnimatePosition());
        }


        public void SetVisible(bool visible)
        {
            this.gameObject.SetActive(visible);
        }

    }
}
