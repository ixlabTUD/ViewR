using System.Collections;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using ViewR.Core.OVR.UX.MultiUI;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;
using ViewR.HelpersLib.Extensions.General;
using ViewR.HelpersLib.Utils.SpacialCalculations;
using ViewR.Managers;

namespace ViewR.Core.UI.FloatingUI.Follower
{
    /// <summary>
    /// Follow a target delayed.
    /// </summary>
    [RequireComponent(typeof(RestoreDefaultTargetFollowerUponDisable))]
    public class TargetFollower : MonoBehaviour
    {
        [Help("Enabled by the ToggleFollower class.\nConfigured by TargetSetter class.")]
        [Header("References")]
        [ReadOnly]
        public Transform target;
        [ReadOnly]
        public Transform cameraTransform;
        [ReadOnly]
        public TargetSetter definingTargetSetter;
        [SerializeField, Optional, Tooltip("Optional. If given, we will call LookAtFollower.MakeMeFirstItem() on it.")]
        private LookAtFollower lookAtFollower;

        [Header("Settings")]
        public bool snapToPositionOnEnable;
        [ReadOnly]
        public bool useFixedHeight;
        [ReadOnly, FormerlySerializedAs("height")]
        public float headHeightOffset = 0.5f;
        [ReadOnly]
        public float headOffsetXZ = 0.5f;
        public float yOffsetIfPushedBackAndUsingFixedHeight = 0.085f;
        public float zOffsetIfPushedBackAndUsingFixedHeight = 0.085f;
        [FormerlySerializedAs("speed")]
        [SerializeField]
        private float defaultSpeed = 0.5f;
        [SerializeField]
        private float threshold = 0.01f;
        [SerializeField]
        private AnimationCurve animationCurve;
        [SerializeField]
        private AnimationCurve animationCurveNearCamera;
        [SerializeField]
        private AnimationCurve animationCurveBetweenUserAndCamera;
        [SerializeField]
        private AnimationCurve animationCurveBehindCamera;
        
        [Header("Events")]
        [SerializeField]
        private UnityEvent arrivedAtTargetForFirstTime;
        [SerializeField]
        private UnityEvent pinned;
        [SerializeField]
        private UnityEvent unpinned;
        
        [Header("MultiUI")]
        [SerializeField]
        private MultiUIManager multiUIManager;
        
        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        private float _fixDistanceTargetToCamera;
        private Transform _userHead;

        public int PosInSequence { get; private set; }

        private float _speed;
        private Coroutine _speedUpRoutine;

        #region Arrived at target for first time

        private bool _arrivedOnce;
        public bool ArrivedAtTargetFirstTime
        {
            get => _arrivedOnce;
            set
            {
                if(_arrivedOnce == value)
                    return;

                _arrivedOnce = value;

                if (value)
                {
                    arrivedAtTargetForFirstTime?.Invoke();
                    ArrivedAtTargetForFirstTime?.Invoke();
                }
            }
        }
        
        public delegate void PropertyChanged();
        public event PropertyChanged ArrivedAtTargetForFirstTime;

        private const float DistanceUntilArrivingAtTarget = 0.05f;

        #endregion

        public const int INITIAL_POSITION_VALUE = -999;


        private void Awake()
        {
            PosInSequence = INITIAL_POSITION_VALUE;
            _speed = defaultSpeed;
        }

        private void Start()
        {
            _fixDistanceTargetToCamera = (target.position - cameraTransform.position).magnitude;

            if (!_userHead)
            {
                _userHead = ReferenceManager.IsInstanceRegistered
                    ? ReferenceManager.Instance.GetMainCamera().transform
                    : Camera.main.transform;
            }
        }

        private void OnEnable()
        {
            // Restore default - just in case
            _speed = defaultSpeed;

            // Snap if requested.
            if (snapToPositionOnEnable)
            {
                if (!_userHead)
                {
                    _userHead = ReferenceManager.IsInstanceRegistered
                        ? ReferenceManager.Instance.GetMainCamera().transform
                        : Camera.main.transform;
                }
                
                var unmodifiedTargetPosition = target.position;
                var targetPosition = useFixedHeight
                    ? CalculateTargetPositionForFixedHeight()
                    : target.position;
                transform.position = targetPosition;
            }

            // Notify the system!
            if (multiUIManager)
                multiUIManager.FireWindowFollowingChanged(true, this);
            else
                Debug.LogWarning($"No {nameof(MultiUIManager)} configured.", this);
            
            // Invoke event
            unpinned?.Invoke();
        }

        private void OnDisable()
        {
            // Notify the system!
            if (multiUIManager)
                multiUIManager.FireWindowFollowingChanged(false, this);
            else
                Debug.LogWarning($"No {nameof(MultiUIManager)} configured.", this);
            
            PosInSequence = INITIAL_POSITION_VALUE;

            //! Note: Use a "RestoreDefaultTargetFollowerUponDisable" component to restore this!
            // ArrivedAtTargetFirstTime = false;
            ForceStopSpeedUpIfRunning();

            // Invoke event
            pinned?.Invoke();
        }

        private void OnDestroy()
        {
            // Ensure array is up to date
            definingTargetSetter.RequestRemove(this);
        }

        #region MultiUI movers

