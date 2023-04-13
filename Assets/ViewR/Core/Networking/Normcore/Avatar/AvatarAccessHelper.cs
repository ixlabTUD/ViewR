using Normal.Realtime;
using UnityEngine;
using ViewR.Core.Networking.Normcore.SyncPlayerProperties;

namespace ViewR.Core.Networking.Normcore.Avatar
{
    /// <summary>
    /// Provides easy access to objects and components within the synced avatar.
    /// </summary>
    public class AvatarAccessHelper : MonoBehaviour
    {
        [SerializeField]
        private SyncedPlayerPropertiesSync syncedPlayerPropertiesSync;
        public SyncedPlayerPropertiesSync SyncedPlayerPropertiesSync => syncedPlayerPropertiesSync;
        
        [SerializeField]
        private RealtimeAvatar realtimeAvatar;
        public RealtimeAvatar RealtimeAvatar => realtimeAvatar;
        
        [SerializeField]
        private RealtimeView mainRealtimeView;
        public RealtimeView MainRealtimeView => mainRealtimeView;
        
        [SerializeField]
        private RealtimeAvatarVoice realtimeAvatarVoice;
        public RealtimeAvatarVoice RealtimeAvatarVoice => realtimeAvatarVoice;
        
        [SerializeField]
        private AvatarVisualRepresentationManager avatarVisualRepresentationManager;
        public AvatarVisualRepresentationManager AvatarVisualRepresentationManager => avatarVisualRepresentationManager;
        
        private void Awake()
        {
            EnsureReferences();
        }

        private void OnValidate()
        {
            EnsureReferences();
        }

        [ContextMenu("Attempt auto-populate")]
        private void EnsureReferences()
        {
            // Ensure we have refs
            if (!syncedPlayerPropertiesSync)
                if (!TryGetComponent(out syncedPlayerPropertiesSync))
                    Debug.LogError($"{nameof(syncedPlayerPropertiesSync)} could not be found.", this);
            
            if (!realtimeAvatar)
                if (!TryGetComponent(out realtimeAvatar))
                    Debug.LogError($"{nameof(realtimeAvatar)} could not be found.", this);

            if (!realtimeAvatarVoice)
            {
                realtimeAvatarVoice = GetComponentInChildren<RealtimeAvatarVoice>();
                if(!realtimeAvatarVoice)
                    Debug.LogError($"{nameof(realtimeAvatarVoice)} could not be found.", this);
            }
            
            if (!avatarVisualRepresentationManager)
                if (!TryGetComponent(out avatarVisualRepresentationManager))
                    Debug.LogError($"{nameof(avatarVisualRepresentationManager)} could not be found.", this);
        }
    }
}
