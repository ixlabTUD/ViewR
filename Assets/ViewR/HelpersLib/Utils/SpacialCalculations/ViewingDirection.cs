using UnityEngine;

namespace ViewR.HelpersLib.Utils.SpacialCalculations
{
    public static class ViewingDirection
    {
        /// <summary>
        /// Projects the <see cref="viewDirection"/> onto the XZ Plane (Normal: <see cref="Vector3.up"/>).
        /// </summary>
        public static Vector3 XZViewingDirection(Vector3 viewDirection)
        {
            return Vector3.ProjectOnPlane(viewDirection, Vector3.up);
        }
        
        /// <summary>
        /// Projects the <see cref="viewDirection"/> onto the XZ Plane (Normal: <see cref="Vector3.up"/>).
        /// </summary>
        public static Vector3 XZViewingDirectionNormalized(Vector3 viewDirection)
        {
            return XZViewingDirection(viewDirection).normalized;
        }
    }
}