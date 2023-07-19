using System;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Assertions;

namespace ViewR.Core.Networking.OwnershipRequester
{
    /// <summary>
    /// Based on <see cref="NormcoreSelfOwnershipEvents"/>, this will toggle an objects physics via <see cref="PhysicsToggler"/>.
    /// </summary>
    [RequireComponent(typeof(NormcoreSelfOwnershipEvents))]
    public class NetworkPhysicsToggler : PhysicsToggler
    {
        private NormcoreSelfOwnershipEvents _selfOwnershipEvents;
        
        protected bool _started;

        private void Awake()
        {
            // Fetch Refs
            _selfOwnershipEvents = GetComponent<NormcoreSelfOwnershipEvents>();
        }

        private void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(_selfOwnershipEvents);
            this.EndStart(ref _started);

            // Disable physics to ensure its not flickering
            if (!_selfOwnershipEvents.OwnedLocallySelf)
                DisablePhysics();
        }

        private void OnEnable()
        {
            // Subscribe
            _selfOwnershipEvents.ownershipOwnedSelf.AddListener(ReenablePhysics);
            _selfOwnershipEvents.localOwnershipReleased.AddListener(DisablePhysics);
        }

        private void OnDisable()
        {
            // Unsubscribe
            _selfOwnershipEvents.ownershipOwnedSelf.RemoveListener(ReenablePhysics);
            _selfOwnershipEvents.localOwnershipReleased.RemoveListener(DisablePhysics);
        }
    }
}