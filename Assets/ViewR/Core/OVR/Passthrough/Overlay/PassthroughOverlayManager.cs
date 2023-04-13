using System;
using UnityEngine;

namespace ViewR.Core.OVR.Passthrough.Overlay
{
    /// <summary>
    /// A class to configure the Overlay Passthrough layer.
    /// Remember that each Layer has a rather large performance overhead.
    /// </summary>
    public class PassthroughOverlayManager : MonoBehaviour
    {
        [SerializeField]
        private OVRPassthroughLayer overlayLayer;

        [SerializeField]
        private float hidingThreshold = 0.019f;
        
        [SerializeField]
        private float changedTolerance = 0.005f;
        
        public const float MAX_OVERLAY_OPACITY = 0.9f;

        private void OnEnable()
        {
            PassthroughOverlayEdgeOpacityStyler.OverlayEdgeOpacityDidChange += HandleEdgeOpacityChanges;
        }

        private void OnDisable()
        {
            PassthroughOverlayEdgeOpacityStyler.OverlayEdgeOpacityDidChange -= HandleEdgeOpacityChanges;
        }


        /// <summary>
        /// Ensures the PT is enabled if the edge filter is set "on"
        /// </summary>
        /// <param name="newValue"></param>
        private void HandleEdgeOpacityChanges(float newValue)
        {
            if(newValue > 0 && overlayLayer.hidden)
                SetOverlayOpacity(.02f);
            if(newValue == 0)
                SetOverlayOpacity(0);
        }

        public void SetOverlayOpacity(float value)
        {
            // Hide Layer and bail if less than threshold
            if (value < hidingThreshold)
            {
                // Ensure we still update the value!
                TrySetOpacity(value);

                if (!overlayLayer.hidden)
                    overlayLayer.hidden = true;
                return;
            }

            // Ensure we show it
            if (overlayLayer.hidden)
                overlayLayer.hidden = false;

            TrySetOpacity(value);
        }

        /// <summary>
        /// Sets the <see cref="overlayLayer"/>'s <see cref="OVRPassthroughLayer.textureOpacity"/> if the new <see cref="value"/>'s absolute difference to its current value is larger than <see cref="changedTolerance"/>
        /// </summary>
        private void TrySetOpacity(float value)
        {
            if (Math.Abs(overlayLayer.textureOpacity - value) > changedTolerance)
            {
                // Set value
                overlayLayer.textureOpacity = value;
                // Fire away!
                OnOverlayPassthroughOpacityDidChange(value);
            }
        }

        public float GetOverlayOpacity() => overlayLayer.textureOpacity;

        public delegate void OverlayPassthroughOpacityChanged (float newValue);
        
        public static event OverlayPassthroughOpacityChanged OverlayPassthroughOpacityDidChange;

        private static void OnOverlayPassthroughOpacityDidChange(float newValue)
        {
            OverlayPassthroughOpacityDidChange?.Invoke(newValue);
        }
    }
}