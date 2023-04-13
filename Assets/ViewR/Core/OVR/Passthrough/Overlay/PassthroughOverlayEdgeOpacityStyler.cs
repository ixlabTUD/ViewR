using System;
using UnityEngine;

namespace ViewR.Core.OVR.Passthrough.Overlay
{
    /// <summary>
    /// A class that only changes the edge defaultOpacity.
    ///
    /// Additionally now used in conjunction with <see cref="PassthroughOverlayStyler"/>
    /// </summary>
    public class PassthroughOverlayEdgeOpacityStyler : MonoBehaviour
    {
        [SerializeField]
        private OVRPassthroughLayer overlayLayer;

        [SerializeField]
        private float changedTolerance = 0.005f;
        
        public void SetEdgeOpacity(float value)
        {
            if (!(Math.Abs(overlayLayer.textureOpacity - value) > changedTolerance)) return;
            
            // Set value
            overlayLayer.edgeColor = new Color(overlayLayer.edgeColor.r, overlayLayer.edgeColor.g, overlayLayer.edgeColor.b, value);

            // Fire away!
            OnEdgeOpacityDidChange(value);
        }

        public float GetOverlayEdgeOpacity() => overlayLayer.edgeColor.a;

        public delegate void OverlayEdgeOpacityChanged (float newValue);
        
        public static event OverlayEdgeOpacityChanged OverlayEdgeOpacityDidChange;

        public static void OnEdgeOpacityDidChange(float newValue)
        {
            OverlayEdgeOpacityDidChange?.Invoke(newValue);
        }
    }
}