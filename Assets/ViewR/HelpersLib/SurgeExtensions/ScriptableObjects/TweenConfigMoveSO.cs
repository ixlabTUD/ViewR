using UnityEngine;
using UnityEngine.Serialization;

namespace SurgeExtensions.ScriptableObjects
{
    /// <summary>
    /// SO to config movement with Surge.
    /// Should use inheriting child instead! i.e. <see cref="TweenConfigAnchoredPositionSO"/>, as they extend this ones functionality
    /// </summary>
    [CreateAssetMenu(fileName = "TweenConfigMove-", menuName = "TweenConfig/TweenConfigMove/TweenConfigMove", order = 0)]
    public class TweenConfigMoveSO : TweenConfig
    {
        [Header("Movement Settings")] 
        public bool useDistanceInsteadOfStartPos = true;
        [FormerlySerializedAs("movementOrientation")]
        public Orientation2D movementOrientation2D;
        public Direction movementDirection;
        public float distance = 1f;
    }
}