using System.Collections.Generic;
using UnityEngine;
using ViewR.HelpersLib.Extensions.AlignmentHelpers;

namespace ViewR.Core.Calibration.CalibrationData
{
    public class TransformHistory
    {
        public List<Vector3> Positions;
        public List<Quaternion> Rotations;

        public Vector3 GetAveragePosition()
        {
            return AlignmentHelpers.AveragePosition(Positions.ToArray());
        }

        public Quaternion GetAverageRotation()
        {
            return AlignmentHelpers.AverageQuaternion(Rotations.ToArray());
        }

        public void AddValues(Vector3 newPosition, Quaternion newRotation)
        {
            Positions.Add(newPosition);
            Rotations.Add(newRotation);
        }
    }
}