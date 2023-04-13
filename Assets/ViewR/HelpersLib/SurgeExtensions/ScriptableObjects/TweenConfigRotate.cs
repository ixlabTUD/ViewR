using UnityEngine;

namespace SurgeExtensions.ScriptableObjects
{
    /// <summary>
    /// SO to config movement with Surge.
    /// Should use inheriting child instead! 
    /// </summary>
    [CreateAssetMenu(fileName = "TweenConfigRotate-", menuName = "TweenConfig/TweenConfigRotate", order = 0)]
    public class TweenConfigRotate : TweenConfig
    {
        [Header("Rotation specific:")]
        public float rotateAmount = 360;
        public Vector3 rotationPerAxis;
    }
}