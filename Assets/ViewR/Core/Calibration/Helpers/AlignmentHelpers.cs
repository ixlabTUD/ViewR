using System.Collections.Generic;
using UnityEngine;

namespace ViewR.HelpersLib.Extensions.AlignmentHelpers
{
    public static class AlignmentHelpers
    {
        public delegate void NewFloat(float value);

        public delegate void NewVector3(Vector3 value);

        public static event NewFloat RotatedBy;
        public static event NewVector3 MovedBy;

        /// <summary>
        ///     Aligns the rotation of objects using their quaternions.
        /// </summary>
        public static void AlignRotationQuaternions(Transform parentObjectToMove, Quaternion[] sourceRotations,
            Quaternion[] targetRotations)
        {
            var avgSourceRotation = AverageQuaternion(sourceRotations);
            var avgTargetRotation = AverageQuaternion(targetRotations);
            // var avgSourceRotation = QuaternionHelpers.QuaternionAverageRecursivePowerOfTwo(new List<Quaternion>() {sourceRotations});
            // var avgTargetRotation = QuaternionHelpers.QuaternionAverageRecursivePowerOfTwo(new List<Quaternion>() {targetRotations});

            parentObjectToMove.rotation = avgSourceRotation *
                                          Quaternion.Inverse(Quaternion.Inverse(parentObjectToMove.rotation) *
                                                             avgTargetRotation);
        }
        
