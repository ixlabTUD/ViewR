using Normal.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using ViewR.Core.Networking.Normcore.SyncedActiveState;

namespace ViewR.Core.Networking.Normcore.Utils.Ownership
{
    public class RequestOwnershipForAllChildComponents : MonoBehaviour
    {
        [SerializeField]
        private GameObject parentGameObject;

        [FormerlySerializedAs("realtimeView")]
        [SerializeField]
        private RealtimeView ownerRealtimeView;

        [SerializeField]
        private bool requestOnStart = true;

        [Header("Config")]
        [SerializeField]
        private bool requestTransforms = true;
        [SerializeField]
        private bool requestBools = true;

        [SerializeField]
        private bool requestViews;

        private void Start()
        {
            // Bail if not ours
            if (!ownerRealtimeView.isOwnedLocallySelf || !requestOnStart)
                return;

            RequestIfOurs();
        }

        public void RequestIfOurs()
        {
            // Catch
            if (!ownerRealtimeView)
            {
                Debug.LogWarning($"No {nameof(RealtimeView)} given. Cannot run upon {nameof(RequestIfOurs)}. If you want to force-request ownership regardless of {nameof(ownerRealtimeView)}s ownership, call {nameof(ForceRequest)}.", this);
                return;
            }
            
            if (!ownerRealtimeView.isOwnedLocallySelf)
                return;

            ForceRequest();
        }

        [ContextMenu("Force Request for all children")]
        public void ForceRequest()
        {
            if (requestTransforms)
            {
                var transforms = parentGameObject.GetComponentsInChildren<RealtimeTransform>();

                foreach (var rt in transforms)
                    rt.RequestOwnership();
            }
            
            if (requestViews)
            {
                var views = parentGameObject.GetComponentsInChildren<RealtimeView>();

                foreach (var rt in views)
                    rt.RequestOwnership();
            }
            
            if (requestBools)
            {
                var boolSyncs = parentGameObject.GetComponentsInChildren<BoolSync>();

                foreach (var boolSync in boolSyncs)
                    boolSync.RequestOwnership();
            }
        }

        /// <summary>
        /// Convenience feature to auto populate
        /// </summary>
        private void OnValidate()
        {
            if (!parentGameObject)
                parentGameObject = this.gameObject;
        }
    }
}