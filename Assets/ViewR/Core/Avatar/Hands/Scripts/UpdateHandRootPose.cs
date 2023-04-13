using UnityEngine;
using ViewR.Managers;

namespace ViewR.Core.Avatar.Hands.Scripts
{
    /// <summary>
    ///  Sets the hand root of the networked hand. Called every update.
    /// </summary>
    public class UpdateHandRootPose : MonoBehaviour
    {
        [SerializeField]
        private OVRPlugin.Handedness handedness;
    
        private OVRHand _hand;

        private void Update()
        {
            if (!_hand)
            {
                // Get Hand
                _hand = handedness == OVRPlugin.Handedness.LeftHanded
                    ? OvrReferenceManager.Instance.LeftOvrHand
                    : OvrReferenceManager.Instance.RightOvrHand;
            }

            if (_hand)
            {
                Transform parentTransform = transform.parent;
                var handTransform = _hand.transform;
                parentTransform.position = handTransform.position;
                parentTransform.rotation = handTransform.rotation;
                parentTransform.Rotate(Vector3.up *180, Space.Self);
                parentTransform.localScale = handTransform.localScale;
            }
        }
    }
}

