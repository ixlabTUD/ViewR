using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions.ScriptableObjects;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.GameObjects
{
    /// <summary>
    /// Moves the object around.
    /// Best for infrequent movements.
    /// Use <see cref="overwriteTargetTransformTarget"/> to overwrite the scaled transform.
    /// </summary>
    public class ObjectAppearerMove : ObjectAppearer
    {
        [Header("Setup")]
        [SerializeField, Tooltip("We will overwrite the current goal pos value on awake.")] 
        private bool overwriteGoalPosOnAwake = true;
        [SerializeField, Tooltip("Only used if overwriteGoalPosOnAwake is set to false")] 
        private Vector3 goalPos;
        [SerializeField, Tooltip("Only used if tweenConfig.useDistanceInsteadOfStartPos is set to false")] 
        private Vector3 animStartPos;
        
        
        [Header("Tween")]
        [SerializeField]
        private TweenConfigPositionSO tweenConfig;

        [SerializeField, Tooltip("Optional. If given, this transform will be used instead the components GameObject.")]
        private Transform overwriteTargetTransformTarget;

        private bool _initialized;
        private Vector3 _initialPosition;
        private TweenBase _tweenBase;

        private void Awake()
        {
            Initialize();
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
                _initialPosition = overwriteTargetTransformTarget.localPosition;
                
                if (overwriteGoalPosOnAwake)
                    goalPos = overwriteTargetTransformTarget.localPosition;
                
                _initialized = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e, this);

                _initialized = false;
            }
        }
        
        
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void AppearInInverse() => Appear(true, true);
        
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void AppearOutInverse() => Appear(false, true);
        
        public override void Appear(bool appear, bool invertDirection = false, bool startFromCurrentValue = false, 
            Action callback = null)
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
            
            // Stop current animations
            _tweenBase?.Stop();
            
            if (tweenConfig.useDistanceInsteadOfStartPos)
                animStartPos = tweenConfig.CalculateStartPosition(goalPos, invertDirection);

            // Start a new one
            _tweenBase = Tween.LocalPosition(target: overwriteTargetTransformTarget,
                startValue: (tweenConfig.forceUseStartGoalPos || startFromCurrentValue)
                    ? (appear ? animStartPos : goalPos)
                    : overwriteTargetTransformTarget.localPosition,
                endValue: appear ? goalPos : animStartPos,
                duration: tweenConfig.Duration,
                delay: tweenConfig.Delay,
                easeCurve: tweenConfig.AnimationCurve,
                loop: tweenConfig.loopType,
                obeyTimescale: tweenConfig.obeyTimescale);

            // Stop previous callback if there is one
            if(previousDelayedCallback!= null)
                StopCoroutine(previousDelayedCallback);
            
            // Start callback delayed, if given.
            StartCallbackDelayedIfGiven(callback, tweenConfig);
        }
    }
}
