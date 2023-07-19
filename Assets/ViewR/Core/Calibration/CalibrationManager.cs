using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ViewR.Core.Calibration
{
    public class CalibrationManager : Singleton<CalibrationManager>
    {
        public Transform centerEye;
        public Transform transformToMove;
        public Transform leftHandAnchor;
        public Transform rightHandAnchor;
        public Transform leftWrist;
        public Transform rightWrist;
        public Transform leftControllerCalibrationPoint; 
        public Transform rightControllerCalibrationPoint;

        [HideInInspector] public Transform kabschAligner;
        [HideInInspector] public Transform twoPointAligner;


        public List<Transform> targetTransforms = new();
        public List<Transform> sourceTransforms = new();

        [HideInInspector] public List<CalibrationStation> HandCalibrationStations;

        [SerializeField] private float distanceBetweenHandTolerance = 0.015f;
        [SerializeField] private float handCalibrationConfigurationAngleTolerance = 2.0f;

        private bool _stationsInitialized;

        public Dictionary<CalibrationSocketOrientation, CalibrationStation> CalibrationStations { get; private set; }


        private void Start()
        {
            // Get refs
            InitializeStations();

            //Instantiate aligners
            var aligner = new GameObject();
            kabschAligner = Instantiate(aligner.transform, transformToMove);
            kabschAligner.name = "KabschAligner";
            twoPointAligner = Instantiate(aligner.transform, transformToMove);
            twoPointAligner.name = "TwoPointAligner";
        }

        public void RequestCalibrationCharged(List<Vector3> sourcesL, List<Vector3> sourcesR,
            List<Vector3> sourcesDirectionL, List<Vector3> sourcesDirectionR,
            CalibrationStation calibrationStation = null)
        {
            var controllerBasedOrientation = GetCurrentControllersOrientation();

            // Get matching CalibrationStation
            if (calibrationStation == null)
            {
                if (CalibrationStations.Count == 1)
                {
                    calibrationStation = CalibrationStations.Values.First();
                }
                else if (!TryGetMatchingCalibrationStation(controllerBasedOrientation, out calibrationStation))
                {
                    Debug.LogError(
                        $"Could not find a matching {nameof(CalibrationStation)}. The {nameof(CalibrationManager)} holds currently {CalibrationStations.Count} entries. Maybe you have none present?",
                        this);
                    return;
                }
            }

            // Move the right object
            Aligner.Aligner.DoAlignChargedOverTime(transformToMove, new[] { calibrationStation.TargetL.position },
                new[] { calibrationStation.TargetR.position }, sourcesL, sourcesR);
        }

        private CalibrationSocketOrientation GetCurrentControllersOrientation()
        {
            float rotL;
            float rotR;
            
            // if (OvrReferenceManager.Instance.LeftControllerAnchor.parent.forward.y is > -1 and < -0.5f)
            // {
            //      rotL = OvrReferenceManager.Instance.LeftControllerAnchor.parent.localEulerAngles.y;
            //      rotR = OvrReferenceManager.Instance.RightControllerAnchor.parent.localEulerAngles.y;
            // }
            // else
            // {
            //     // ToDo: actually calculate the controllers orientation
            //      rotL = OvrReferenceManager.Instance.LeftControllerAnchor.parent.localEulerAngles.z;
            //      rotR = OvrReferenceManager.Instance.RightControllerAnchor.parent.localEulerAngles.z;
            // }

            //TODO check if statement above
            rotL = leftHandAnchor.localEulerAngles.z;
            rotR = rightHandAnchor.localEulerAngles.z;

            var leftControllerControllerOrientation = GetControllerOrientation(rotL);
            var rightControllerControllerOrientation = GetControllerOrientation(rotR);

            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is (ControllerOrientation
                    .North, ControllerOrientation.North))
                return CalibrationSocketOrientation.NorthXNorth;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.North, ControllerOrientation.East))
                return CalibrationSocketOrientation.NorthXEast;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.North, ControllerOrientation.South))
                return CalibrationSocketOrientation.NorthXSouth;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.North, ControllerOrientation.West))
                return CalibrationSocketOrientation.NorthXWest;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.East, ControllerOrientation.North))
                return CalibrationSocketOrientation.EastXNorth;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.East, ControllerOrientation.East))
                return CalibrationSocketOrientation.EastXEast;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.East, ControllerOrientation.South))
                return CalibrationSocketOrientation.EastXSouth;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.East, ControllerOrientation.West))
                return CalibrationSocketOrientation.EastXWest;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.South, ControllerOrientation.North))
                return CalibrationSocketOrientation.SouthXNorth;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.South, ControllerOrientation.East))
                return CalibrationSocketOrientation.SouthXEast;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.South, ControllerOrientation.South))
                return CalibrationSocketOrientation.SouthXSouth;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.South, ControllerOrientation.West))
                return CalibrationSocketOrientation.SouthXWest;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.West, ControllerOrientation.North))
                return CalibrationSocketOrientation.WestXNorth;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.West, ControllerOrientation.East))
                return CalibrationSocketOrientation.WestXEast;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.West, ControllerOrientation.South))
                return CalibrationSocketOrientation.WestXSouth;
            if ((leftControllerControllerOrientation, rightControllerControllerOrientation) is
                (ControllerOrientation.West, ControllerOrientation.West))
                return CalibrationSocketOrientation.WestXWest;

            return CalibrationSocketOrientation.Undefined;
        }

        private ControllerOrientation GetControllerOrientation(float angle)
        {
            if (angle > 180) angle -= 360;

            if (Mathf.Abs(angle) <= 45) return ControllerOrientation.North;
            if (angle <= -45 && angle >= -135) return ControllerOrientation.East;
            if (Mathf.Abs(angle) >= 135) return ControllerOrientation.South;

            if (angle >= 45 && angle <= 135) return ControllerOrientation.West;

            return ControllerOrientation.Undefined;
        }


        // Hand calibration
        public HandCalibrationConfiguration getHandCalibrationConfiguration(Vector3 leftIndexMidPoint,
            Vector3 rightIndexMidPoint)
        {
            var AngleBetweenMidpoints = getAngleBetween(leftIndexMidPoint, rightIndexMidPoint);

            if (AngleBetweenMidpoints > 90f + handCalibrationConfigurationAngleTolerance)
                return HandCalibrationConfiguration.MiddleXUp;
            if (AngleBetweenMidpoints < 90f - handCalibrationConfigurationAngleTolerance)
                return HandCalibrationConfiguration.MiddleXDown;
            return HandCalibrationConfiguration.MiddleXMiddle;
        }


        private float getAngleBetween(Vector3 PositionA, Vector3 PositionB)
        {
            var relativePosition = PositionA - PositionB;
            var angle = Vector3.Angle(relativePosition, Vector3.up);

            return angle;
        }

        #region Keep Track of Stations

        private void InitializeStations(bool overwrite = false)
        {
            if (_stationsInitialized && !overwrite)
                return;

            CalibrationStations = new Dictionary<CalibrationSocketOrientation, CalibrationStation>();
            HandCalibrationStations = new List<CalibrationStation>();

            _stationsInitialized = true;
        }

        public void RegisterStation(CalibrationStation newCalibrationStation)
        {
            InitializeStations();

            if (newCalibrationStation.calibrationStationType == CalibrationStationType.Hand)
                HandCalibrationStations.Add(newCalibrationStation);
            else
                CalibrationStations.Add(newCalibrationStation.socketOrientation, newCalibrationStation);
        }

        public void UnregisterStation(CalibrationStation calibrationStation)
        {
            InitializeStations();

            if (calibrationStation.calibrationStationType == CalibrationStationType.Hand)
                HandCalibrationStations.Remove(calibrationStation);
            else
                CalibrationStations.Remove(calibrationStation.socketOrientation);
        }

        /// <summary>
        ///     Gets the <see cref="CalibrationStation" /> that matches the given <see cref="CalibrationSocketOrientation" />.
        /// </summary>
        /// <param name="socketOrientation">Search Parameter.</param>
        /// <param name="calibrationStation">Result.</param>
        /// <returns>Success.</returns>
        public bool TryGetMatchingCalibrationStation(CalibrationSocketOrientation socketOrientation,
            out CalibrationStation calibrationStation)
        {
            return CalibrationStations.TryGetValue(socketOrientation, out calibrationStation);
        }

        /// <summary>
        ///     Gets the <see cref="HandCalibrationStation" /> that matches the given
        ///     <see cref="Distancebetween and confirguration" />.
        /// </summary>
        /// <param name="DistanceBetweenPoints">Search Parameter.</param>
        /// <param name="handCalibrationConfiguration">Search Parameter.</param>
        /// <returns>CalibrationStation.</returns>
        public bool TryGetMatchingHandCalibrationStation(float DistanceBetweenPoints,
            HandCalibrationConfiguration handCalibrationConfiguration,
            out CalibrationStation calibrationStation)
        {
            float minDistance = 0;
            float maxDistance = 0;

            calibrationStation = null;

            for (var i = 0; i < HandCalibrationStations.Count; i++)
            {
                maxDistance = HandCalibrationStations[i].distanceBetweenCalibrationTargets +
                              distanceBetweenHandTolerance;
                minDistance = HandCalibrationStations[i].distanceBetweenCalibrationTargets -
                              distanceBetweenHandTolerance;

                if (minDistance <= DistanceBetweenPoints && DistanceBetweenPoints <= maxDistance &&
                    HandCalibrationStations[i].calibrationConfiguration == handCalibrationConfiguration)
                {
                    calibrationStation = HandCalibrationStations[i];
                    return true;
                }
            }

            return false;
        }


        public float GetMaxDistanceBetweenCalibTargets()
        {
            float maxDistance = 0;
            for (var i = 0; i < HandCalibrationStations.Count; i++)
                if (HandCalibrationStations[i].distanceBetweenCalibrationTargets > maxDistance)
                    maxDistance = HandCalibrationStations[i].distanceBetweenCalibrationTargets;
            return maxDistance;
        }

        public float GetMinDistanceBetweenCalibTargets()
        {
            if (HandCalibrationStations.Count <=0)
            {
                return 0;
            }
            
            var minDistance = HandCalibrationStations[0].distanceBetweenCalibrationTargets;
            for (var i = 0; i < HandCalibrationStations.Count; i++)
                if (HandCalibrationStations[i].distanceBetweenCalibrationTargets < minDistance)
                    minDistance = HandCalibrationStations[i].distanceBetweenCalibrationTargets;
            return minDistance;
        }

        #endregion
    }
}