using UnityEngine;

namespace ViewR.Core.OVR.UX
{
    public class OnEnableLookAtTransformOnce : MonoBehaviour
    {
        [SerializeField]
        private Transform lookAtTarget;
        [SerializeField]
        private bool inverse;

        private void OnEnable()
        {
            LookAt(lookAtTarget);
        }

        [ContextMenu("Look At Target")]
        private void LookAtTarget() => LookAt(lookAtTarget);
        
        private void LookAt(Transform target)
        {
            if(!inverse) 
                transform.LookAt(target);
            else
                transform.rotation = Quaternion.LookRotation(transform.position - target.position);
        }
    }
}
