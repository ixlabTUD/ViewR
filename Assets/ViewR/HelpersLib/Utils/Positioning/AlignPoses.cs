using UnityEngine;

namespace ViewR.HelpersLib.Utils.Positioning
{
    /// <summary>
    /// Syncs two transforms.
    /// </summary>
    public class AlignPoses : MonoBehaviour
    {
        [Header("Behaviour")]
        public bool alignPosition = true;
        public bool alignRotation = false;
        public bool alignScale = false;
        public bool useLocalPose = false;

        [Header("Offset")]
        public bool usePositionalOffset = false;
        public Vector3 worldPositionOffset;
        public Vector3 localPositionOffset;

        [Header("References")]
        public Transform sourceTransform;
        public Transform targetTransform;
        

        private void Update()
        {
            if (alignPosition)
            {
                if (useLocalPose)
                    targetTransform.localPosition = sourceTransform.localPosition;
                else
                    targetTransform.position = sourceTransform.position;

                if (usePositionalOffset)
                {
                    targetTransform.position += worldPositionOffset;
                    targetTransform.localPosition += localPositionOffset;
                }
            }
            
                
            if (alignRotation)
                if (useLocalPose)
                    targetTransform.localRotation = sourceTransform.localRotation;
                else
                    targetTransform.rotation = sourceTransform.rotation;
            
            // We will only use local scale.
            if (alignScale)
                targetTransform.localScale = sourceTransform.localScale;
        }
    }
}