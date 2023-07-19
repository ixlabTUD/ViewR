using System;
using UnityEngine;
using UnityEngine.UI;
using ViewR.StatusManagement;

namespace ViewR.Core.UI.MainUI.UI.ConfigMenu
{
    /// <summary>
    /// Ensures the Max slider value == options in <see cref="PassthroughLevel"/>.
    /// Sets the slider.value to the current Passthrough level on enable - only if they are not the same.
    /// </summary>
    public class ClientPassthroughToSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        private void Awake()
        {
            // Set max value
            slider.maxValue = Enum.GetNames(typeof(PassthroughLevel)).Length - 1;
        }

        private void OnEnable()
        {
            UpdateSlider((int) ClientPassthroughLevel.CurrentPassthroughLevel);
            
            // Subscribe
            ClientPassthroughLevel.PassthroughLevelUpdated += OnPassthroughLevelUpdated;
        }

        private void OnDisable()
        {
            // Unsubscribe
            ClientPassthroughLevel.PassthroughLevelUpdated -= OnPassthroughLevelUpdated;
        }

        private void OnPassthroughLevelUpdated(PassthroughLevel passthroughLevel)
        {
            UpdateSlider((int) ClientPassthroughLevel.CurrentPassthroughLevel);
        }

        private void UpdateSlider(int value)
        {
            // If slider != current Passthrough level:
            if (Math.Abs(slider.value - value) > 0.05f)
                slider.value = value;
        }
    }
}