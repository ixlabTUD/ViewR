using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions.ScriptableObjects;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;
using ViewR.HelpersLib.Extensions.General;
using ViewR.HelpersLib.SurgeExtensions.Animators.Movers.Parents;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.Movers
{
    /// <summary>
    /// Moves the object stepwise around.
    /// Best for infrequent movements.
    /// Use <see cref="overwriteTargetTransformTarget"/> to overwrite the scaled transform.
    /// </summary>
    public class ObjectStepwiseMover : Mover
    {
        [Header("Setup")]
        [SerializeField, Tooltip("Optional. If given, this transform will be used instead the components GameObject.")]
        private Transform overwriteTargetTransformTarget;

        [Header("Tween")]
        [SerializeField]
        private TweenConfigPositionSO tweenConfig;

        [Header("Debugging")]
        [SerializeField]
        private Movement moveUp;

        [SerializeField]
        private Movement moveRight;

        [SerializeField]
        private Movement moveForward;

        [SerializeField, ReadOnly]
        private Vector3 _target;


        private bool _initialized;
        private Vector3 _initialPosition;
        private TweenBase _tweenBase;
        private Vector3 _goalPos;
        private Vector3 _animStartPos;

        private void Awake()
        {
            Initialize();
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
                _initialPosition = overwriteTargetTransformTarget.localPosition;
                _target = overwriteTargetTransformTarget.localPosition;

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
        public void MoveUp() => MoveNext(moveUp);

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void MoveRight() => MoveNext(moveRight);

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void MoveForward() => MoveNext(moveForward);

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void MoveUpInverse() => MoveNext(moveUp, true);

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void MoveRightInverse() => MoveNext(moveRight, true);

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void MoveForwardInverse() => MoveNext(moveForward, true);

        /// <summary>
        /// Returns the transform to its <see cref="_initialPosition"/>.
        /// </summary>
        /// <param name="callback"></param>
        public override void Restore(Action callback = null)
        {
            // Ensure we initialized first.
            if (!_initialized)
                Initialize();
            if (!_initialized)
            {
                Debug.LogError("Still not initialized. Bailing.".StartWithFrom(GetType()), this);
                return;
            }

            // Stop current animations
            _tweenBase?.Stop();

            // Start a new one
            _tweenBase = Tween.LocalPosition(target: overwriteTargetTransformTarget,
                startValue: overwriteTargetTransformTarget.localPosition,
                endValue: _initialPosition,
                duration: tweenConfig.Duration,
                delay: tweenConfig.Delay,
                easeCurve: tweenConfig.AnimationCurve,
                loop: tweenConfig.loopType,
                obeyTimescale: tweenConfig.obeyTimescale);

            // Stop previous callback if there is one
            if (previousDelayedCallback != null)
                StopCoroutine(previousDelayedCallback);

            // Start callback delayed, if given.
            StartCallbackDelayedIfGiven(callback, tweenConfig);
        }

        public void MoveNext(Movement movement, bool invertDirection = false, Action callback = null)
        {
            // Ensure we initialized first.
            if (!_initialized)
                Initialize();
            if (!_initialized)
            {
                Debug.LogError("Still not initialized. Bailing.".StartWithFrom(GetType()), this);
                return;
            }

            var wasRunning = _tweenBase?.Status == Tween.TweenStatus.Running;

            // Stop current animations
            _tweenBase?.Stop();

            var target = CalculateNextPosition(movement, invertDirection);

            _animStartPos = tweenConfig.CalculateStartPosition(_goalPos, invertDirection);

            // Start a new one
            _tweenBase = Tween.LocalPosition(target: overwriteTargetTransformTarget,
                startValue: overwriteTargetTransformTarget.localPosition,
                endValue: target,
                duration: tweenConfig.Duration,
                delay: tweenConfig.Delay,
                easeCurve: tweenConfig.AnimationCurve,
                loop: tweenConfig.loopType,
                obeyTimescale: tweenConfig.obeyTimescale);

            // Stop previous callback if there is one
            if (previousDelayedCallback != null)
                StopCoroutine(previousDelayedCallback);

            // Start callback delayed, if given.
            StartCallbackDelayedIfGiven(callback, tweenConfig);
        }


        /// <summary>
        /// Calculates the next position and adds it to the <see cref="_target"/> variable.
        /// Results in a grid-like movement.
        /// </summary>
        public Vector3 CalculateNextPosition(Movement movement, bool invertDirection = false)
        {
            var directionFactor = invertDirection ? -1 : 1;

            _target += overwriteTargetTransformTarget.up * movement.moveUpStepSize * directionFactor;
            _target += overwriteTargetTransformTarget.right * movement.moveRightStepSize * directionFactor;
            _target += overwriteTargetTransformTarget.forward * movement.moveForwardStepSize * directionFactor;

            return _target;
        }

        [System.Serializable]
        public class Movement
        {
            public float moveUpStepSize;
            public float moveRightStepSize;
            public float moveForwardStepSize;
        }
    }
}