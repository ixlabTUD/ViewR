using UnityEngine;

namespace ViewR.Core.OVR.Passthrough.Highlight
{
    /// <summary>
    /// A simple object to configure a passthrough layer.
    /// </summary>
    /// <remarks>
    /// Could be extended.
    /// </remarks>
    [System.Serializable]
    public class PassthroughLayerStyleConfig
    {
        public Color edgeColor = Color.white;
        public float brightness;
        public float contrast;
        public float posterize;
        public bool edgeRenderingEnabled;

        public PassthroughLayerStyleConfig(Color edgeColor = default , float brightness = 0f, float contrast = 0f, float posterize = 0f, bool edgeRenderingEnabled = false)
        {
            this.edgeColor = edgeColor;
            this.brightness = brightness;
            this.contrast = contrast;
            this.posterize = posterize;
            this.edgeRenderingEnabled = edgeRenderingEnabled;
        }
    }
}