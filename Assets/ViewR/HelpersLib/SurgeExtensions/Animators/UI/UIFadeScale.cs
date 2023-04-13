using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.Utils.ToggleObjects;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.UI
{
    public class UIFadeScale : UIAppearer
    {
        [Header("Surge Tween")] 
        [SerializeField, Tooltip("Controls both, fading and scaling.")] 
        private TweenConfig tweenConfig;
        [Header("Refs")]
        [SerializeField, Tooltip("Can be null.")] private CanvasGroup canvasGroup;
        [SerializeField, Tooltip("Can be null.")] private Transform transformToScale;
        [Header("Optional")]
        [Help("If there is no object set, it will not be toggled on Appear/Close.")]
        [SerializeField] private GameObject objectToDeactivate;
        [SerializeField] private ObjectsToToggle objectsToToggle;
        
        private TweenBase _tweenBaseFade;
        private TweenBase _tweenBaseScale;

        private Vector3 _initialScale;
        private bool _initialized;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize(bool overwriteInitialized = false)
        {
            if (_initialized && !overwriteInitialized)
                return;

            if (transformToScale)
                _initialScale = transformToScale.localScale;

            _initialized = true;
        }

        public override void Appear(bool appear, bool invertDirection = false, bool startFromCurrentValue = true,
            Action callback = null)
        {
            Initialize();

            base.Appear(appear, invertDirection, startFromCurrentValue, callback);
            
            if(appear)
            {
                if(transformToScale) 
                    transformToScale.gameObject.SetActive(true);
                if(objectToDeactivate)
                    objectToDeactivate.SetActive(true);
                
                objectsToToggle.Enable(true);
            }

            var wasRunning = (_tweenBaseFade?.Status == Tween.TweenStatus.Running ||
                              _tweenBaseScale?.Status == Tween.TweenStatus.Running);
            
            // Stop current tweens
            _tweenBaseFade?.Stop();
            _tweenBaseScale?.Stop();
            
            
            // Start tweens
            if(canvasGroup)
                _tweenBaseFade = Tween.CanvasGroupAlpha(canvasGroup,
                    startValue: (wasRunning || startFromCurrentValue) ? canvasGroup.alpha : appear ? 0 : 1, 
                    endValue: appear ? 1 : 0,
                    tweenConfig.Duration,
                    tweenConfig.Delay,
                    tweenConfig.AnimationCurve,
                    tweenConfig.loopType,
                    completeCallback: () =>
                    {
                        if (!appear)
                        {
                            // Catch in case there is nothing to scale.
                            if (!transformToScale && objectToDeactivate)
                                objectToDeactivate.SetActive(false);
                            if (!transformToScale)
                                objectsToToggle.Enable(false);
                        }
                    });

            if (transformToScale)
                _tweenBaseScale = Tween.LocalScale(transformToScale,
                    startValue: (wasRunning || startFromCurrentValue) ? transformToScale.localScale : appear ? Vector3.zero : _initialScale,
                    endValue: appear ? _initialScale : Vector3.zero,
                    tweenConfig.Duration,
                    tweenConfig.Delay,
                    tweenConfig.AnimationCurve,
                    tweenConfig.loopType,
                    completeCallback: () =>
                    {
                        if (!appear)
                        {
                            if (objectToDeactivate)
                                objectToDeactivate.SetActive(false);
                            objectsToToggle.Enable(false);
                        }
                    });
            
            if(previousDelayedCallback!= null)
                StopCoroutine(previousDelayedCallback);
            
            // Start callback delayed, if given.
            if(callback != null)
            {
                // Get max value
                var maxDuration = tweenConfig.Duration + tweenConfig.Delay;

                previousDelayedCallback = StartCoroutine(StartCallbackAfterSeconds(maxDuration, callback));
            }
        }
    }
}