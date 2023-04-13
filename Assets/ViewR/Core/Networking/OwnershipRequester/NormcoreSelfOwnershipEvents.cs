using System;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace ViewR.Core.Networking.OwnershipRequester
{
    /// <summary>
    /// Fires events based on the given realtime component(s).
    /// Can work with realtime views and realtime transforms, or both.
    /// </summary>
    public class NormcoreSelfOwnershipEvents : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent ownershipUnownedSelf;
        public UnityEvent ownershipOwnedSelf;
        public UnityEvent ownershipOwnedRemote;
        public UnityEvent localOwnershipReleased;

        [Header("Realtime Components")]
        [SerializeField]
        private RealtimeTransform realtimeTransform;
        [SerializeField]
        private RealtimeView realtimeView;

        private bool _wasOurs;

        public bool OwnedLocallySelf => realtimeTransform && realtimeTransform.isOwnedLocallySelf || realtimeView && realtimeView.isOwnedLocallySelf;

        private void Start()
        {
            // Catch if no transform nor view given.
            if (realtimeTransform == null && realtimeView == null)
            {
                Debug.LogWarning($"Received neither a {typeof(RealtimeTransform)}, nor {typeof(RealtimeView)}. This will be disabled.", this);
                this.enabled = false;
            }
        }

        private void OnEnable()
        {
            // Subscribe
            if (realtimeTransform)
                realtimeTransform.ownerIDSelfDidChange += RealtimeTransformOnOwnerIDSelfDidChange;
            if (realtimeView)
                realtimeView.ownerIDSelfDidChange += RealtimeViewOnOwnerIDSelfDidChange;
        }

        private void OnDisable()
        {
            // Unsubscribe
            if (realtimeTransform)
                realtimeTransform.ownerIDSelfDidChange -= RealtimeTransformOnOwnerIDSelfDidChange;
            if (realtimeView)
                realtimeView.ownerIDSelfDidChange -= RealtimeViewOnOwnerIDSelfDidChange;
        }

        private void RealtimeViewOnOwnerIDSelfDidChange(RealtimeView view, int id)
        {
            CheckNewOwnership((IRealtimeComponent) view);
        }

        private void RealtimeTransformOnOwnerIDSelfDidChange(RealtimeComponent<RealtimeTransformModel> model, int id)
        {
            CheckNewOwnership((IRealtimeComponent) model);
        }

        /// <summary>
        /// Evaluates the NEW ownership on the given <see cref="IRealtimeComponent"/>, and fires the respective events.
        /// </summary>
        private void CheckNewOwnership(IRealtimeComponent realtimeComponent)
        {
            // If it is now ours:
            if (realtimeComponent.isOwnedLocallySelf)
            {
                InvokeOwnershipOurs();
                _wasOurs = true;
                return;
            }
            // If its not ours, but was ours:
            if (_wasOurs)
                InvokeLocalOwnershipReleased();
            
            // If its now unowned
            if (realtimeComponent.isUnownedSelf)
                InvokeOwnershipUnowned();
            
            // If its now owned remotely
            if (realtimeComponent.isOwnedRemotelySelf)
                InvokeOwnershipRemote();
        }

        #region Invokers

        private void InvokeLocalOwnershipReleased()
        {
            localOwnershipReleased?.Invoke();
        }

        private void InvokeOwnershipRemote()
        {
            ownershipOwnedRemote?.Invoke();
        }

        private void InvokeOwnershipOurs()
        {
            ownershipOwnedSelf?.Invoke();
        }

        private void InvokeOwnershipUnowned()
        {
            ownershipUnownedSelf?.Invoke();
        }

        #endregion
    }
}