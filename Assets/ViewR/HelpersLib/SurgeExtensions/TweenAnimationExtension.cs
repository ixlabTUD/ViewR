using System;
using Pixelplacement;
using UnityEngine;

namespace SurgeExtensions
{
    /// <summary>
    /// Curves available in SURGE
    /// Usage: use with <see cref="TweenAnimationExtension.GetTweenAnimation(TweenAnimationCurve)"/>
    /// </summary>
    public enum TweenAnimationCurve
    {
        EaseBounce,
        EaseIn,
        EaseLinear,
        EaseOut,
        EaseSpring,
        EaseWobble,
        EaseInBack,
        EaseInOut,
        EaseInStrong,
        EaseOutBack,
        EaseOutStrong,
        EaseInOutBack,
        EaseInOutStrong
    }

    /// <summary>
    /// Custom extension to SURGE - Tween
    /// Allows simple config via the editor.
    /// </summary>
    public static class TweenAnimationExtension
    {
        public static AnimationCurve GetTweenAnimation(TweenAnimationCurve curve)
        {
            return curve switch
            {
                TweenAnimationCurve.EaseBounce => Tween.EaseBounce,
                TweenAnimationCurve.EaseIn => Tween.EaseIn,
                TweenAnimationCurve.EaseLinear => Tween.EaseLinear,
                TweenAnimationCurve.EaseOut => Tween.EaseOut,
                TweenAnimationCurve.EaseSpring => Tween.EaseSpring,
                TweenAnimationCurve.EaseWobble => Tween.EaseWobble,
                TweenAnimationCurve.EaseInBack => Tween.EaseInBack,
                TweenAnimationCurve.EaseInOut => Tween.EaseInOut,
                TweenAnimationCurve.EaseInStrong => Tween.EaseInStrong,
                TweenAnimationCurve.EaseOutBack => Tween.EaseOutBack,
                TweenAnimationCurve.EaseOutStrong => Tween.EaseOutStrong,
                TweenAnimationCurve.EaseInOutBack => Tween.EaseInOutBack,
                TweenAnimationCurve.EaseInOutStrong => Tween.EaseInOutStrong,
                _ => throw new ArgumentOutOfRangeException(nameof(curve), curve, null)
            };
        }
        
    }
}