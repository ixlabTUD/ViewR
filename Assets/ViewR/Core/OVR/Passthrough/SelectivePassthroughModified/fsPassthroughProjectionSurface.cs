using UnityEngine;

namespace ViewR.Core.OVR.Passthrough.SelectivePassthroughModified
{
    /// <summary>
    /// Aligns the projection object with the hand or controller if button pressed or pinched.
    /// </summary>
    public class fsPassthroughProjectionSurface : MonoBehaviour
    {
        [SerializeField]
        private OVRPassthroughLayer passthroughLayerUserDefined;

        public MeshFilter projectionObject;
        
        public bool useHands;

        private MeshRenderer _quadOutline;
        private bool _surfaceDefined;
        private bool _pinching = false;

        private OVRSkeleton[] _skeletons;
        private OVRHand[] _hands;
        private int _handIndex = -1;

        private void Start()
        {
            // The MeshRenderer component renders the quad as a blue outline
            // we only use this when Passthrough isn't visible
            _quadOutline = projectionObject.GetComponent<MeshRenderer>();
            _quadOutline.enabled = true;

            // Init.
            _skeletons = new OVRSkeleton[2];
            _hands = new OVRHand[2];
        }

        private void Update()
        {
            var usingControllers =
                (OVRInput.GetActiveController() == OVRInput.Controller.RTouch ||
                 OVRInput.GetActiveController() == OVRInput.Controller.LTouch ||
                 OVRInput.GetActiveController() == OVRInput.Controller.Touch);

            if (!usingControllers)
            {
                FindHands();

                if (useHands && _handIndex >= 0)
                {
                    ControlWithHand(_hands[_handIndex], _skeletons[_handIndex]);
                }
            }
            else
            {
                // If using controllers:
                ControlWithControllers();
            }
        }

        /// <summary>
        /// Controller interaction logic
        /// </summary>
        private void ControlWithControllers()
        {
            // Hide object when A button is held, show it again when button is released, move it while held.
            if (OVRInput.GetDown(OVRInput.Button.Two))
                DisablePassthroughObject();

            if (OVRInput.Get(OVRInput.Button.Two))
                AlignObjectWithController();

            if (OVRInput.GetUp(OVRInput.Button.Two))
                EnablePassthroughObject();
        }


        /// <summary>
        /// Hand interaction logic
        /// Bails on the non dominant hand for now.
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="skeleton"></param>
        private void ControlWithHand(OVRHand hand, OVRSkeleton skeleton)
        {
            // Bail on non-dominant hand
            if(!hand.IsDominantHand)
                return;
            
            if (_pinching)
            {
                if (hand.GetFingerPinchStrength(OVRHand.HandFinger.Index) < 0.8f)
                {
                    _pinching = false;

                    EnablePassthroughObject();
                }
                else
                {
                    // Align object position
                    AlignObjectWithHand(skeleton);
                }
            }
            else
            {
                if (hand.GetFingerIsPinching(OVRHand.HandFinger.Index))
                {
                    if (_surfaceDefined)
                        DisablePassthroughObject();

                    _pinching = true;
                }
            }
        }

        private void AlignObjectWithController()
        {
            var controllingHand = OVRInput.Controller.RTouch;
            transform.position = OVRInput.GetLocalControllerPosition(controllingHand);
            transform.rotation = OVRInput.GetLocalControllerRotation(controllingHand);
        }

        private void DisablePassthroughObject()
        {
            passthroughLayerUserDefined.RemoveSurfaceGeometry(projectionObject.gameObject);
            _quadOutline.enabled = true;

            _surfaceDefined = false;
        }

        private void EnablePassthroughObject()
        {
            passthroughLayerUserDefined.AddSurfaceGeometry(projectionObject.gameObject);
            _quadOutline.enabled = false;

            _surfaceDefined = true;
        }

        private void AlignObjectWithHand(OVRSkeleton skeleton)
        {
            transform.position = skeleton.Bones[5].Transform.position;
            transform.rotation = skeleton.Bones[5].Transform.rotation;
            // transform.rotation = Quaternion.LookRotation(skeleton.Bones[5].Transform.position - skeleton.Bones[0].Transform.position);
        }


        private void FindHands()
        {
            if (_skeletons[0] == null || _skeletons[1] == null)
            {
                var foundSkeletons = FindObjectsOfType<OVRSkeleton>();
                if (foundSkeletons[0])
                {
                    _skeletons[0] = foundSkeletons[0];
                    _hands[0] = _skeletons[0].GetComponent<OVRHand>();
                    _handIndex = 0;
                }

                if (foundSkeletons[1])
                {
                    _skeletons[1] = foundSkeletons[1];
                    _hands[1] = _skeletons[1].GetComponent<OVRHand>();
                    _handIndex = 1;
                }
            }
            else
            {
                if (_handIndex == 0)
                {
                    if (_hands[1].GetFingerIsPinching(OVRHand.HandFinger.Index))
                    {
                        _handIndex = 1;
                    }
                }
                else
                {
                    if (_hands[0].GetFingerIsPinching(OVRHand.HandFinger.Index))
                    {
                        _handIndex = 0;
                    }
                }
            }
        }
    }
}