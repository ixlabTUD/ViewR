using System;
using Oculus.Interaction;
using UnityEngine;
using ViewR.Managers;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing.FineTunedAlignment.Translational
{
    /// <summary>
    /// This is steered by <see cref="TranslationalAlignmentTuningManager"/>.
    /// </summary>
    [RequireComponent(typeof(TranslationalAlignmentTuningManager))]
    public class TranslationalAlignmentTuningRunner : MonoBehaviour
    {
        [SerializeField]
        private ControllerOffset controllerOffset;

        // References 
        private Transform _transformToMove;
        private TranslationalAlignmentTuningManager _translationalAlignmentTuningManager;

        // Initial values
        private Vector3 _initialPositionUserTransformToMove;
        private Quaternion _initialRotationTransformToMove;
        private Vector3 _previousPositionController;
        private Quaternion _initialRotationController;

        private Vector3 _initialOffsetPosition;

        private void Awake()
        {
            // Get refs
            _translationalAlignmentTuningManager = GetComponent<TranslationalAlignmentTuningManager>();
            _transformToMove = ReferenceManager.Instance.PlayerControllerTransform;
        }

        private void OnEnable()
        {
            // Get initial pose of object to move
            _initialPositionUserTransformToMove = _transformToMove.position;
            _initialRotationTransformToMove = _transformToMove.rotation;

            // Get initial controller pose
            Pose pose = default;
            controllerOffset.GetWorldPose(ref pose);
            _previousPositionController = pose.position;
            _initialRotationController = pose.rotation;

            _initialOffsetPosition = _initialPositionUserTransformToMove - _previousPositionController; // 1-3=-2
        }
        
        private void Update()
        {
            FineTune();
        }

        private void FineTune()
        {
            var newPositionDelta = controllerOffset.transform.position - _previousPositionController;

            switch (_translationalAlignmentTuningManager.currentDirectionToFix)
            {
                case TranslationalAlignmentTuningManager.DirectionToFix.HorizontalX:
                    var controllerForward = controllerOffset.transform.TransformDirection(Vector3.forward);
                    newPositionDelta =
                        new Vector3(newPositionDelta.x * Mathf.Abs(controllerForward.x),
                            newPositionDelta.y * Mathf.Abs(controllerForward.y),
                            newPositionDelta.z * Mathf.Abs(controllerForward.z));
                    break;
                case TranslationalAlignmentTuningManager.DirectionToFix.HorizontalZ:
                    var controllerRight = controllerOffset.transform.TransformDirection(Vector3.right);
                    newPositionDelta =
                        new Vector3(newPositionDelta.x * Mathf.Abs(controllerRight.x),
                            newPositionDelta.y * Mathf.Abs(controllerRight.y),
                            newPositionDelta.z * Mathf.Abs(controllerRight.z));
                    break;
                case TranslationalAlignmentTuningManager.DirectionToFix.Vertical:
                    var controllerUp = controllerOffset.transform.TransformDirection(Vector3.up);
                    newPositionDelta =
                        new Vector3(newPositionDelta.x * Mathf.Abs(controllerUp.x),
                            newPositionDelta.y * Mathf.Abs(controllerUp.y),
                            newPositionDelta.z * Mathf.Abs(controllerUp.z));
                    break;
                case TranslationalAlignmentTuningManager.DirectionToFix.Free:
                    // Dont manipulate position delta at all.
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _transformToMove.position -= newPositionDelta;

            _previousPositionController = controllerOffset.transform.position - newPositionDelta;
            
            // Fire event 
            AlignmentEvents.InvokeAlignmentRunning();
        }
    }
}