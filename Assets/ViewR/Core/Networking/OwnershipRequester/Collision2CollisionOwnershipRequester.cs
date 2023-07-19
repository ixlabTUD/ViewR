using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Assertions;

namespace ViewR.Core.Networking.OwnershipRequester
{
    /// <summary>
    /// Requests the ownership on another collider, IFF that object contains another <see cref="Collision2CollisionOwnershipRequester"/>.
    /// This passes ownership on to other objects, allowing to move objects by collision with other objects.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Collision2CollisionOwnershipRequester : RealtimeTransformOwnershipRequester
    {
        [SerializeField, Tooltip("Setting this to false forces this collider to run \"TryGetComponent\" on EVERY collision.")]
        private bool executeOnlyOnTriggerColliders = true;

        protected bool _started;
        
        private void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(realtimeTransformsToRequest);
            this.EndStart(ref _started);
        }

        private void OnTriggerEnter(Collider other)
        {
            // Bail if this object is not active in hierarchy.
            if (!this.gameObject.activeInHierarchy)
                return;
            
            // Bail if we do not own this ones first realtime transform.
            if (!realtimeTransformsToRequest[0].isOwnedLocallySelf) 
                return;
            
            // Save a bit by only running GetComponent if we make it past these checks. 
            if (executeOnlyOnTriggerColliders)
                if (!other.isTrigger) 
                    return;

            // Don't do anything if the other does not have the given component.
            if (!other.TryGetComponent(out Collision2CollisionOwnershipRequester otherCollisionOwnershipRequester))
                return;
            
            // Request ownership of the other object
            otherCollisionOwnershipRequester.RequestOwnerships();
        }
    }
}