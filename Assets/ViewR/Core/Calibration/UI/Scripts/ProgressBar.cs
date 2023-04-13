using System.Collections;
using UnityEngine;
using ViewR.Core.UI.FloatingUI.Loading.Visuals;


    public class ProgressBar : MonoBehaviour
    {
        public delegate void chargeSuccess();

        public GameObject uiParent;

        public AnimationCurve scaleCurve;

        public float chargeTime = 3f;

        [SerializeField] private CircleProgressBar circleProgressBar;

        [SerializeField] private RectTransform uiCanvas;


        private float _chargeValue;
        private bool _successFiredOnceInCycle;
        private float _timer;
        private float finalScale;

        private float graphValue;
        private readonly float initialScale = 0f;


        private float ChargeValue
        {
            get => _chargeValue;
            set
            {
                _chargeValue = value;

                // Update Visuals
                circleProgressBar.Progress = _chargeValue;
            }
        }

        public bool Running => ChargeValue > 0;

        private void Start()
        {
            finalScale = uiCanvas.localScale.x;
            // Make it disappear on start
            uiParent.SetActive(false);
            uiCanvas.localScale = new Vector3(initialScale, initialScale, initialScale);
        }


        private void OnDisable()
        {
            DisableProgressBar();
        }

        public event chargeSuccess chargeSuccessEvent;

        // Main point of entrance
        public void EnableProgressBar()
        {
            if (Running)
                return;

            if (ChargeValue < 0)
                ResetFlags();

            // Appear In
            StartCoroutine(scaleUI(false));
        }

        // Call this to increase the timer! i.e. on Update
        public void IncreaseProgressBar()
        {
            if (ChargeValue < 1)
            {
                // Increase fill
                _timer += Time.deltaTime;
                ChargeValue = _timer / chargeTime;
            }
            else
            {
                ChargeValue = 1;
                // If 100%: Request action. 
                InvokeSuccessOnce();
            }
        }

        // Main point of exit.
        public void DisableProgressBar()
        {
            if (!Running)
                return;

            // Appear out
            StartCoroutine(scaleUI(true));

            // Reset
            ResetFlags();
        }

        private void InvokeSuccessOnce()
        {
            if (_successFiredOnceInCycle)
                return;

            // Invoke
            chargeSuccessEvent?.Invoke();

            // Set flag
            _successFiredOnceInCycle = true;
        }

        private IEnumerator scaleUI(bool invert)
        {
            if (invert)
                uiParent.SetActive(false);
            else
                uiParent.SetActive(true);

            float i = 0;
            var rate = 1 / 0.2f;
            while (i < 1)
            {
                i += rate * Time.deltaTime;

                var sampleValue = i;
                if (invert) sampleValue = 1 - i;
                graphValue = scaleCurve.Evaluate(sampleValue);
                uiCanvas.localScale = new Vector3(finalScale * graphValue, finalScale * graphValue,
                    finalScale * graphValue);
                yield return 0;
            }
        }

        private void ResetFlags()
        {
            _successFiredOnceInCycle = false;
            ChargeValue = 0;
            _timer = 0;
        }
    }
