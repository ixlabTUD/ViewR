using System;
using System.Linq;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Serialization;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;
using Random = UnityEngine.Random;

// Contains SO classes and references to easily configure Tween-Effects with simple refrences.

namespace SurgeExtensions
{
    public enum Orientation2D
    {
        Horizontal,
        Vertical,
        DiagonalNe,
        DiagonalSe,
        DiagonalSW,
        DiagonalNw
    }
    public enum Orientation3D
    {
        HorizontalRight,
        HorizontalForward,
        VerticalUp
    }

    public enum Direction
    {
        Positive,
        Negative
    }

    [CreateAssetMenu(fileName = "TweenConfig-", menuName = "TweenConfig/TweenConfig", order = 0)]
    public class TweenConfig : ScriptableObject
    {
        [Header("Surge Tween")]
        [FormerlySerializedAs("duration")]
        [SerializeField]
        private float fixedDuration = 1f;
        [SerializeField]
        [FormerlySerializedAs("delay")] private float fixedDelay = 0;
        [SerializeField]
        private TweenAnimationCurve tweenAnimationCurve;
        [SerializeField]
        private bool useCustomAnimationCurveInstead;
        [SerializeField]
        private AnimationCurve customAnimationCurve;
        public Tween.LoopType loopType;
        [ReadOnly]
        public Tween.TweenStatus tweenStatus;
        public Action startCallback;
        public Action completeCallback;
        public bool obeyTimescale = true;

        [Header("Randomized behavior")]
        [SerializeField]
        private bool useRandomDuration;
        [SerializeField]
        private Vector2 minMaxDuration;
        [SerializeField]
        private bool useRandomDelay;
        [SerializeField]
        private Vector2 minMaxDelay;

        [Header("Probability curves")]
        [Help("These use the minMax values above to scale.")]
        [SerializeField]
        private bool useProbabilityCurveDuration;
        [SerializeField]
        private AnimationCurve probabilityCurveAnimationDuration = AnimationCurve.EaseInOut(0, 1, 1, 1);
        [SerializeField]
        private bool useProbabilityCurveDelay;
        [SerializeField]
        private AnimationCurve probabilityCurveAnimationDelay = AnimationCurve.EaseInOut(0, 1, 1, 1);

        [Header("Debugging")]
        [SerializeField]
        protected bool debugging;

        [SerializeField,
         Tooltip(
             "If this is true, we will overwrite the custom curve with the selected Tween to improve inspector-experience")]
        private bool overwriteCustomCurveIfItIsNotUsed = true;


        /// <summary>
        /// Returns the duration or random duration depending on the settings <see cref="useRandomDuration"/>
        /// </summary>
        public float Duration
        {
            get
            {
                if (useProbabilityCurveDuration)
                {
                    var x = Random.Range(probabilityCurveAnimationDuration.keys.First().time,
                        probabilityCurveAnimationDuration.keys.Last().time);

                    var y = probabilityCurveAnimationDuration.Evaluate(x);

                    float maxDistance;
                    float minValue;
                    if (minMaxDuration.x < minMaxDuration.y)
                    {
                        minValue = minMaxDuration.x;
                        maxDistance = -minMaxDuration.x + minMaxDuration.y;
                    }
                    else
                    {
                        minValue = minMaxDuration.x;
                        maxDistance = -minMaxDuration.y + minMaxDuration.x;
                    }

                    return minValue + y * maxDistance;
                }
                else
                    return useRandomDuration ? Random.Range(minMaxDuration.x, minMaxDuration.y) : fixedDuration;
            }
            set => fixedDuration = value;
        }

        public float RandomDuration => Random.Range(minMaxDuration.x, minMaxDuration.y);


        /// <summary>
        /// Returns the delay or random delay depending on the settings <see cref="useRandomDelay"/>
        /// </summary>
        public float Delay
        {
            get
            {
                if (useProbabilityCurveDelay)
                {
                    var x = Random.Range(probabilityCurveAnimationDelay.keys.First().time,
                        probabilityCurveAnimationDelay.keys.Last().time);

                    var y = probabilityCurveAnimationDelay.Evaluate(x);

                    float maxDistance;
                    float minValue;
                    if (minMaxDelay.x < minMaxDelay.y)
                    {
                        minValue = minMaxDelay.x;
                        maxDistance = -minMaxDelay.x + minMaxDelay.y;
                    }
                    else
                    {
                        minValue = minMaxDelay.x;
                        maxDistance = -minMaxDelay.y + minMaxDelay.x;
                    }

                    return minValue + y * maxDistance;
                }
                else
                    return useRandomDelay ? Random.Range(minMaxDelay.x, minMaxDelay.y) : fixedDelay;
            }
            set => fixedDelay = value;
        }

        public float RandomDelay => Random.Range(minMaxDelay.x, minMaxDelay.y);


        public AnimationCurve AnimationCurve =>
            useCustomAnimationCurveInstead
                ? customAnimationCurve
                : TweenAnimationExtension.GetTweenAnimation(tweenAnimationCurve);


        private void OnValidate()
        {
            // if we do not use our custom Curve: (FOR VISUAL INFO IN INSPECTOR ONLY) 
            if (overwriteCustomCurveIfItIsNotUsed && !useCustomAnimationCurveInstead)
                customAnimationCurve =
                    new AnimationCurve(TweenAnimationExtension.GetTweenAnimation(tweenAnimationCurve)?.keys);
        }
    }
}