using System;
using System.Collections;
using UnityEngine;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Core.OVR.Passthrough.Highlight
{
    /// <summary>
    /// Class to stylize a layer.
    /// <see cref="StartEffect"/> will fade the system towards the PassthroughLayerConfig (highlightedLayerConfig or customHighlightedLayerConfig), and allows to pulse afterwards.
    /// </summary>
    /// <remarks>
    /// Example Usage: See <see cref="ViewR.RapidEnvCreation.ObjectCreation.EnvironmentObjectHighlighter.StartHighlighting"/> / <see cref="ViewR.RapidEnvCreation.ObjectCreation.EnvironmentObjectHighlighter.EndHighlighting"/>
    /// </remarks>
    [RequireComponent(typeof(OVRPassthroughLayer))]
    public class PassthroughLayerStyler : MonoBehaviour
    {
        public PassthroughLayerStyleConfig highlightedLayerStyleConfig;
        public float fadeDuration = .3f;
        [Header("Config")]
        [SerializeField, Range(0, 1)]
        private float maxValueEdge = 0.5f;
        [SerializeField, Range(0, 1)]
        private float maxValueContrast = 0.2f;
        [SerializeField]
        private float timeMultiplier = 0.1f;
        
        private OVRPassthroughLayer _passthroughLayer;

        private IEnumerator _fadeIn;
        private IEnumerator _fadeOut;
        private IEnumerator _pulse;
        private bool _doPulse;

        private PassthroughLayerStyleConfig _previousPassthroughLayerStyleConfig;
        private bool _edgeRenderingEnabledWasActive;
        private PassthroughLayerStyleConfig _editorHighlightedLayerStyleConfig;

        private void Awake()
        {
            _passthroughLayer = GetComponent<OVRPassthroughLayer>();
            
            FetchInitialValuesIfNotRunning();
            _editorHighlightedLayerStyleConfig = highlightedLayerStyleConfig;
        }

        public void StartEffect(PassthroughLayerStyleConfig customHighlightedLayerStyleConfig = null,Action callback = null, bool pulseAfterwards = false)
        {
            FetchInitialValuesIfNotRunning();

            // Use either the editor settings or cache the newly received once.
            highlightedLayerStyleConfig = customHighlightedLayerStyleConfig ?? _editorHighlightedLayerStyleConfig;  
            
            // Stop running effects
            if (_fadeIn != null) StopCoroutine(_fadeIn);
            if (_fadeOut != null) StopCoroutine(_fadeOut);
            if (_pulse != null) StopCoroutine(_pulse);
            
            // Start new coroutine
            _fadeIn = FadeToHighlightedStyle(fadeDuration, callback, pulseAfterwards);
            StartCoroutine(_fadeIn);
        }

        public void StopEffect(Action callback = null)
        {
            // Stop running effects
            if (_fadeIn != null) StopCoroutine(_fadeIn);
            if (_fadeOut != null) StopCoroutine(_fadeOut);
            if (_pulse != null) StopCoroutine(_pulse);

            // Start new coroutine
            _fadeOut = FadeOut(fadeDuration, _previousPassthroughLayerStyleConfig, callback);
            StartCoroutine(_fadeOut);
        }

        /// <summary>
        /// Allows us to fetch the current values and still highlight.
        /// Will not fetch the values if we are currently running, as, then, our initial values would be lost.
        /// </summary>
        private void FetchInitialValuesIfNotRunning()
        {
            // Don't overwrite values if we are still running.
            var currentlyRunning = _fadeIn != null || _fadeOut != null;

            if (!currentlyRunning)
            {
                _previousPassthroughLayerStyleConfig = new PassthroughLayerStyleConfig(edgeColor: _passthroughLayer.edgeColor,
                    brightness: _passthroughLayer.colorMapEditorBrightness,
                    contrast: _passthroughLayer.colorMapEditorContrast,
                    posterize: _passthroughLayer.colorMapEditorPosterize);
            }
        }

        /// <summary>
        /// Fades towards the highlighted style
        /// </summary>
        /// <param name="fadeTime">Duration</param>
        /// <param name="callback">Optional callbacks.</param>
        /// <param name="pulseAfterwards">Should the pulse effect start afterwards?</param>
        private IEnumerator FadeToHighlightedStyle(float fadeTime, Action callback = null, bool pulseAfterwards = false)
        {
            _edgeRenderingEnabledWasActive = _passthroughLayer.edgeRenderingEnabled;
            
            var timer = 0.0f;
            var currentBrightness = _passthroughLayer.colorMapEditorBrightness;
            var currentContrast = _passthroughLayer.colorMapEditorContrast;
            var currentPosterize = _passthroughLayer.colorMapEditorPosterize;
            var currentEdgeColor = _passthroughLayer.edgeColor;
            _passthroughLayer.edgeRenderingEnabled = highlightedLayerStyleConfig.edgeRenderingEnabled;
            while (timer <= fadeTime)
            {
                timer += Time.deltaTime;
                var normTimer = Mathf.Clamp01(timer / fadeTime);
                _passthroughLayer.colorMapEditorBrightness = Mathf.Lerp(currentBrightness, highlightedLayerStyleConfig.brightness, normTimer);
                _passthroughLayer.colorMapEditorContrast = Mathf.Lerp(currentContrast, highlightedLayerStyleConfig.contrast, normTimer);
                _passthroughLayer.colorMapEditorPosterize = Mathf.Lerp(currentPosterize, highlightedLayerStyleConfig.posterize, normTimer);
                _passthroughLayer.edgeColor = Color.Lerp(currentEdgeColor, highlightedLayerStyleConfig.edgeColor, normTimer);
                yield return null;
            }

            if (pulseAfterwards)
            {
                _pulse = Pulse();
                _doPulse = true;
                StartCoroutine(_pulse);
            }
            
            // Fire callback
            callback?.Invoke();
        }

        /// <summary>
        /// Pluses over contrast and edge color time. 
        /// </summary>
        /// <remarks>
        /// ToDo: Could Tween instead to avoid initial jump.
        /// </remarks>
        private IEnumerator Pulse()
        {
            if (!_passthroughLayer)
            {
                Debug.Log("There's no passthroughLayer to modify... Bailing.".StartWithFrom(GetType()), this);
                yield break;
            }

            while (_doPulse)
            {
                var colorHSV = Time.time * timeMultiplier;
                colorHSV %= maxValueEdge; // ensure color value is within [0...1], s.t. Color.HSVToRGB functions correctly
                var edgeColor = Color.HSVToRGB(colorHSV, 1, 1);
                _passthroughLayer.edgeColor = edgeColor;

                var contrastRange = Mathf.Sin(Time.time) * maxValueContrast; // returns a value -1...1, ideal range for contrast
                _passthroughLayer.SetColorMapControls(contrastRange);
            }
        }
        
        /// <summary>
        /// Fades back to the previous settings.
        /// </summary>
        /// <param name="fadeTime">Duration</param>
        /// <param name="passthroughLayerStyleConfig"> config to fade towards to.</param>
        /// <param name="callback">Optional callbacks.</param>
        private IEnumerator FadeOut(float fadeTime, PassthroughLayerStyleConfig passthroughLayerStyleConfig, Action callback = null)
        {
            // Stop pulse
            if (_pulse != null)
            {
                StopCoroutine(_pulse);
                // Tidy up
                _doPulse = false;
                _pulse = null;
            }
            
            var timer = 0.0f;
            var currentBrightness = _passthroughLayer.colorMapEditorBrightness;
            var currentContrast = _passthroughLayer.colorMapEditorContrast;
            var currentPosterize = _passthroughLayer.colorMapEditorPosterize;
            var currentEdgeColor = _passthroughLayer.edgeColor;
            while (timer <= fadeTime)
            {
                timer += Time.deltaTime;
                var normTimer = Mathf.Clamp01(timer / fadeTime);
                _passthroughLayer.colorMapEditorBrightness = Mathf.Lerp(currentBrightness, passthroughLayerStyleConfig.brightness, normTimer);
                _passthroughLayer.colorMapEditorContrast = Mathf.Lerp(currentContrast, passthroughLayerStyleConfig.contrast, normTimer);
                _passthroughLayer.colorMapEditorPosterize = Mathf.Lerp(currentPosterize, passthroughLayerStyleConfig.posterize, normTimer);
                _passthroughLayer.edgeColor = Color.Lerp(currentEdgeColor, passthroughLayerStyleConfig.edgeColor, normTimer);
                yield return null;
            }

            // restore edges to previous setting.
            _passthroughLayer.edgeRenderingEnabled = _edgeRenderingEnabledWasActive;
            
            // Fire callback
            callback?.Invoke();
        }
        
        
    }
}