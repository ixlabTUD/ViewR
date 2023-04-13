using System;
using UnityEngine;

namespace ViewR.Core.Networking.OwnershipRequester
{
    /// <summary>
    /// Allows to disable and re-enable physics on a given <see cref="Rigidbody"/> by caching and manipulating its <see cref="Rigidbody.isKinematic"/>. 
    /// </summary>
    public class PhysicsToggler : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rigidbodyToManipulate;

        private bool _savedIsKinematicState = false;
        private bool _savedGravityState = false;

        public void DisablePhysics()
        {
            CachePhysicsState();
            rigidbodyToManipulate.isKinematic = true;
            rigidbodyToManipulate.useGravity = false;
        }

        public void ReenablePhysics()
        {
            // revert the original kinematic state
            rigidbodyToManipulate.isKinematic = _savedIsKinematicState;
            rigidbodyToManipulate.useGravity = _savedGravityState;
        }


        private void CachePhysicsState()
        {
            _savedIsKinematicState = rigidbodyToManipulate.isKinematic;
            _savedGravityState = rigidbodyToManipulate.useGravity;
        }
        
    }
}