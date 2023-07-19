using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions;
using UnityEngine;
using ViewR.Core.OVR.Passthrough.Highlight;

namespace ViewR.Core.OVR.Passthrough.Overlay
{
    /// <summary>
    /// A class to aid the overlays visual representation
    /// Primarily for Default/Edge Overlay buttons.
    /// </summary>
    public class PassthroughOverlayStyler : MonoBehaviour
    {
        public PassthroughLayerStyleConfig defaultStyle;
        public PassthroughLayerStyleConfig edgeStyle;

        [SerializeField]
        private TweenConfig tweenConfig;

        [SerializeField]
        private bool disableEdgeStyleForDefaultStyle = true;

        [SerializeField]
        private OVRPassthroughLayer passthroughLayer;
        public OVRPassthroughLayer PassthroughLayer => passthroughLayer;

        private TweenBase _tweenBase;
        private PassthroughLayerStyleConfig _currentStyleConfig;
        private float _currentBrightness;
        private float _currentContrast;
        private float _currentPosterize;
        private Color _currentEdgeColor;
        private bool _currentlyFadingToEdgeStyle;
        private float _previousEdgeAlphaSetting = 0f;


        public void Start()
        {
            FadeToStyle(true);
        }
        

        public void FadeToStyle(bool toEdgeStyle)
        {
            // cache values if we toggle off:
            if (!toEdgeStyle)
            {
                // store alpha value:
                _previousEdgeAlphaSetting = passthroughLayer.edgeColor.a;
            }

            _currentStyleConfig = toEdgeStyle ? edgeStyle : defaultStyle;
            _currentlyFadingToEdgeStyle = toEdgeStyle;
            _currentBrightness = passthroughLayer.colorMapEditorBrightness;
            _currentContrast = passthroughLayer.colorMapEditorContrast;
            _currentPosterize = passthroughLayer.colorMapEditorPosterize;
            _currentEdgeColor = passthroughLayer.edgeColor;

            // Stop other tweens
            _tweenBase?.Stop();

            // Start our tween
            _tweenBase = Tween.Value(
                0f,
                1f,
                ValueUpdatedCallback,
                tweenConfig.Duration,
                tweenConfig.Duration,
                tweenConfig.AnimationCurve,
                tweenConfig.loopType,
                completeCallback: CompleteCallback,
                obeyTimescale: tweenConfig.obeyTimescale);
        }

        /// <summary>
        /// Disables the edge rendering fading back to default && if <see cref="disableEdgeStyleForDefaultStyle"/>
        /// </summary>
        private void CompleteCallback()
        {
            if (disableEdgeStyleForDefaultStyle && !_currentlyFadingToEdgeStyle)
                passthroughLayer.edgeRenderingEnabled = false;
        }

        /// <summary>
        /// Applies the tween.
        /// </summary>
        private void ValueUpdatedCallback(float newValue)
        {
            passthroughLayer.edgeRenderingEnabled = _currentStyleConfig.edgeRenderingEnabled;
            passthroughLayer.colorMapEditorBrightness =
                Mathf.Lerp(_currentBrightness, _currentStyleConfig.brightness, newValue);
            passthroughLayer.colorMapEditorContrast =
                Mathf.Lerp(_currentContrast, _currentStyleConfig.contrast, newValue);
            passthroughLayer.colorMapEditorPosterize =
                Mathf.Lerp(_currentPosterize, _currentStyleConfig.posterize, newValue);

            //! Fade to previous edge color
            var targetColor = _currentStyleConfig.edgeColor;

            // If there is a previous alpha that is not 0, fade towards it instead of the configured layer value! 
            if (_currentlyFadingToEdgeStyle && _previousEdgeAlphaSetting != 0)
                targetColor = new Color(targetColor.r, targetColor.g, targetColor.b, _previousEdgeAlphaSetting);

            passthroughLayer.edgeColor = Color.Lerp(_currentEdgeColor, targetColor, newValue);

            // Ensure we fire the event to update the ui.
            PassthroughOverlayEdgeOpacityStyler.OnEdgeOpacityDidChange(Mathf.Lerp(_currentEdgeColor.a, targetColor.a, newValue));
        }

        #region Injection
        
        public void InjectNewTargetStyle(PassthroughLayerStyleConfig newStyle, bool autoFadeToStyle = true)
        {
            edgeStyle = newStyle;

            if (autoFadeToStyle)
                FadeToStyle(true);
        }

        #endregion
    }
}