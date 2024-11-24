using System.Collections;
using UnityEngine;

namespace NekoLegends
{
    using UnityEngine;
    

    public class SwayAnimator : MonoBehaviour
    {
        public bool _isEnabled = false;
        public float _speed = 0.2f;
        public float _range = 20f;
        public float _angle;

        private float _localTime = 0f;
        private bool _firstEnable = true; // Flag to indicate the first time the script is enabled

        void Update()
        {
            if (_isEnabled)
            {
                // Handle the first enable
                if (_firstEnable)
                {
                    // Calculate initial _localTime based on current rotation
                    float currentPP = transform.localEulerAngles.y;
                    _localTime = Mathf.Acos(currentPP / _range) / (_speed * Mathf.PI);
                    _firstEnable = false;
                }

                // Increment local time
                _localTime += Time.deltaTime;

                // Calculate new position
                float pp = Mathf.Cos(_localTime * _speed * Mathf.PI) * _range;

                transform.localRotation = Quaternion.Euler(_angle, pp, 0);
            }
            else
            {
                _firstEnable = true; // Reset the first enable flag when disabled
            }
        }
    }


}
