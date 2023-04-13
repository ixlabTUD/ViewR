using UnityEngine;
using UnityEngine.Serialization;

namespace ViewR.HelpersLib.Utils.Positioning
{
    public class PositionBetween : MonoBehaviour
    {
        public Transform objectA;
        
        public Transform objectB;
        
        [FormerlySerializedAs("offset")]
        public Vector3 worldCoordinateOffset;

        public Vector3 localCoordinateOffset;

        public bool Active
        {
            get => this.enabled;
            set => this.enabled = value;
        }

        private void Update()
        {
            var objectAPosition = objectA.position;
            var localTransform = transform;
            localTransform.position = objectAPosition +  (objectB.position - objectAPosition) / 2 + worldCoordinateOffset + (localTransform = transform).TransformVector(localCoordinateOffset);
        }
    }
}