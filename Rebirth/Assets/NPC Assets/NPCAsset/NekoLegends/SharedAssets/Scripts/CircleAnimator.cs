using System.Collections;
using UnityEngine;

namespace NekoLegends
{
    public class CircleAnimator : MonoBehaviour
    {
        public float speed = 1.0f;
        public float radius = 5.0f;
        public bool loop = true;
        public bool counterClockwise;

        private Vector3 center;
        private float angle;

        void Start()
        {
            center = transform.position;
        }

        void Update()
        {
            if (counterClockwise)
            {
                angle += Time.deltaTime * speed;
            }
            else
            {
                angle -= Time.deltaTime * speed;
            }
            this.gameObject.transform.position = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            this.gameObject.transform.LookAt(center);

            if (loop)
            {
                if (angle >= Mathf.PI * 2)
                {
                    angle -= Mathf.PI * 2;
                }
            }
        }
    }
}