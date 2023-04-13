using System;
using UnityEngine;
using UnityEngine.UI;
using ViewR.Managers;

namespace ViewR.Core.UI.MainUI.UI.ConfigMenu
{
    /// <summary>
    /// Ensures we show the right visuals based on the users stored preferences on startup
    ///
    /// Access point to update values in  <see cref="UserConfig"/>.
    /// </summary>
    public class ConfigUserHandedness : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField]
        private Toggle leftHandedToggle;
        [SerializeField]
        private Toggle rightHandedToggle;
        
        [Header("Debugging")]
        [SerializeField]
        private bool debugging;
        
        private void OnEnable()
        {
            UpdateVisuals();
        }

        /// <summary>
        /// Ensures the right toggle is shown.
        /// </summary>
        private void UpdateVisuals()
        {
            switch (UserConfig.Instance.Handedness)
            {
                case OVRPlugin.Handedness.LeftHanded:
                    rightHandedToggle.isOn = false;
                    leftHandedToggle.isOn = true;
                    break;
                case OVRPlugin.Handedness.RightHanded:
                    rightHandedToggle.isOn = true;
                    leftHandedToggle.isOn = false;
                    break;
                case OVRPlugin.Handedness.Unsupported:
                    rightHandedToggle.isOn = false;
                    leftHandedToggle.isOn = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SelectRightHanded(bool active)
        {
            if(active)
                // Set value:
                SetUserHandedness(OVRPlugin.Handedness.RightHanded);
        }

        public void SelectLeftHanded(bool active)
        {
            if(active)
                // Set value:
                SetUserHandedness(OVRPlugin.Handedness.LeftHanded);
        }

        /// <summary>
        /// Sets the users handedness.
        /// </summary>
        /// <param name="handedness"></param>
        public void SetUserHandedness(OVRPlugin.Handedness handedness)
        {
            // Set value:
            UserConfig.Instance.Handedness = handedness;
        } 
    }
}
