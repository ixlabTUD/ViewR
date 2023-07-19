using System;
using UnityEngine;

namespace SurgeExtensions
{
    [ExecuteInEditMode]
    public class DisplayTweenCurve : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("This script does nothing but display the Curve settings.")]
        [SerializeField] private AnimationCurve animCurve;
        [SerializeField] private TweenAnimationCurve tweenAnimationCurve;
        
        private void OnValidate()
        {
            animCurve = TweenAnimationExtension.GetTweenAnimation(tweenAnimationCurve);
        }
#endif
    }
}