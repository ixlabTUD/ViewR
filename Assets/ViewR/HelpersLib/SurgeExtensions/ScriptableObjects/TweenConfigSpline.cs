using SurgeExtensions;
using UnityEngine;

namespace SurgeExtensions.ScriptableObjects
{
    /// <summary>
    /// SO to config movement with Surge.
    /// </summary>
    [CreateAssetMenu(fileName = "TweenConfigSpline-", menuName = "TweenConfig/TweenConfigSpline", order = 0)]
    public class TweenConfigSpline : TweenConfig
    {
        [Header("Tween Config Spline")] 
        public bool faceDirection; 
        public float startValue = 0f; 
        public float endValue = 1f;
    }
}