using System;
using UnityEngine;
using UnityEngine.UI;
using ViewR.Core.Networking.Normcore;
using ViewR.Managers;

namespace ViewR.Core.OVR.Passthrough.Overlay
{
    public class PassthroughOpacitySelectiveToSlider : MonoBehaviour
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
            passthroughSettingsSync.passthroughSelectiveOpacityDidChange.AddListener(HandleSelectiveOpacityChange);

            // Set current value
            slider.value = passthroughSettingsSync.GetCurrentOpacitySelective();
        }

        private void OnDisable()
        {
            // Unsubscribe
            passthroughSettingsSync.passthroughSelectiveOpacityDidChange.RemoveListener(HandleSelectiveOpacityChange);
        }

        private void HandleSelectiveOpacityChange(float newValue)
        {
            // This threshold should suffice to ensure we don't run endless loops.
            if(Math.Abs(slider.value - newValue) > updateSliderTolerance)
                slider.value = newValue;
        }
    }
}