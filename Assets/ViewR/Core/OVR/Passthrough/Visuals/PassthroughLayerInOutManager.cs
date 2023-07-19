using System;
using System.Collections;
using UnityEngine;
using ViewR.Core.OVR.Passthrough.Visuals.Blink;
using ViewR.Core.OVR.Passthrough.Visuals.Fader;
using ViewR.Managers;

namespace ViewR.Core.OVR.Passthrough.Visuals
{
    /// <summary>
    /// A class to manage the layers FadeIn/FadeOut.
    /// Option to hide layers after fading.
    /// Ensures layers are activated.
    /// Need one for each <see cref="LayerType"/>, as we need an object for the coroutines.
    /// Caches the <see cref="OVRPassthroughLayer"/>
    ///
    /// This is the heart of <see cref="BlinkPassthroughLayer"/> and <see cref="FadePassthroughLayer"/>.
    /// </summary>
    public class PassthroughLayerInOutManager : MonoBehaviour
    {
        [SerializeField]
        private LayerType layerType;

        private OVRPassthroughLayer _cachedPassthroughLayer;
        private Coroutine _fadeCoroutine;
        private Coroutine _fadeLayerOutAndInCoroutine;

        public const float FADE_TIME = 1;

        private void OnDisable()
        {
            _cachedPassthroughLayer = null;
        }

        /// <summary>
        /// Fade in the PT layer from black to visual or vise versa.
        /// Additionally, optionally, hides the layer upon fading, if <see cref="hideAfterFadeOut"/> is set when fading out.
        /// </summary>
        public void FadeInPassthrough(bool fadeIn, bool hideAfterFadeOut = true)
        {
            // Stop existing ones if we start a new one.
            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);

            // Cache Layer if none 
            if (_cachedPassthroughLayer == null)
            {
                // Cache our layer.
                _cachedPassthroughLayer = layerType switch
                {
                    LayerType.Main => PassthroughManager.Instance.mainPassthroughLayer,
                    LayerType.Reprojected => PassthroughManager.Instance.userDefinedPassthroughLayer,
                    LayerType.ReprojectedHighlighted => PassthroughManager.Instance.userDefinedPassthroughLayerHighlighter,
                    LayerType.Overlay => PassthroughManager.Instance.overlayPassthroughLayer,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            // Fade it
            _fadeCoroutine = StartCoroutine(FadeInPassthrough(_cachedPassthroughLayer, fadeIn: fadeIn, hideLayerWhenCompleted: hideAfterFadeOut));
        }

        /// <summary>
        /// Fades the configured layer Out and in again. A blink-like effect!
        /// </summary>
        public void FadePassthroughOutAndIn()
        {
            // Stop existing ones if we start a new one.
            if (_fadeLayerOutAndInCoroutine != null)
                StopCoroutine(_fadeLayerOutAndInCoroutine);

            // Cache Layer if none 
            if (_cachedPassthroughLayer == null)
            {
                // Cache our layer.
                _cachedPassthroughLayer = layerType switch
                {
                    LayerType.Main => PassthroughManager.Instance.mainPassthroughLayer,
                    LayerType.Reprojected => PassthroughManager.Instance.userDefinedPassthroughLayer,
                    LayerType.ReprojectedHighlighted => PassthroughManager.Instance.userDefinedPassthroughLayerHighlighter,
                    LayerType.Overlay => PassthroughManager.Instance.overlayPassthroughLayer,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            
            // Fade it
            _fadeLayerOutAndInCoroutine = StartCoroutine(FadeLayerOutAndIn(_cachedPassthroughLayer));
        }


        /// <summary>
        /// Fades in the Passthrough.
        /// Also enables the layer if it was currently hidden. Don't forget to hide again if no longer needed to save performance!
        /// Additional options: Make objects appear, and, again optionally, reposition them.
        /// </summary>
        /// <param name="passthroughLayer">The <see cref="OVRPassthroughLayer"/> to modify.</param>
        /// <param name="fadeIn">Fading in or fading out?</param>
        /// <param name="hideLayerWhenCompleted">If fade out: Should we hide the layer once this animation is completed?</param>
        /// <param name="objectsToToggle">Objects to show (if fading in) or hide (if fading out), once fading is done</param>
        /// <param name="repositionObjectsInFrontOfCamera">Should we reposition these objects in front of the camera?</param>
        /// <param name="height"> Height to position them at.</param>
        /// <returns></returns>
        private IEnumerator FadeInPassthrough(OVRPassthroughLayer passthroughLayer, bool fadeIn = true, bool hideLayerWhenCompleted = true, GameObject[] objectsToToggle = null, bool repositionObjectsInFrontOfCamera = false, float height = .8f)
        {
            // Toggle objects if required.
            if(objectsToToggle != null)
                foreach (var o in objectsToToggle)
                {
                    o.SetActive(!fadeIn);
                }

            // Show the layer if currently hidden:
            if (passthroughLayer.hidden)
                passthroughLayer.hidden = false;

            // Fade!
            var timer = 0.0f;
            while (timer <= FADE_TIME)
            {
                timer += Time.deltaTime;
                var normTimer = Mathf.Clamp01(timer / FADE_TIME);
                
                // Invert value if we fade out.
                if (!fadeIn)
                    normTimer = normTimer * -1f + 1f;
                
                // Apply effect
                passthroughLayer.colorMapEditorBrightness = Mathf.Lerp(-1.0f, 0.0f, normTimer);
                passthroughLayer.colorMapEditorContrast = Mathf.Lerp(-1.0f, 0.0f, normTimer);
                
                yield return null;
            }
            
            // Wait a moment
            yield return new WaitForSeconds(.234f);
            
            // Hide the layer if we faded out and configured it to be hidden upon completion.
            if(!fadeIn)
                if (hideLayerWhenCompleted)
                    passthroughLayer.hidden = true;
            
            // Toggle objects
            if(objectsToToggle != null)
                foreach (var o in objectsToToggle)
                {
                    o.SetActive(fadeIn);
                    
                    if (repositionObjectsInFrontOfCamera)
                    {
                        var mainCameraTransform = ReferenceManager.Instance.GetMainCamera().transform;
                        var objFwd = new Vector3(mainCameraTransform.forward.x,
                            height,
                            mainCameraTransform.forward.z).normalized;
                        o.transform.position = mainCameraTransform.position + objFwd;
                        o.transform.rotation = Quaternion.LookRotation(objFwd);
                    }
                }
        }

        /// <summary>
        /// Blink-like animation!
        /// </summary>
        /// <param name="passthroughLayer">The layer to affect.</param>
        /// <returns></returns>
        private IEnumerator FadeLayerOutAndIn(OVRPassthroughLayer passthroughLayer)
        {
            // Stop existing ones if we start a new one.
            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);

            _fadeCoroutine = StartCoroutine(FadeInPassthrough(passthroughLayer, false, false));
            yield return new WaitForSeconds(FADE_TIME);

            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(FadeInPassthrough(passthroughLayer, true));
        }
    }
}