using Normal.Realtime;
using UnityEngine;

namespace ViewR.Core.Networking.OwnershipRequester
{
    public class RealtimeTransformOwnershipRequester : MonoBehaviour
    {
        [SerializeField]
        internal RealtimeTransform[] realtimeTransformsToRequest;
        
        public void RequestOwnerships()
        {
            for (var i = 0; i < realtimeTransformsToRequest.Length; i++)
            {
                realtimeTransformsToRequest[i].RequestOwnership();
            }
        }
        
        public void ReleaseOwnerships()
        {
            for (var i = 0; i < realtimeTransformsToRequest.Length; i++)
            {
                realtimeTransformsToRequest[i].ClearOwnership();
            }
        }
    }
}