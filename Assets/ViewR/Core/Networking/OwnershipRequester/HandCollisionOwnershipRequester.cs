using UnityEngine;

namespace ViewR.Core.Networking.OwnershipRequester
{
    /// <summary>
    /// Request the ownerships of the given <see cref="realtimeTransformsToRequest"/> if its <see cref="Collider"/> is set to trigger, and it collides with another <see cref="Collider"/>.
    /// </summary>
    /// <remarks>
    /// Note: This script only requests upon collision with the users hand.
    /// Note: This script requires the 4th parent of the hand colliders to be tagged with "OVRHand".
    ///
    /// Limitation: Never releases ownership.
    /// </remarks>
    [RequireComponent(typeof(Collider))]
    public class HandCollisionOwnershipRequester : RealtimeTransformOwnershipRequester
    {
        private void OnTriggerEnter(Collider other)
        {
            // Bail if this object is not active in hierarchy.
            if (!this.gameObject.activeInHierarchy)
                return;
            
            if ( other.transform.parent?.parent?.parent?.parent != null && 
                 other.transform.parent.parent.parent.parent.CompareTag("OVRHand"))
            {
                RequestOwnerships();
            }
        }
    }
}
