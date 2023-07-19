using Normal.Realtime;
using Pixelplacement;
using UnityEngine;
using ViewR.Core.Networking.Normcore.Avatar;
using ViewR.Managers;
using ViewR.StatusManagement;

namespace ViewR.Core.Networking.Normcore
{
    /// <summary>
    /// Fires events based on user changes.
    /// Note: remains somewhat untested
    /// </summary>
    public class UserJoinedEvents : SingletonExtended<UserJoinedEvents>
    {
        #region Events

        public delegate void AvatarCreatedDestroyed(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar);
        public /*static*/ event AvatarCreatedDestroyed FirstRemoteClientConnected;
        public /*static*/ event AvatarCreatedDestroyed FurtherRemoteClientConnected;
        public /*static*/ event AvatarCreatedDestroyed LastRemoteClientDisconnected;
        public /*static*/ event AvatarCreatedDestroyed AvatarCreated;
        public /*static*/ event AvatarCreatedDestroyed AvatarDestroyed;

        #endregion

        /// <summary>
        /// Is there a remote client currently?
        /// Cached value.
        /// </summary>
        public /*static*/ bool RemoteClientCurrentlyPresent { get; private set; }

        
        #region Unity Methods

        private void OnEnable()
        {
            var managerToUse = NetworkManager.Instance.RealtimeAvatarManager;
            
            // Subscribe
            managerToUse.avatarCreated += HandleAvatarCreated;
            managerToUse.avatarDestroyed += HandleAvatarDestroyed;
        }
        private void OnDisable()
        {
            var managerToUse = NetworkManager.Instance.RealtimeAvatarManager;
            
            // Unsubscribe
            managerToUse.avatarCreated -= HandleAvatarCreated;
            managerToUse.avatarDestroyed -= HandleAvatarDestroyed;
        }

        #endregion

        private void HandleAvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
        {
            HandleAvatarChange(avatarManager, avatar, isLocalAvatar, true);
        }

        private void HandleAvatarDestroyed(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
        {
            HandleAvatarChange(avatarManager, avatar, isLocalAvatar, false);
        }

        private void HandleAvatarChange(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar, bool calledFromAvatarCreated)
        {
            var aRemoteClientPresent = AnyRemoteClientsPresent(avatarManager);
            
            if (calledFromAvatarCreated)
            {
                if (aRemoteClientPresent)
                {
                    // If there already was a remote client present:
                    if (RemoteClientCurrentlyPresent)
                    {
                        // Already an remote client present. Don't change things.
                        FurtherRemoteClientConnected?.Invoke(avatarManager, avatar, isLocalAvatar);
                    }
                    // If there was no remote client present so far:
                    else
                    {
                        // The first remote client joined!
                        FirstRemoteClientConnected?.Invoke(avatarManager, avatar, isLocalAvatar);
                        // Set flag.
                        RemoteClientCurrentlyPresent = true;
                    }
                }
                
                // Call this regardless of aRemoteClientPresent, or of first or further remote client.
                AvatarCreated?.Invoke(avatarManager, avatar, isLocalAvatar);
            }
            else // Avatar was destroyed
            {
                // If there was a remote client present so far, but there is none now:
                if (!aRemoteClientPresent && RemoteClientCurrentlyPresent)
                {
                    LastRemoteClientDisconnected?.Invoke(avatarManager, avatar, isLocalAvatar);
                    // Set flag.
                    RemoteClientCurrentlyPresent = false;
                }
                
                // Call this regardless of aRemoteClientPresent.
                AvatarDestroyed?.Invoke(avatarManager, avatar, isLocalAvatar);
            }
        }

        /// <summary>
        /// Checks all avatars in the <see cref="avatarManager"/> and checks if they are remote clients.
        /// </summary>
        public static bool AnyRemoteClientsPresent(RealtimeAvatarManager avatarManager)
        {
            var foundRemoteClient = false;
            
            // Are there any remote clients?
            foreach (var (_, realtimeAvatar) in avatarManager.avatars)
            {
                var accessHelper = realtimeAvatar.GetComponent<AvatarAccessHelper>();
                var currentLocation = accessHelper.SyncedPlayerPropertiesSync.GetCurrentPhysicalLocation();

                // Skip if OnSite.
                if (currentLocation == ClientPhysicalLocation.OnSite)
                    continue;

                foundRemoteClient = true;
                break;
            }

            return foundRemoteClient;
        }
    }
}