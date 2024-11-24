using UnityEngine;


namespace NekoLegends
{
    public static class Utils
    {
        /// <summary>
        /// Recursively stops all coroutines running on the provided transform and its children.
        /// </summary>
        /// <param name="parentTransform">The parent transform to start stopping coroutines from.</param>
        public static void StopAllCoroutinesInChildren(Transform parentTransform)
        {
            MonoBehaviour[] components = parentTransform.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour component in components)
            {
                component.StopAllCoroutines();
            }

            // Recursively stop coroutines in children
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                StopAllCoroutinesInChildren(parentTransform.GetChild(i));
            }
        }
    }
}
