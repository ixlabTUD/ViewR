using UnityEngine;
using UnityEngine.EventSystems;
using ViewR.HelpersLib.Extensions.General;
using ViewR.Managers;

namespace ViewR.Core.OVR.UX
{
    public class HandTrackingUI : MonoBehaviour
    {
        
        private OVRHand _lHand;
        private OVRHand LHand
        {
            get
            {
                if (_lHand == null)
                    _lHand = OvrReferenceManager.Instance.LeftOvrHand;
                return _lHand;
            }
        }
        private OVRHand _rHand;
        private OVRHand RHand
        {
            get
            {
                if (_rHand == null)
                    _rHand = OvrReferenceManager.Instance.RightOvrHand;
                return _rHand;
            }
        }

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;


        private OVRCameraRig _cameraRig;
        private OVRInputModule _inputModule;

        private void Start()
        {
            _cameraRig = FindObjectOfType<OVRCameraRig>();
            _inputModule = FindObjectOfType<OVRInputModule>();
        }

        private void Update()
        {
            SetActiveController(OVRInput.GetActiveController());

            // _inputModule.rayTransform = hand.PointerPose;
        }

        private void SetActiveController(OVRInput.Controller c)
        {
            Transform t;
            switch (c)
            {
                case OVRInput.Controller.LTouch:
                    t = _cameraRig.leftHandAnchor;
                    break;
                case OVRInput.Controller.RTouch:
                    t = _cameraRig.rightHandAnchor;
                    break;
                case OVRInput.Controller.Touch:
                    // If there is a user config && both controllers are present, set to main hand
                    if(UserConfig.Instance != null)
                        t = UserConfig.Instance.LeftHanded ? LHand.PointerPose : RHand.PointerPose;
                    // fallback
                    else
                        t = _cameraRig.rightHandAnchor;
                    break;
                case OVRInput.Controller.LHand:
                    t = LHand.PointerPose;
                    break;
                case OVRInput.Controller.RHand:
                    t = RHand.PointerPose;
                    break;
                case OVRInput.Controller.Hands:
                    var leftHandIsReliable = OvrReferenceManager.Instance.LeftOvrHand.IsTracked &&
                                             OvrReferenceManager.Instance.LeftOvrHand.HandConfidence ==
                                             OVRHand.TrackingConfidence.High;
                    var rightHandIsReliable = OvrReferenceManager.Instance.RightOvrHand.IsTracked &&
                                              OvrReferenceManager.Instance.RightOvrHand.HandConfidence ==
                                              OVRHand.TrackingConfidence.High;
                    var leftHandProperlyTracked = OvrReferenceManager.Instance.LeftOvrHand.IsPointerPoseValid;
                    var rightHandProperlyTracked = OvrReferenceManager.Instance.RightOvrHand.IsPointerPoseValid;

                    // If there is a user config && both hands are valid, set to main hand
                    if(UserConfig.Instance != null && 
                       (leftHandIsReliable && leftHandProperlyTracked) &&
                       (rightHandIsReliable && rightHandProperlyTracked))
                    {
                        // Set to main hand
                        t = UserConfig.Instance.LeftHanded ? LHand.PointerPose : RHand.PointerPose;
                    }
                    // else set it to the reliable hand.
                    else
                    {
                        if (rightHandIsReliable && rightHandProperlyTracked)
                            t = RHand.PointerPose;
                        else if (leftHandIsReliable && leftHandProperlyTracked)
                            t = LHand.PointerPose;
                        else
                            // fallback
                            t = _cameraRig.rightHandAnchor;
                    }

                    if (debugging)
                        Debug.Log($"Hand confidence: ".StartWithFrom(GetType())
                                  + $"LeftHand: Reliable: {leftHandIsReliable}; properly tracked: {leftHandProperlyTracked}\n"
                                      .Green()
                                  + $"RightHand: Reliable: {rightHandIsReliable}; properly tracked: {rightHandProperlyTracked}"
                                      .Green()
                            , this);

                    break;
                default:
                    t = _cameraRig.rightHandAnchor;
                    break;
            }

            _inputModule.rayTransform = t;

            if (debugging)
                Debug.Log($"Controller type:".StartWithFrom(GetType()) + $" {c}".Green(), this);
        }
    }
}