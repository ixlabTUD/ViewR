using System;
using System.Collections;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.General;
using ViewR.HelpersLib.SurgeExtensions.Animators.Movers.Parents;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.Movers
{
    public class ObjectRotator : Mover
    {
        [Header("Setup")]
        [SerializeField, Tooltip("Optional. If given, this transform will be used instead the components GameObject.")]
        private Transform overwriteTargetTransformTarget;
        [SerializeField]
        private float additionalDelayedCallbackTimeMoveIn;
        [SerializeField]
        private float additionalDelayedCallbackTimeRestore;

        [Header("Tween")]
        [SerializeField]
        private TweenConfigRotate tweenConfigMove;
        [SerializeField]
        private TweenConfigRotate tweenConfigRestore;

        [Header("Callbacks")]
        [SerializeField]
        private UnityEvent movingCompleted;
        [SerializeField]
        private UnityEvent restoreCompleted;

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;
        
        private bool _initialized;
        private Quaternion _initialLocalRotation;
        private TweenBase _tweenBase;
        private Coroutine _delayedCallbackInternal;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            if (triggerOnEnable)
                Move(true);
        }

        private void OnDisable()
        {
            if (restoreOnDisable)
                Move(false);
        }

        private void Initialize(bool overwrite = false)
        {
            // Bail if already initialized
            if (_initialized && !overwrite) return;

            try
            {
                // if no overwriteTransformTarget given, use this instead.
                if (!overwriteTargetTransformTarget)
                    overwriteTargetTransformTarget = this.transform;

                // Get initial values.
                _initialLocalRotation = overwriteTargetTransformTarget.localRotation;

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
        public void RotateMove() => Move(true);

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void RestoreLocalRotation() => Move(false);
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void RestoreLocalRotationIfTransformActive() => RestoreIfTransformActive();

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void StopTween()
        {
            // Stop current animations
            _tweenBase?.Stop();
            
            // Stop previous internal callback if there is one
            if (_delayedCallbackInternal != null)
                StopCoroutine(_delayedCallbackInternal);

            // Stop previous callback if there is one
            if (previousDelayedCallback != null)
                StopCoroutine(previousDelayedCallback);
        }


        /// <summary>
        /// Returns the transform to its <see cref="_initialLocalRotation"/>.
        /// </summary>
        /// <param name="callback"></param>
        public override void Restore(Action callback = null)
        {
            Move(false, callback: callback);
        }


        /// <summary>
        /// Returns the transform to its <see cref="_initialLocalRotation"/>.
        /// </summary>
        /// <param name="callback"></param>
        public void RestoreIfTransformActive(Action callback = null)
        {
            // Ensure we initialized first.
            if (!_initialized)
                Initialize();

            if (!overwriteTargetTransformTarget.gameObject.activeSelf)
            {
                // Bail
                return;
            }
            
            Move(false, callback: callback);
        }
        
        public override void Move(bool moveIn, bool invertDirection = false, Action callback = null)
        {
            // Ensure we initialized first.
            if (!_initialized)
                Initialize();
            if (!_initialized)
            {
                Debug.LogError("Still not initialized. Bailing.".StartWithFrom(GetType()), this);
                return;
            }

            var tweenToUse = moveIn ? tweenConfigMove : tweenConfigRestore;

            // Stop current animations
            _tweenBase?.Stop();
            
            // Stop previous internal callback if there is one
            if (_delayedCallbackInternal != null)
                StopCoroutine(_delayedCallbackInternal);
            
            // Start a new one
            _tweenBase = Tween.LocalRotation(overwriteTargetTransformTarget,
                moveIn ? tweenToUse.rotationPerAxis : _initialLocalRotation.eulerAngles,
                tweenToUse.Duration,
                tweenToUse.Delay,
                tweenToUse.AnimationCurve,
                tweenToUse.loopType,
                completeCallback: () =>
                {
                    // Bail if not active
                    if (!this.enabled || !this.gameObject.activeSelf || !overwriteTargetTransformTarget.gameObject.activeSelf)
                        return;
                    var buffer = moveIn ? additionalDelayedCallbackTimeMoveIn : additionalDelayedCallbackTimeRestore;
                    _delayedCallbackInternal = StartCoroutine(DelayedCallback(tweenToUse.Delay + tweenToUse.Duration + buffer, moveIn));
                },
                obeyTimescale: tweenToUse.obeyTimescale);

            // Stop previous callback if there is one
            if (previousDelayedCallback != null)
                StopCoroutine(previousDelayedCallback);

            // Start callback delayed, if given.
            StartCallbackDelayedIfGiven(callback, tweenConfigRestore);
            
        }

        private IEnumerator DelayedCallback(float waitTime, bool moveIn, Action callback = null)
        {
            yield return new WaitForSeconds(waitTime);
            if (moveIn)
                movingCompleted?.Invoke();
            else
                restoreCompleted?.Invoke();
        }
    }
}