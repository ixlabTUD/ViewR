using UnityEngine;

namespace ViewR.Core.Avatar
{
    /// <summary>
    /// Sends a ray downwards, looking for the "Ground" layer.
    /// If it's found within 2.5m, the GameObject will be scaled to the hit.distance.
    /// Otherwise it will be assuming it's initial scale.
    /// </summary>
    /// <remarks>
    /// - Draws lines in the editor
    /// - Should be executed only locally and synced to avoid costly ray casts on potentially complex models. 
    /// </remarks>
    public class AutoBodySize : MonoBehaviour
    {
        private enum BodyGeometry
        {
            Cube,
            Cylinder
        }

        [SerializeField]
        private LayerMask groundLayerMask;

        [SerializeField]
        private BodyGeometry bodyGeometry;

        [SerializeField]
        private Transform groundFindingRayOrigin;

        private float _initialHeightScale;
        private const float MaxHeight = 2.5f;


        private void Awake()
        {
            _initialHeightScale = this.transform.localScale.y;

            if (!groundFindingRayOrigin)
                groundFindingRayOrigin = transform;
        }

        private void FixedUpdate()
        {
            RunRaycast();
        }


        private void RunRaycast()
        {
            var originPosition = groundFindingRayOrigin.position;
            if (Physics.Raycast(originPosition, groundFindingRayOrigin.TransformDirection(Vector3.down), out var hit,
                    MaxHeight, groundLayerMask))
            {
#if UNITY_EDITOR
                Debug.DrawLine(originPosition, originPosition + (Vector3.down * hit.distance), Color.yellow, 1f,
                    depthTest: false);
#endif
                // Apply current scale
                var scale = bodyGeometry == BodyGeometry.Cube ? hit.distance : hit.distance / 2;
                var thisTransform = transform;
                var previousLocalScale = thisTransform.localScale;
                thisTransform.localScale = new Vector3(x: previousLocalScale.x,
                    y: scale,
                    z: previousLocalScale.z);
            }
            else
            {
#if UNITY_EDITOR
                Debug.DrawLine(originPosition, originPosition + (Vector3.down * MaxHeight), Color.white, 1f,
                    depthTest: false);
#endif
                // Apply initial scale
                var thisTransform = transform;
                var previousLocalScale = thisTransform.localScale;
                thisTransform.localScale = new Vector3(x: previousLocalScale.x,
                    y: _initialHeightScale,
                    z: previousLocalScale.z);
            }
        }
    }
}