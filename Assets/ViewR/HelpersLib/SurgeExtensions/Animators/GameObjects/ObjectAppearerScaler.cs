using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions;
using UnityEngine;
using UnityEngine.Serialization;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.GameObjects
{
    /// <summary>
    /// Scales the object to show and hide it.
    /// Use <see cref="overwriteTargetTransformTarget"/> to overwrite the scaled transform.
    /// </summary>
    public class ObjectAppearerScaler : ObjectAppearer
    {
        [SerializeField]
        private TweenConfig tweenConfig;

        [SerializeField, Tooltip("Optional. If given, this transform will be used instead the components GameObject.")]
        private Transform overwriteTargetTransformTarget;
        [SerializeField, Tooltip("If set to true, appearIn will force the scale to start from zero. Else: start from current scale.")]
        private bool forceTweenInFromScaleZero;
        [FormerlySerializedAs("runTweenOnEnable")] 
        [SerializeField, Tooltip("If set to true, the tween will be start if enabled. Else: nothing.")]
        private bool appearInOnEnable;
        [SerializeField]
        private bool appearInOnStart;
        [SerializeField]
        private bool appearOutOnStart;
        [SerializeField, Tooltip("If set to true, the tween will be stopped if disabled. Else: continues.")]
        private bool stopTweenOnDisable;
        
        [Header("Optional")]
        [Help("If there is no object set, it will not be toggled on Appear/Close.")]
        [SerializeField] private GameObject objectToDeactivate;

        private bool _initialized;
        private Vector3 _initialScale;
        private TweenBase _tweenBase;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            if (appearInOnStart)
                Appear(true);
            if (appearOutOnStart)
                Appear(false);
        }

        private void OnEnable()
        {
            if (appearInOnEnable)
            {
                Appear(true);
            }
        }

        private void OnDisable()
        {
            if (stopTweenOnDisable)
            {
                // Stop current animations
                _tweenBase?.Stop();

                // Stop previous callback if there is one
                if(previousDelayedCallback!= null)
                    StopCoroutine(previousDelayedCallback);
            }
        }

        private void Initialize(bool overwrite = false)
        {
            // Bail if already initialized
            if(_initialized && !overwrite) return; 
            
            try
            {
                // if no overwriteTransformTarget given, use this instead.
                if (!overwriteTargetTransformTarget)
                    overwriteTargetTransformTarget = this.transform;
                
                // Get initial values.
                _initialScale = overwriteTargetTransformTarget.localScale;
                
                _initialized = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e, this);

                _initialized = false;
            }
        }
        
        public override void Appear(bool appear, bool invertDirection = false, bool startFromCurrentValue = false, Action callback = null)
        {
            // Ensure we initialized first.
            if (!_initialized)
                Initialize();
            if (!_initialized)
            {
                Debug.LogError("Still not initialized. Bailing.".StartWithFrom(GetType()),this);
                return;
            }
            
            base.Appear(appear, invertDirection, startFromCurrentValue, callback);
            
            if(appear)
            {
                if(overwriteTargetTransformTarget) 
                    overwriteTargetTransformTarget.gameObject.SetActive(true);
                if(objectToDeactivate)
                    objectToDeactivate.SetActive(true);
            }

            if (!overwriteTargetTransformTarget.gameObject.activeSelf)
            {
                // Bail.
                Debug.Log($"overwriteTargetTransformTarget: {nameof(overwriteTargetTransformTarget)} is already disabled. Bailing.");
                return;  
            }
            
            // Stop current animations
            _tweenBase?.Stop();


            var startValue =
                startFromCurrentValue ? overwriteTargetTransformTarget.localScale :
                    appear ? 
                        (forceTweenInFromScaleZero 
                            ? Vector3.zero
                            : overwriteTargetTransformTarget.localScale
                        )
                        : overwriteTargetTransformTarget.localScale;
            
            // Start a new one
            _tweenBase = Tween.LocalScale(target: overwriteTargetTransformTarget,
                startValue: startValue,
                endValue: appear? _initialScale : Vector3.zero,
                duration: tweenConfig.Duration,
                delay: tweenConfig.Delay,
                easeCurve: tweenConfig.AnimationCurve,
                loop: tweenConfig.loopType,
                completeCallback: () =>
                {
                    if (!appear)
                    {
                        overwriteTargetTransformTarget.gameObject.SetActive(false);
                        if (objectToDeactivate)
                            objectToDeactivate.SetActive(false);
                    }
                },
                obeyTimescale: tweenConfig.obeyTimescale);

            // Stop previous callback if there is one
            if(previousDelayedCallback!= null)
                StopCoroutine(previousDelayedCallback);
            
            // Start callback delayed, if given.
            StartCallbackDelayedIfGiven(callback, tweenConfig);
        }
    }
}
