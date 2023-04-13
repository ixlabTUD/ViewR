using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions;
using UnityEngine;

namespace ViewR.Tools.ElevatorEffect
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField]
        private Transform elevatorFloor;

        public float assumedHeight = 1.75f;

        [SerializeField,
         Tooltip(
             "Threshold to adjust the elevator position on \"enter elevator\". Useful for entering the elevator from other levels.")]
        private float thresholdHeadOffset = 2.5f;

        [SerializeField]
        private float thresholdHeadOffsetMin = 1.3f;


        [SerializeField]
        private TweenConfig tweenConfig;

        private float _headHeightOnEnter;
        private Transform _userHead;
        private bool _inElevator;
        private bool _currentlyTweening;
        private TweenBase _tweenBase;


        private void Update()
        {
            if (!_inElevator) return;

            if (_tweenBase is {Status: Tween.TweenStatus.Running})
            {
                // Stop if not running
                return;
            }

            // Adjust position of elevator to users head.
            var position = elevatorFloor.position;
            position = new Vector3(
                x: position.x,
                y: _userHead.position.y - _headHeightOnEnter,
                z: position.z);

            elevatorFloor.position = position;
        }


        public void EnterElevator()
        {
            _inElevator = true;

            ReactToUserHeadHeightOnEnter();
        }

        public void LeaveElevator()
        {
            _inElevator = false;

            _tweenBase?.Stop();
        }

        /// <summary>
        /// Evaluates the user head height on entering the collider.
        /// Important: Do not rely on collision hit point! The collider box is not just the center of the head, but a box around it.
        /// </summary>
        private void ReactToUserHeadHeightOnEnter()
        {
            var userHeadPosition = _userHead.position;
            var floorPosition = elevatorFloor.position;
            _headHeightOnEnter = userHeadPosition.y - floorPosition.y;

            var negativeHeadHeight = _headHeightOnEnter <= 0;
            var lessThanThreshold = Mathf.Abs(userHeadPosition.y - floorPosition.y) > thresholdHeadOffset;
            var greaterThanThreshold = Mathf.Abs(userHeadPosition.y - floorPosition.y) < thresholdHeadOffsetMin;

            // If we are not on the ground level, adjust the users "head height on enter" and reposition the elevator. , OR if head height is negative.
            if (lessThanThreshold || greaterThanThreshold || negativeHeadHeight)
            {
                // Adjust headHeightOnEnter
                _headHeightOnEnter = assumedHeight;
            }

            // Tween the floor up instead of sudden jump.
            if (lessThanThreshold || greaterThanThreshold || negativeHeadHeight)
            {
                var elevatorFloorPosition = elevatorFloor.position;

                _tweenBase = Tween.Position(target: elevatorFloor,
                    endValue: new Vector3(elevatorFloorPosition.x,
                        _userHead.position.y - assumedHeight,
                        elevatorFloorPosition.z),
                    duration: tweenConfig.Duration,
                    delay: tweenConfig.Delay,
                    easeCurve: tweenConfig.AnimationCurve,
                    loop: tweenConfig.loopType,
                    obeyTimescale: tweenConfig.obeyTimescale
                );
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.tag.Equals("OVRHead"))
                return;

            // Fetch head.
            _userHead = other.transform;

            EnterElevator();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.tag.Equals("OVRHead"))
                return;

            LeaveElevator();
        }
    }
}