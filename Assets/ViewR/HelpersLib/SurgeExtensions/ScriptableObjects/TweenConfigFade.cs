using UnityEngine;

namespace SurgeExtensions.ScriptableObjects
{
    /// <summary>
    /// SO to config movement with Surge.
    /// Should use inheriting child instead! 
    /// </summary>
    [CreateAssetMenu(fileName = "TweenConfigFade-", menuName = "TweenConfig/TweenConfigFade/TweenConfigFade", order = 0)]
    public class TweenConfigFade : TweenConfig
    {
        // [Header("Movement Settings")] 
        // public bool useDistanceInsteadOfStartPos = true;
        // public Orientation movementOrientation;
        // public Direction movementDirection;
        // public float distance = 1f;
    }
}