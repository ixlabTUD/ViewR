using System;
using Pixelplacement;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;

namespace SurgeExtensions.ScriptableObjects
{
    /// <summary>
    /// SO to config anchored positions with Surge
    /// </summary>
    [CreateAssetMenu(fileName = "TweenConfigPosition3D-", menuName = "TweenConfig/TweenConfigMove/3D TweenConfigPosition", order = 0)]
    public class TweenConfigPositionSO : TweenConfigMoveSO
    {
        [Header("Start Value Config")]
        public bool forceUseStartGoalPos;
        
        [Header("AnchoredPosition Settings")] 
        [ReadOnly] public Tween.TweenType tweenType = Tween.TweenType.Position;

        /// <summary>
        /// Calculates the start position based on the <see cref="goalPos"/>, <see cref="TweenConfigMoveSO.movementOrientation2D"/>, <see cref="TweenConfigMoveSO.movementDirection"/>, and <see cref="TweenConfigMoveSO.distance"/>.
        /// </summary>
        public Vector3 CalculateStartPosition( Vector3 goalPos, bool invertDirection = false)
        {
            var orientation = movementOrientation2D switch
            {
                Orientation2D.Horizontal => Vector3.right,
                Orientation2D.Vertical => Vector3.up,
                Orientation2D.DiagonalNe => new Vector3(1, 1, 0).normalized,
                Orientation2D.DiagonalSe => new Vector3(1, -1, 0).normalized,
                Orientation2D.DiagonalSW => new Vector3(-1, -1, 0).normalized,
                Orientation2D.DiagonalNw => new Vector3(-1, 1, 0).normalized,
                _ => throw new ArgumentOutOfRangeException()
            };

            return movementDirection switch
            {
                Direction.Positive => GetGoalPlusDirection(goalPos, orientation * distance, !invertDirection),
                Direction.Negative => GetGoalPlusDirection(goalPos, orientation * distance, invertDirection),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Vector3 GetGoalPlusDirection(Vector3 goal, Vector3 direction, bool add)
        {
            return add ? (goal + direction) : (goal - direction);
        }
    }
}