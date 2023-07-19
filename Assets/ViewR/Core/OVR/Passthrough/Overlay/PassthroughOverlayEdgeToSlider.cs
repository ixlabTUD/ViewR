using System;
using UnityEngine;
using UnityEngine.UI;
using ViewR.Managers;

namespace ViewR.Core.OVR.Passthrough.Overlay
{
    public class PassthroughOverlayEdgeToSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private float updateSliderTolerance = 0.01f;

        private void OnEnable()
        {
            PassthroughOverlayEdgeOpacityStyler.OverlayEdgeOpacityDidChange += HandleOpacityChange;

            // Set current value, after scaling it from [0 ... 255] to [0 ... 1]
            // slider.value = ScaleRange(passthroughOverlayEdgeAccess.PassthroughOverlayEdgeOpacityStyler.GetOverlayEdgeOpacity());
            slider.value = PassthroughManager.Instance.passthroughOverlayEdgeOpacityStyler.GetOverlayEdgeOpacity();
        }
        

        private void OnDisable()
        {
            PassthroughOverlayEdgeOpacityStyler.OverlayEdgeOpacityDidChange -= HandleOpacityChange;
        }
        
        private void HandleOpacityChange(float newValue)
        {
            // This threshold should suffice to ensure we don't run endless loops.
            if(Math.Abs(slider.value - newValue) > updateSliderTolerance)
                slider.value = newValue;
        }

        /// <summary>
        /// Converts [0 ... 255] -> [0 ... 1]
        /// </summary>
        private float ScaleRange(float newAlphaValue) => Mathf.InverseLerp(0, 255, newAlphaValue);
    }
}