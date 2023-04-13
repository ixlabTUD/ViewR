using System;
using Oculus.Interaction.Input;
using UnityEngine;
using ViewR.HelpersLib.Utils.ToggleObjects;
using ViewR.Managers;

namespace ViewR.Core.OVR.Hands
{
    /// <summary>
    /// A class to disable the <see cref="SkinnedMeshRenderer"/> on the OVR Hand visual, given its confidence is not high (<see cref="OVRHand.IsDataHighConfidence"/>.). 
    /// </summary>
    /// <remarks>
    /// Workaround. Currently, if the hand confidence drops or the hands become not-trackable, OVR leaves them where they are. We might be able to solve this through their system as well, but a first search did not reveal a good access point to do so.
    /// </remarks>
    public class DisableOnLowHandConfidence : MonoBehaviour
    {    
        [SerializeField]
        private SkinnedMeshRenderer handMesh;
        [SerializeField]
        private Handedness handedness;

        [Header("Optional")]
        [SerializeField] 
        private ObjectsToToggle objectsToToggle;

        private OVRHand _localOvrHand;
        private bool _currentStatus;
        
        private void Start()
        {
            _localOvrHand = handedness == Handedness.Left
                ? OvrReferenceManager.Instance.LeftOvrHand
                : OvrReferenceManager.Instance.RightOvrHand;
            _currentStatus = _localOvrHand.IsDataHighConfidence;
        }

        private void Update()
        {
            // Show / hide hands
            var highConfidence = _localOvrHand.IsDataHighConfidence;
            
            if(handMesh.enabled != highConfidence || _currentStatus != highConfidence)
            {
                handMesh.enabled = highConfidence;

                // And Toggle additional optional objects
                objectsToToggle.Enable(highConfidence);
            }

            _currentStatus = _localOvrHand.IsDataHighConfidence;
        }
        
        // Convenience
        private void OnValidate()
        {
            if (!handMesh)
                TryGetComponent(out handMesh);
        }
    }
}
