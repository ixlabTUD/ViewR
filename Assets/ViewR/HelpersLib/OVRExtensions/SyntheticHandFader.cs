using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using ViewR.HelpersLib.Extensions.DelayedCallbacks;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.HelpersLib.OVRExtensions
{
    /// <summary>
    /// Fades the synthetic hands material in and out.
    /// </summary>
    public class SyntheticHandFader : DelayedCallbacksTrackerMono
    {
        [Header("References")]
        [SerializeField]
        private new Renderer renderer;
        [SerializeField]
        private GameObject syntheticHand;
        [FormerlySerializedAs("tweenConfig")]
        [Header("Tween")]
        [SerializeField]
        private TweenConfig tweenConfigFadeIn;
        [SerializeField]
        private TweenConfig tweenConfigFadeOut;
        [Header("Shader")]
        [SerializeField]
        private string shaderPropertyToModifyCore = "_Opacity";
        [SerializeField]
        private string shaderPropertyToModifyOutline = "_OutlineOpacity";

        [Header("Opacities")]
        [SerializeField]
        private float maxValueCore = 0.27f;
        [SerializeField]
        private float maxValueOutline = 1;

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;
        
        private bool _initialized;
        private Material _material;
        private TweenBase _tweenBase;
        
        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            Assert.IsNotNull(renderer);
        }

        private void Initialize(bool overwrite = false)
        {
            // Bail if we are already initialized and not supposed to overwrite 
            if(!overwrite && _initialized)
                return;
            
            // Ensure we have a renderer 
            Assert.IsNotNull(renderer);

            // Get the material 
            _material = renderer.material;

            // Ensure it has the properties needed. 
            if(!_material.HasFloat(shaderPropertyToModifyOutline) || !_material.HasFloat(shaderPropertyToModifyCore))
            {
                throw new MissingMemberException($"The shader of this material should have a property called \"{shaderPropertyToModifyCore}\" and \"{shaderPropertyToModifyOutline}\" ".StartWithFrom(GetType()));
            }
            
            _initialized = true;
        }

        [ExposeMethodInEditor]
        public void FadeIn() => FadeIn(true);
        [ExposeMethodInEditor]
        public void FadeOut() => FadeIn(false);
        
        public void FadeIn(bool fadeIn = true)
        {
            if(!_initialized)
                Initialize();
            if(!_initialized)
            {
                Debug.LogError("Could not initialize. Bailing.", this);
                return;
            }
            
            if(fadeIn)
                EnableSyntheticHandGameObject();

            // Stop previous tween.
            _tweenBase?.Stop();
            
            // Start new tween -- two different tweens to save bool checks on every property.
            if (fadeIn)
                _tweenBase = Tween.Value(
                    // startValue: fadeIn ? 0 : material.GetFloat(shaderPropertyToModifyOutline),
                    startValue: 0f,
                    endValue: 1f,
                    valueUpdatedCallback: ApplyFade,
                    duration: tweenConfigFadeIn.Duration,
                    delay: tweenConfigFadeIn.Delay,
                    easeCurve: tweenConfigFadeIn.AnimationCurve,
                    loop: tweenConfigFadeIn.loopType,
                    obeyTimescale: tweenConfigFadeIn.obeyTimescale
                );
            else
                _tweenBase = Tween.Value(
                    // startValue: fadeIn ? 0 : material.GetFloat(shaderPropertyToModifyOutline),
                    startValue: 1f,
                    endValue: 0f,
                    valueUpdatedCallback: ApplyFade,
                    duration: tweenConfigFadeOut.Duration,
                    delay: tweenConfigFadeOut.Delay,
                    easeCurve: tweenConfigFadeOut.AnimationCurve,
                    loop: tweenConfigFadeOut.loopType,
                    obeyTimescale: tweenConfigFadeOut.obeyTimescale
                );
            
            // Stop previous callback if there is one
            if (PreviousDelayedCallback != null)
                StopCoroutine(PreviousDelayedCallback);

            // Start callback delayed, if we are fading out.
            if(!fadeIn)
                StartCallbackDelayedIfGiven(DisableSyntheticHandGameObject, tweenConfigFadeIn.Delay + tweenConfigFadeIn.Duration);
        }
        
        /// <summary>
        /// Applies the given new fraction to the different shader properties.
        /// </summary>
        private void ApplyFade(float newFraction)
        {
            _material.SetFloat(shaderPropertyToModifyCore, maxValueCore * newFraction);
            _material.SetFloat(shaderPropertyToModifyOutline, maxValueOutline * newFraction);
        }

        private void EnableSyntheticHandGameObject()
        {
            syntheticHand.SetActive(true);
            if (debugging)
                Debug.Log("Setting synthetic Hand GO to ACTIVE.".StartWithFrom(GetType()), this);
        }

        private void DisableSyntheticHandGameObject()
        {
            syntheticHand.SetActive(false);
            if (debugging)
                Debug.Log("Setting synthetic Hand GO to INACTIVE.".StartWithFrom(GetType()), this);
        }
    }
}