        public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
        {
            return Mathf.Atan2(
                Vector3.Dot(n, Vector3.Cross(v1, v2)),
                Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
        }
        
        public static Quaternion AverageQuaternion(Quaternion[] quaternions)
        {
            var quaternionAverage = Quaternion.identity ;
            var averageWeight = 1f / quaternions.Length ;
            
            for ( var i = 0; i < quaternions.Length; i ++ )
            {
                var q = quaternions [i] ;
                // based on https://forum.unity.com/members/lordofduct.66428/ 
                quaternionAverage *= Quaternion.Slerp ( Quaternion.identity, q, averageWeight ) ;
            }

            return quaternionAverage;
        }
        
        /// <summary>
        /// Get the average value of a list of quaternion
        /// </summary>
        /// <param name="quaternions">The list of quaternions to average. Count must be a power of 2 and at least 2</param>
        /// <returns>The average quaternion</returns>
        /// <remarks>Based on https://codereview.stackexchange.com/a/232394</remarks>
        public static Quaternion QuaternionAverageRecursivePowerOfTwo(List<Quaternion> quaternions)
        {
            if (quaternions.Count == 2)
                return Quaternion.Lerp(quaternions[0], quaternions[1], 0.5f);

            var quats1 = quaternions.GetRange(0, quaternions.Count / 2);
            var quats2 = quaternions.GetRange(quaternions.Count / 2, quaternions.Count / 2);

            return Quaternion.Lerp(QuaternionAverageRecursivePowerOfTwo(quats1), QuaternionAverageRecursivePowerOfTwo(quats2), 0.5f);
        }

        /// <summary>
        ///     Aligns the rotation of objects using their quaternions.
        /// </summary>
        public static void AlignRotations(Transform objectToMove,
            Vector3[] sourcePositionsL,
            Vector3[] sourcePositionsR,
            Vector3[] targetPositionsL,
            Vector3[] targetPositionsR)
        {
            // Set y to 0
            var sourceChildAdjustedL = AdjustHeightToZero(sourcePositionsL);
            var sourceChildAdjustedR = AdjustHeightToZero(sourcePositionsR);
            var targetChildAdjustedL = AdjustHeightToZero(targetPositionsL);
            var targetChildAdjustedR = AdjustHeightToZero(targetPositionsR);

            // Average each of them
            var sourceChildLAvg = AveragePosition(sourceChildAdjustedL);
            var sourceChildRAvg = AveragePosition(sourceChildAdjustedR);
            var targetChildLAvg = AveragePosition(targetChildAdjustedL);
            var targetChildRAvg = AveragePosition(targetChildAdjustedR);


            // TODO: Optimize for performance and allocation
            var avrTargetPosition = (targetChildLAvg + targetChildRAvg) / 2;

            var rotOffset = AngleSigned(sourceChildRAvg - sourceChildLAvg,
                targetChildRAvg - targetChildLAvg, Vector3.up);

            // Apply
            objectToMove.RotateAround(avrTargetPosition, Vector3.up, rotOffset);

            RotatedBy?.Invoke(rotOffset);
        }

        public static Vector3 SetY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
        /// <summary>
        ///     Aligns the rotation of objects using their quaternions.
        /// </summary>
        public static void AlignRotations(Transform objectToMove,
            Vector3 sourcePositionL,
            Vector3 sourcePositionR,
            Vector3 targetPositionL,
            Vector3 targetPositionR)
        {
            // Set y to 0
            var sourcePositionL_ = sourcePositionL.SetY(0);
            var sourcePositionR_ = sourcePositionR.SetY(0);
            var targetPositionL_ = targetPositionL.SetY(0);
            var targetPositionR_ = targetPositionR.SetY(0);

            // TODO: Optimize for performance and allocation
            var TargetPosition = (targetPositionL_ + targetPositionR_) / 2;

            var rotOffset = AngleSigned(sourcePositionR_ - sourcePositionL_,
                targetPositionR_ - targetPositionL_, Vector3.up);

            // Apply
            objectToMove.RotateAround(TargetPosition, Vector3.up, rotOffset);

            RotatedBy?.Invoke(rotOffset);
        }

        public static void AlignRotation(Transform objectToMove,
            Vector3 sourceDirectionL,
            Vector3 sourceDirectionR,
            Vector3 targetDirectionL,
            Vector3 targetDirectionR,
            Vector3 targetPosition)
        {
            var rotOffset1 = AngleSigned(sourceDirectionL, targetDirectionL, Vector3.up);
            var rotOffset2 = AngleSigned(sourceDirectionR, targetDirectionR, Vector3.up);

            var avrRotOffset = (rotOffset1 + rotOffset2) / 2.0f;

            // Apply
            objectToMove.RotateAround(targetPosition, Vector3.up, avrRotOffset);

            RotatedBy?.Invoke(avrRotOffset);
        }


        public static Vector3[] AdjustHeightToZero(Vector3[] source)
        {
            var sourceAdjusted = new Vector3[source.Length];
            for (var i = 0; i < source.Length; i++)
            {
                var sourceL = source[i];
                sourceAdjusted[i] = sourceL.SetY(0);
            }

            return sourceAdjusted;
        }

        public static void AlignPositions(Transform parentObjectToMove, Vector3[] sourcePositions,
            Vector3[] targetPositions)
        {
            var avrSourcePosition = AveragePosition(sourcePositions);
            var avgTargetPosition = AveragePosition(targetPositions);


            parentObjectToMove.position += avgTargetPosition - avrSourcePosition;

            MovedBy?.Invoke(avgTargetPosition - avrSourcePosition);
        }

        public static void AlignPosition(Transform parentObjectToMove, Vector3 sourcePosition,
            Vector3 targetPosition)
        {
            parentObjectToMove.position += targetPosition - sourcePosition;

            MovedBy?.Invoke(targetPosition - sourcePosition);
        }

        public static void ForceUp(Transform objectToMove)
        {
            objectToMove.eulerAngles = new Vector3(0, objectToMove.eulerAngles.y, 0);
        }


        //Find the line of intersection between two planes.
//The inputs are two game objects which represent the planes.
//The outputs are a point on the line and a vector which indicates it's direction.
        public static void planePlaneIntersection(out Vector3 linePoint, out Vector3 lineVec, Plane plane1,
            Plane plane2)
        {
            linePoint = Vector3.zero;
            lineVec = Vector3.zero;

            //Get the normals of the planes.
            var plane1Normal = plane1.normal;
            var plane2Normal = plane2.normal;

            //We can get the direction of the line of intersection of the two planes by calculating the
            //cross product of the normals of the two planes. Note that this is just a direction and the line
            //is not fixed in space yet.
            lineVec = Vector3.Cross(plane1Normal, plane2Normal);

            //Next is to calculate a point on the line to fix it's position. This is done by finding a vector from
            //the plane2 location, moving parallel to it's plane, and intersecting plane1. To prevent rounding
            //errors, this vector also has to be perpendicular to lineDirection. To get this vector, calculate
            //the cross product of the normal of plane2 and the lineDirection.      
            var ldir = Vector3.Cross(plane2Normal, lineVec);

            var numerator = Vector3.Dot(plane1Normal, ldir);

            //Prevent divide by zero.
            if (Mathf.Abs(numerator) > 0.000001f)
            {
                var plane1ToPlane2 = plane1.ClosestPointOnPlane(new Vector3()) -
                                     plane2.ClosestPointOnPlane(new Vector3());
                var t = Vector3.Dot(plane1Normal, plane1ToPlane2) / numerator;
                linePoint = plane2.ClosestPointOnPlane(new Vector3()) + t * ldir;
            }
        }

        //With absolute value
        public static bool Approximately(Vector3 me, Vector3 other, float allowedDifference)
        {
            var dx = me.x - other.x;
            if (Mathf.Abs(dx) > allowedDifference)
                return false;

            var dy = me.y - other.y;
            if (Mathf.Abs(dy) > allowedDifference)
                return false;

            var dz = me.z - other.z;

            return Mathf.Abs(dz) >= allowedDifference;
        }

        //With percentage i.e. between 0 and 1
        public static bool ApproximatelyPercentage(Vector3 me, Vector3 other, float percentage)
        {
            var dx = me.x - other.x;
            if (Mathf.Abs(dx) > me.x * percentage)
                return false;

            var dy = me.y - other.y;
            if (Mathf.Abs(dy) > me.y * percentage)
                return false;

            var dz = me.z - other.z;

            return Mathf.Abs(dz) >= me.z * percentage;
        }
        
        public static Vector3 AveragePosition(Vector3[] positions)
        {
            var positionAverage = Vector3.zero;

            for (var i = 0; i < positions.Length; i++)
                positionAverage += positions[i];

            return positionAverage / positions.Length;
        }
        
        public static float AverageValues(float[] values)
        {
            var valueAverage = 0f;

            for (var i = 0; i < values.Length; i++)
                valueAverage += values[i];

            return valueAverage / values.Length;
        }
    }
}