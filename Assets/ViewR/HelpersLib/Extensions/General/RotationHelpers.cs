using UnityEngine;

namespace ViewR.HelpersLib.Extensions.General
{
    public static class RotationHelpers
    {
        /// <summary>
        /// Determine the signed angle between two vectors, with normal 'n'
        /// as the rotation axis.
        /// </summary>
        // Based on https://forum.unity.com/threads/need-vector3-angle-to-return-a-negtive-or-relative-value.51092/#post-324018
        public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
        {
            return Mathf.Atan2(
                Vector3.Dot(n, Vector3.Cross(v1, v2)),
                Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
        }

    }
}