        public void PushMeForward(Vector3 amount)
        {
            // Bail if already "main" position
            if (PosInSequence <= 0) return;
            
            target.localPosition -= amount;
            PosInSequence--;
            StartSpeedUp();
        }
        public void PushMeBack(Vector3 amount)
        {
            target.localPosition += amount;
            PosInSequence ++;
            StartSpeedUp();
        }
        public void SetMeToMain()
        {
            target.localPosition = definingTargetSetter.DefaultTargetOffsetFromCamera;
            PosInSequence = 0;
            StartSpeedUp();

            if (lookAtFollower != null)
                lookAtFollower.MakeMeFirstItem();
        }

        /// <summary>
        /// Starts the speed-up.
        /// </summary>
        private void StartSpeedUp()
        {
            ForceStopSpeedUpIfRunning();
            
            _speedUpRoutine = StartCoroutine(SpeedUpForSeconds());
        }

        /// <summary>
        /// Stops the speed-up.
        /// </summary>
        private void ForceStopSpeedUpIfRunning(bool resetSpeed = false)
        {
            if(_speedUpRoutine != null)
                StopCoroutine(_speedUpRoutine);
            
            _speedUpRoutine = null;

            if (resetSpeed)
                _speed = defaultSpeed;
        }

        /// <summary>
        /// Increases the <see cref="_speed"/> for a short time to allow us to execute UI swaps quickly.
        /// </summary>
        private IEnumerator SpeedUpForSeconds(float seconds = 0.6f)
        {
            _speed = defaultSpeed * 3;
            yield return new WaitForSeconds(seconds);
            _speed = defaultSpeed;
        }

        #endregion

        #region Update -> mover

        private void Update()
        {
            var targetPosition = target.position;

            // Modify target if we are using fixed height.
            if (useFixedHeight)
            {
                targetPosition = CalculateTargetPositionForFixedHeight();
                
                // Push up if not in first position
                if (PosInSequence != 0)
                {
                    var additionalYOffset = yOffsetIfPushedBackAndUsingFixedHeight * PosInSequence;
                    targetPosition += Vector3.up * additionalYOffset;
                    var additionalZOffset = zOffsetIfPushedBackAndUsingFixedHeight * PosInSequence;
                    targetPosition += target.forward * additionalZOffset;
                }
            }
            
            // Move
            // Calculate values
            var transformPosition = transform.position;
            var distanceToTarget = (transformPosition - targetPosition).magnitude;

            if (distanceToTarget > threshold)
            {
                // Calculate values
                var distanceToCamera = (transformPosition - cameraTransform.position).magnitude;
                
                // Calculate bools
                var closeToCamera = distanceToTarget > threshold /*thresholdWhenCloseToCamera */ && distanceToCamera < _fixDistanceTargetToCamera;
                var behindTheCamera = distanceToTarget > distanceToCamera;
                var betweenUserAndCamera = distanceToCamera < _fixDistanceTargetToCamera && distanceToTarget < _fixDistanceTargetToCamera;
            
                // Calculate speed
                if (debugging) Debug.Log($"Distance: {distanceToTarget}".Green());
                var animationCurveValue = animationCurve.Evaluate(distanceToTarget);

                if (closeToCamera)
                    animationCurveValue += animationCurveNearCamera.Evaluate(distanceToCamera);
                if (betweenUserAndCamera)
                    animationCurveValue += animationCurveBetweenUserAndCamera.Evaluate(distanceToTarget);
                if (behindTheCamera)
                    animationCurveValue += animationCurveBehindCamera.Evaluate(distanceToTarget);

                // Move
                transform.position = Vector3.MoveTowards(transform.position,
                    targetPosition,
                    _speed * Time.deltaTime * animationCurveValue);
            }

            // Ensure we first check whether we already called "ArrivedAtTargetFirstTime". Then compare distances - then potentially set "ArrivedAtTargetFirstTime"
            if (!ArrivedAtTargetFirstTime && distanceToTarget < DistanceUntilArrivingAtTarget)
            {
                ArrivedAtTargetFirstTime = true;
            }
        }

        /// <summary>
        /// Calculates the target position if we rely on fixed height.
        /// </summary>
        /// <returns></returns>
        private Vector3 CalculateTargetPositionForFixedHeight()
        {
            var twoDimensionalViewingDirection = ViewingDirection.XZViewingDirectionNormalized(_userHead.forward);

            return _userHead.position + headOffsetXZ * twoDimensionalViewingDirection + headHeightOffset * Vector3.up;
        }

        #endregion

        /// <summary>
        /// Convenience
        /// </summary>
        private void OnValidate()
        {
            if (lookAtFollower == null)
                TryGetComponent(out lookAtFollower);
        }
        
        /// <summary>
        /// Debugging and visually pleasing
        /// </summary>
        private void OnDrawGizmos()
        {
            if (useFixedHeight)
            {
                var twoDimensionalViewingDirection = ViewingDirection.XZViewingDirectionNormalized(_userHead.forward);
                var drawTarget = _userHead.position + 4 * twoDimensionalViewingDirection;

                var targetPosition = _userHead.position +
                                     headOffsetXZ * twoDimensionalViewingDirection +
                                     headHeightOffset * Vector3.up;

                // Show a connection between vector and response.
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_userHead.position, drawTarget);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_userHead.position, targetPosition);

                // Now show the input position.
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_userHead.position, 0.05f);

                // And finally the resulting position.
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(drawTarget, 0.05f);
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(targetPosition, 0.05f);
            }
        }
    }
}