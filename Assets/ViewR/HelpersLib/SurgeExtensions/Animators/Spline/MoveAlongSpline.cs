using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
#endif
using SurgeExtensions.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.Spline
{
    public class MoveAlongSpline : MonoBehaviour
    {
        [Header("References")] 
        public Pixelplacement.Spline spline;
        public Transform objectToMove;
        [Header("Config")]
        public TweenConfigSpline tweenConfigSpline;
        public bool startOnStart = true;

        [Header("Speed")] 
        [Tooltip("Uses the movement speed instead of the config duration.")]
        public bool useMovementSpeed = true;

        public float movementSpeed;
        
        public UnityEvent startedTween;
        public UnityEvent stoppedTween;

        public TweenBase TweenBase { get; private set; }

        public Tween.TweenStatus GetTweenBaseStatus() => TweenBase.Status;

        public Tween.TweenType GetTweenBaseType() => TweenBase.tweenType;
        
        
        private void Start()
        {
            if (startOnStart)
                StartTween();
        }
        
        /// <summary>
        /// Starts the <see cref="TweenBase"/> based on <see cref="tweenConfigSpline"/>.
        /// </summary>
#if ODIN_INSPECTOR
        [Button]
#else
        [ExposeMethodInEditor]
#endif
        [ContextMenu(nameof(StartTween))]
        public void StartTween()
        {
            if (useMovementSpeed)
            {
                // Ensure length is set
                spline.CalculateLength();
                tweenConfigSpline.Duration = spline.Length / movementSpeed;
            }

            // Stop if existing
            TweenBase?.Stop();

            // Start new Tween
            TweenBase = Tween.Spline(spline,
                objectToMove,
                TweenBase?.Percentage ?? tweenConfigSpline.startValue,
                tweenConfigSpline.endValue,
                tweenConfigSpline.faceDirection,
                tweenConfigSpline.Duration,
                tweenConfigSpline.Delay,
                tweenConfigSpline.AnimationCurve,
                tweenConfigSpline.loopType,
                tweenConfigSpline.startCallback,
                tweenConfigSpline.completeCallback,
                tweenConfigSpline.obeyTimescale);
            
            startedTween?.Invoke();
        }

        /// <summary>
        /// Stops/pauses the <see cref="TweenBase"/>.
        /// </summary>
#if ODIN_INSPECTOR
        [Button]
#else
        [ExposeMethodInEditor]
#endif
        [ContextMenu(nameof(StopTween))]
        public void StopTween()
        {
            TweenBase?.Stop();
            stoppedTween?.Invoke();
        }

        /// <summary>
        /// Resumes a stopped/paused <see cref="TweenBase"/>.
        /// Calls <see cref="StartTween"/> if none yet exists.
        /// </summary>
#if ODIN_INSPECTOR
        [Button]
#else
        [ExposeMethodInEditor]
#endif
        [ContextMenu(nameof(ResumeTween))]
        public void ResumeTween(bool recalculateLength = false)
        {
            // Ensure we also continue with our tween if we are to recalculate the length of the spline.
            if (TweenBase == null || recalculateLength)
                StartTween();
            else
            {
                TweenBase.Resume();
                startedTween?.Invoke();
            }
        }

        /// <summary>
        /// Toggles the <see cref="TweenBase"/> on or off, given its current state.
        /// </summary>
#if ODIN_INSPECTOR
        [Button]
#else
        [ExposeMethodInEditor]
#endif
        [ContextMenu(nameof(ToggleTween))]
        public void ToggleTween()
        {
            switch (TweenBase.Status)
            {
                case Tween.TweenStatus.Delayed:
                case Tween.TweenStatus.Running:
                    StopTween();
                    break;
                case Tween.TweenStatus.Canceled:
                case Tween.TweenStatus.Stopped:
                case Tween.TweenStatus.Finished:
                    ResumeTween();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
    }
}