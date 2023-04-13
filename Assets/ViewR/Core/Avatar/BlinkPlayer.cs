using UnityEngine;

namespace ViewR.Core.Avatar
{
    public class BlinkPlayer : MonoBehaviour
    {
        public GameObject lEye;
        public GameObject rEye;

        [Range(0.5f, 2f)]
        public float initialEyeSize = 0.9f;

        [Range(0.0f, 2f)]
        public float blinkEyeSize = 0.0f;

        public float smoothnessEyeBlink = 15f;
        public float blinkingThreshold = 26f;

        private Vector3 _initialEyeScale;
        private Vector3 _blinkEyeScale;
        private bool _blinking = false;
        private bool _unblink = false;
        private float _blinkingThreshold = 0f;
        private Vector3 _tempTransition;
        private float _eyeClosedThreshold = 0.01f;

        // Start is called before the first frame update
        private void Start()
        {
            // initialize values
            _initialEyeScale.Set(1f, initialEyeSize, 1f);
            _blinkEyeScale.Set(_initialEyeScale.x, blinkEyeSize, _initialEyeScale.z);
            lEye.transform.localScale = _initialEyeScale;
            rEye.transform.localScale = _initialEyeScale;
        }

        // Update is called once per frame
        private void Update()
        {
            // start blinking at random time steps
            if (!_blinking && !_unblink)
            {
                _blinkingThreshold += Random.Range(0.0f, 0.5f);
                _blinking = (_blinkingThreshold > blinkingThreshold) ? true : false;
            }


            // handle transitions
            if (_blinking && !_unblink) // if currently closing eyes
            {
                // reset threshold
                _blinkingThreshold = 0f;

                // getting current eye opening 
                _tempTransition = lEye.transform.localScale;

                // transition
                _tempTransition = Vector3.Slerp(_tempTransition, _blinkEyeScale, smoothnessEyeBlink * Time.deltaTime);

                // setting eye scaling
                lEye.transform.localScale = _tempTransition;
                rEye.transform.localScale = _tempTransition;

                // Handling bools
                if (_tempTransition.y - blinkEyeSize < _eyeClosedThreshold) // reached minSize
                {
                    _blinking = false;
                    _unblink = true;
                }
            }

            if (!_blinking && _unblink) // if currently opening eyes
            {
                // getting current eye opening 
                _tempTransition = lEye.transform.localScale;

                // transition
                _tempTransition = Vector3.Slerp(_tempTransition, _initialEyeScale, smoothnessEyeBlink * Time.deltaTime);

                // setting eye scaling
                lEye.transform.localScale = _tempTransition;
                rEye.transform.localScale = _tempTransition;

                // Handling bools
                if (initialEyeSize - _tempTransition.y < _eyeClosedThreshold) // reached normal size again
                {
                    _unblink = false;
                }
            }
        }
    }
}