using Normal.Realtime;
using UnityEngine;
using ViewR.Core.Networking.Normcore.Avatar;
using ViewR.Managers;
using ViewR.StatusManagement;

namespace ViewR.Core.Networking.Normcore.Voice
{
    /// <summary>
    /// Controls the voice output of all avatars - except the local, because it does not contain a <see cref="AudioSource"/>.
    /// 
    /// Reacts to changes in the avatar manager.
    /// If there are no remote clients, turn everyone's volume to 0.
    /// If there are on site clients, and we are on site, turn volume on for Remote clients to <see cref="remoteVolume"/>.
    /// If there are on site clients, and we are Remote, turn volume on for all clients to <see cref="remoteVolume"/>.
    /// </summary>
    public class VoiceOutputManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float onSiteVolume = 0;

        [SerializeField]
        private float remoteVolume = 1f;
        
        [Header("Debugging")]
        [SerializeField]
        private bool debugging;
        
        #region Unity Methods

        private void OnEnable()
        {
            // Subscribe
            NetworkManager.Instance.RealtimeAvatarManager.avatarCreated += HandleAvatarCreated;
            NetworkManager.Instance.RealtimeAvatarManager.avatarDestroyed += HandleAvatarDestroyed;
        }
        private void OnDisable()
        {
            // Unsubscribe
            NetworkManager.Instance.RealtimeAvatarManager.avatarCreated -= HandleAvatarCreated;
            NetworkManager.Instance.RealtimeAvatarManager.avatarDestroyed -= HandleAvatarDestroyed;
        }

        #endregion

        private void HandleAvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
        {
            HandleAvatarChange(avatarManager, avatar, isLocalAvatar);
        }

        private void HandleAvatarDestroyed(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
        {
            HandleAvatarChange(avatarManager, avatar, isLocalAvatar);
        }

        
        /// <summary>
        /// Reacts to changes in the avatar manager.
        /// If there are no remote clients, turn everyone's volume to 0.
        /// If there are on site clients, and we are on site, turn volume on for Remote clients to <see cref="remoteVolume"/>.
        /// If there are on site clients, and we are Remote, turn volume on for all clients to <see cref="remoteVolume"/>.
        /// </summary>
        /// <remarks>
        /// The local avatar has to be handled differently, as it does not contain an <see cref="AudioSource"/> component.
        /// </remarks>
        private void HandleAvatarChange(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
        {
            var remoteClientPresent = UserJoinedEvents.AnyRemoteClientsPresent(avatarManager);

            // Apply volume to all.
            if (!remoteClientPresent)
            {
                if (debugging)
                    Debug.Log($"{nameof(VoiceOutputManager)}.{nameof(HandleAvatarChange)}: Only on site clients. Muting volume output for all clients.", this);
                
                ModifyAllAvatarsVolume(avatarManager, onSiteVolume);
                return;
            }

            // If there is any client that is remote: check our own location and apply changes based on it.
            var weAreOnSite = ClientPhysicalLocationState.CurrentClientPhysicalLocation == ClientPhysicalLocation.OnSite;

            // If we are on site, make all remote avatars audible
            if (weAreOnSite)
            {
                if (debugging)
                    Debug.Log($"{nameof(VoiceOutputManager)}.{nameof(HandleAvatarChange)}: we are OnSite. Making all remote avatars audible.", this);

                ModifyAvatarVolumeForRemoteAvatars(avatarManager, remoteVolume);
            }
            // Otherwise, make all avatars audible.
            else
            {
                if (debugging)
                    Debug.Log($"{nameof(VoiceOutputManager)}.{nameof(HandleAvatarChange)}: we are not OnSite. Making all avatars audible.", this);

                ModifyAllAvatarsVolume(avatarManager, remoteVolume);
            }
        }

        private void ModifyAvatarVolumeForRemoteAvatars(RealtimeAvatarManager avatarManager, float volume)
        {
            foreach (var (_, realtimeAvatar) in avatarManager.avatars)
            {
                var accessHelper = realtimeAvatar.GetComponent<AvatarAccessHelper>();
                if (accessHelper.SyncedPlayerPropertiesSync.GetCurrentPhysicalLocation() == ClientPhysicalLocation.OnSite)
                {
                    // Skip
                    continue;
                }
                
                // Get voice and set volume
                var realtimeAvatarVoice = accessHelper.RealtimeAvatarVoice;
                SetAvatarVoiceVolume(realtimeAvatarVoice, volume);
            }
        }

        private void ModifyAllAvatarsVolume(RealtimeAvatarManager avatarManager, float volume)
        {
            foreach (var (_, realtimeAvatar) in avatarManager.avatars)
            {
                var realtimeAvatarVoice = realtimeAvatar.GetComponent<AvatarAccessHelper>().RealtimeAvatarVoice;
                SetAvatarVoiceVolume(realtimeAvatarVoice, volume);
            }
        }

        /// <summary>
        /// Sets the volume on the <see cref="RealtimeAvatarVoice"/> to <see cref="volume"/>.
        /// </summary>
        /// <returns>Whether we could set the <see cref="volume"/>.</returns>
        private bool SetAvatarVoiceVolume(Component realtimeAvatarVoice, float volume)
        {
            // Set volume on avatars that do have a audio source component on their voice object.
            var foundComponent = realtimeAvatarVoice.TryGetComponent(out AudioSource voiceAudioSource);
            if (foundComponent)
            {
                voiceAudioSource.volume = volume;
            }
            else if (debugging)
                Debug.Log($"{nameof(VoiceOutputManager)}.{nameof(HandleAvatarChange)}: found an avatar without voice. isOwnedLocallySelf: {realtimeAvatarVoice.GetComponentInParent<RealtimeAvatar>().isOwnedLocallySelf}.", this);

            return foundComponent;
        }
    }
}