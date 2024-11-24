using System.Collections;
using UnityEngine;

namespace NekoLegends
{
    public class RotateAnimator : MonoBehaviour
    {
        public bool _isEnabled = false;
        public float _speed = 50f;
        public float _angle;


        [Header("Advance Settings")] 
        public bool UseAdvancedSettings;

        public bool lockXAxis = false;
        public bool lockYAxis = false;
        public bool lockZAxis = false;



        void Update()
        {
            if (_isEnabled)
            {
                if (UseAdvancedSettings)
                {
                    RotateObjectAdvanced();
                }
                else
                {
                    // Desired orientation based on the current angle
                    Quaternion targetRotation = Quaternion.Euler(_angle, transform.localEulerAngles.y + _speed * Time.deltaTime, 0);

                    // Smoothly interpolate towards the desired orientation
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * _speed);
                }
            }
        }

        private void RotateObjectAdvanced()
        {
            float angle = _speed * Time.deltaTime;

            // Calculate the incremental rotation
            Vector3 rotationIncrement = new Vector3(
                lockXAxis ? 0 : angle,
                lockYAxis ? 0 : angle,
                lockZAxis ? 0 : angle
            );

            // Apply the rotation
            transform.Rotate(rotationIncrement, Space.Self);
        }

    }
}