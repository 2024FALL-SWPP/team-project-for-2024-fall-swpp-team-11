
using UnityEngine;

namespace NekoLegends
{
    public class AutoDestroy : MonoBehaviour
    {
        public float destroySelfAfterSeconds = .15f;


        private void Start()
        {
            Destroy(gameObject, destroySelfAfterSeconds);
        }

      

    }
}
