using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using ViewR.HelpersLib.Extensions.General;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore.Utils
{
    /// <summary>
    /// Quickly toggles the controller-active state of the networked controllers to mirror those of the local controller/hands.
    /// Also repositions active (networked) controllers.
    /// </summary>
    public class SwitchHandsAndControllers : MonoBehaviour
    {
        [FormerlySerializedAs("networkedControllerL")]
        [SerializeField]
        private GameObject networkedControllerParentL;
        [FormerlySerializedAs("networkedControllerR")]
        [SerializeField]
        private GameObject networkedControllerParentR;
        [FormerlySerializedAs("networkedHandL")] [SerializeField]
        private GameObject networkedMeshHandL;
        [FormerlySerializedAs("networkedHandR")] [SerializeField]
        private GameObject networkedMeshHandR;

        [SerializeField] public GameObject leftHand;
        [SerializeField] public GameObject rightHand;


        private OVRHand _localOVRHandL;
        private OVRHand _localOVRHandR;
        private GameObject _localControllerParentL;
        private GameObject _localControllerParentR;

        private List<ControllerPair> _controllerPairs = new List<ControllerPair>();

        private void Awake()
        {
            // Get refs
            _localOVRHandL = OvrReferenceManager.Instance.LeftOvrHand;
            _localOVRHandR = OvrReferenceManager.Instance.RightOvrHand;
            _localControllerParentL = ReferenceManager.Instance.OvrControllerPrefabLeft;
            _localControllerParentR = ReferenceManager.Instance.OvrControllerPrefabRight;
            
            // Cache
            CacheBothHands();
        }

        private void CacheBothHands()
        {
            CacheHandPairs(networkedControllerParentL, _localControllerParentL, Handedness.Left);
            CacheHandPairs(networkedControllerParentR, _localControllerParentR, Handedness.Right);
        }

        private void CacheHandPairs(GameObject networkedControllerParent, GameObject localControllerParent, Handedness handedness)
        {
            foreach (Transform networkedControllerForDevice in networkedControllerParent.transform)
            {
                var localControllerForDevice = localControllerParent.transform.Find(networkedControllerForDevice.gameObject.name).gameObject;
                
                _controllerPairs.Add(new ControllerPair(localControllerForDevice, networkedControllerForDevice.gameObject, handedness));
            }
        }

        private void Update()
        {
            if(_controllerPairs.Count <= 0)
            {
                Debug.LogWarning("The list of ControllerPairs is empty. Caching hands again!".StartWithFrom(GetType()), this);
                CacheBothHands();
                return;
            }
            
            // Show / hide hands
            var confidenceL = _localOVRHandL.IsDataHighConfidence;
            var confidenceR = _localOVRHandR.IsDataHighConfidence;
            if(networkedMeshHandL.activeSelf != confidenceL)
                networkedMeshHandL.SetActive(confidenceL); 
            if(networkedMeshHandR.activeSelf != confidenceR)
                networkedMeshHandR.SetActive(confidenceR); 

            // Show / hide controller & Pose Update
            foreach (var controllerPair in _controllerPairs)
            {
                // Toggle active state
                if(controllerPair.localController.activeSelf != controllerPair.networkedController.activeSelf)
                    controllerPair.networkedController.SetActive(controllerPair.localController.activeSelf);
                
                // Reposition if active
                if (controllerPair.localController.activeSelf)
                {
                    if (controllerPair.handedness == Handedness.Left)
                    {
                        leftHand.transform.position = controllerPair.localController.transform.position;
                        leftHand.transform.rotation = controllerPair.localController.transform.rotation;
                        leftHand.transform.localScale = controllerPair.localController.transform.localScale;

                    }
                    else
                    {
                        rightHand.transform.position = controllerPair.localController.transform.position;
                        rightHand.transform.rotation = controllerPair.localController.transform.rotation;
                        rightHand.transform.localScale = controllerPair.localController.transform.localScale;
                    }
                    
                    //OLD POSE SET WITHOUT PARENT HAND
                    // controllerPair.networkedController.transform.position = controllerPair.localController.transform.position;
                    // controllerPair.networkedController.transform.rotation = controllerPair.localController.transform.rotation;
                    // controllerPair.networkedController.transform.localScale = controllerPair.localController.transform.localScale;
                   
                }
            }
        }

        /// <summary>
        /// A class containing reference to the local and the networked controller.
        /// </summary>
        private class ControllerPair
        {
            public GameObject localController;
            public GameObject networkedController;

            public Handedness handedness;
            

            public ControllerPair(GameObject localController, GameObject networkedController, Handedness handedness)
            {
                this.localController = localController;
                this.networkedController = networkedController;
                this.handedness = handedness;
            }
        }

        private enum Handedness
        {
            Left,
            Right
        }
    
    }
}
