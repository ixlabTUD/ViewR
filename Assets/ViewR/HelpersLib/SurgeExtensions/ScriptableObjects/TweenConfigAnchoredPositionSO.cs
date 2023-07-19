using System;
using Pixelplacement;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;

namespace SurgeExtensions.ScriptableObjects
{
    /// <summary>
    /// SO to config anchored positions with Surge
    /// </summary>
    [CreateAssetMenu(fileName = "TweenConfigAnchoredPosition-", menuName = "TweenConfig/TweenConfigMove/TweenConfigAnchoredPosition", order = 0)]
    public class TweenConfigAnchoredPositionSO : TweenConfigMoveSO
    {
        [Header("AnchoredPosition Settings")] 
        [ReadOnly] public Tween.TweenType tweenType = Tween.TweenType.AnchoredPosition;

        /// <summary>
        /// Calculates the start position based on the <see cref="goalPos"/>, <see cref="movementOrientation"/>, <see cref="movementDirection"/>, and <see cref="distance"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Vector2 CalculateStartPosition( Vector2 goalPos, bool invertDirection = false)
        {
            var orientation = movementOrientation2D switch
            {
                Orientation2D.Horizontal => Vector2.right,
                Orientation2D.Vertical => Vector2.up,
                Orientation2D.DiagonalNe => new Vector2(1, 1).normalized,
                Orientation2D.DiagonalSe => new Vector2(1, -1).normalized,
                Orientation2D.DiagonalSW => new Vector2(-1, -1).normalized,
                Orientation2D.DiagonalNw => new Vector2(-1, 1).normalized,
                _ => throw new ArgumentOutOfRangeException()
            };

            return movementDirection switch
            {
                Direction.Positive => GetGoalPlusDirection(goalPos, orientation * distance, !invertDirection),
                Direction.Negative => GetGoalPlusDirection(goalPos, orientation * distance, invertDirection),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Vector2 GetGoalPlusDirection(Vector2 goal, Vector2 direction, bool add)
        {
            return add ? (goal + direction) : (goal - direction);
        }
    }
}