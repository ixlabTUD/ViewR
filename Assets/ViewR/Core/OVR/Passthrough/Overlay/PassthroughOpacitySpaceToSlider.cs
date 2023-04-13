using System;
using UnityEngine;
using UnityEngine.UI;
using ViewR.Core.Networking.Normcore;
using ViewR.Managers;

namespace ViewR.Core.OVR.Passthrough.Overlay
{
    public class PassthroughOpacitySpaceToSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private float updateSliderTolerance = 0.01f;

        [SerializeField]
        private PassthroughSettingsSync passthroughSettingsSync;

        private void OnEnable()
        {
            // Subscribe
            passthroughSettingsSync.passthroughSpaceOpacityDidChange.AddListener(HandleSpaceOpacityChange);

            // Set current value
            slider.value = passthroughSettingsSync.GetCurrentOpacitySpace();
        }

        private void OnDisable()
        {
            // Unsubscribe
            passthroughSettingsSync.passthroughSpaceOpacityDidChange.RemoveListener(HandleSpaceOpacityChange);
        }

        private void HandleSpaceOpacityChange(float newValue)
        {
            // This threshold should suffice to ensure we don't run endless loops.
            if(Math.Abs(slider.value - newValue) > updateSliderTolerance)
                slider.value = newValue;
        }
    }
}