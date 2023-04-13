using Normal.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace ViewR.Core.Networking.Normcore.Utils.Ownership
{
    public class RequestOwnershipForThisComponent : MonoBehaviour
    {
        [SerializeField]
        private RealtimeTransform realtimeTransformToRequest;

        [SerializeField]
        private RealtimeView realtimeViewToRequest;
        
        [SerializeField, Tooltip("If we are owner of this realtime view, we can request permission.")]
        [FormerlySerializedAs("realtimeView")]
        private RealtimeView ownerRealtimeView;

        [SerializeField]
        private bool requestOnStart = true;

        private void Start()
        {
            // Bail if not ours
            if (!ownerRealtimeView.isOwnedLocallySelf || !requestOnStart)
                return;

            RequestIfOurs();
        }

        private void RequestIfOurs()
        {
            if (!ownerRealtimeView.isOwnedLocallySelf)
                return;

            ForceRequest();
        }
        
        [ContextMenu("Force Request")]
        private void ForceRequest()
        {
            if (realtimeTransformToRequest)
                realtimeTransformToRequest.RequestOwnership();
            if (realtimeViewToRequest)
                realtimeViewToRequest.RequestOwnership();
        }

        /// <summary>
        /// Convenience feature to auto populate
        /// </summary>
        [ContextMenu("Attempt AutoPopulate")]
        private void AutoPopulate()
        {
            if (!realtimeTransformToRequest)
                TryGetComponent(out realtimeTransformToRequest);
            if (!realtimeViewToRequest)
                TryGetComponent(out realtimeViewToRequest);
        }
    }
}