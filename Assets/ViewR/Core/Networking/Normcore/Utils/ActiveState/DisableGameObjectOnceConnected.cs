using Normal.Realtime;
using UnityEngine;
using ViewR.Core.Networking.Normcore.RealtimeInstanceManagement;

namespace ViewR.Core.Networking.Normcore.Utils.ActiveState
{
    public class DisableGameObjectOnceConnected : RealtimeReferencer
    {
        [SerializeField]
        private bool destroyOnceConnected;
        
        private void Start()
        {
            RealtimeToUse.didConnectToRoom += RealtimeOndidConnectToRoom;
        }

        private void OnDestroy()
        {
            RealtimeToUse.didConnectToRoom -= RealtimeOndidConnectToRoom;
        }

        private void RealtimeOndidConnectToRoom(Realtime realtime1)
        {
            this.gameObject.SetActive(false);
            
            
            // Clean up - if we are not in the editor. Allowing us to keep track easier in the editor.
#if !UNITY_EDITOR
            if(destroyOnceConnected)
                Destroy(this);
#endif
        }
    }
}
