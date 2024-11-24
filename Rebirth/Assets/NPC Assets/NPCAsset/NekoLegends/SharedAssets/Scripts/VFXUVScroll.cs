using UnityEngine;

namespace NekoLegends
{
    public class VFXUVScroll : MonoBehaviour
    {
        public int materialId = 0;
        public Vector2 scrollSpeed = new Vector2(0.5f, 0.5f);

        private Renderer _renderer;
        private Material _material;
        private Vector2 _currentOffset;

        void Start()
        {
            _renderer = GetComponent<Renderer>();
            if (_renderer == null)
            {
                Debug.LogError("Renderer component not found on the object!");
                return;
            }

            if (_renderer.materials.Length <= materialId)
            {
                Debug.LogError("Invalid materialId! The object doesn't have that many materials.");
                return;
            }

            _material = _renderer.materials[materialId];
        }

        void Update()
        {
            if (_material != null)
            {
                _currentOffset += scrollSpeed * Time.deltaTime;
                _material.SetTextureOffset("_MainTex", _currentOffset);
            }
        }
    }

}