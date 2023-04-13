using Normal.Realtime;
using UnityEngine;
using ViewR.Core.Networking.Normcore.Voice.Settings;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore.Voice
{
    /// <summary>
    /// Searches for <see cref="UserJoinedEvents.AnyRemoteClientsPresent"/> and then mutes/unmutes the <b>local</b> client.
    ///
    /// <see cref="MuteUnmuteMic"/> is the actual MAIN class that actually handles mute / unmute of the local user.
    /// </summary>
    public class VoiceInputManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private MuteUnmuteMic muteUnmuteMic;
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
        /// Searches for <see cref="UserJoinedEvents.AnyRemoteClientsPresent"/> and then mutes or unmutes the local client
        /// </summary>
        private void HandleAvatarChange(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
        {
            var remoteClientPresent = UserJoinedEvents.AnyRemoteClientsPresent(avatarManager);

            // Mute us if there is no remote client.
            if (!remoteClientPresent)
            {
                if (debugging)
                    Debug.Log($"{nameof(VoiceInputManager)}.{nameof(HandleAvatarChange)}: muting local client.", this);
                
                // var accessHelper = avatarManager.localAvatar.GetComponent<AvatarAccessHelper>();
                // accessHelper.RealtimeAvatarVoice.mute = false;

                muteUnmuteMic.Mute = true;
            }
            else
            {
                if (debugging)
                    Debug.Log($"{nameof(VoiceInputManager)}.{nameof(HandleAvatarChange)}: unmuting local client. Note, the user currently does not control this and should at least be notified.", this);
                
                // ToDo: Don't force unmute - or at least send in a notification.
                // var accessHelper = avatarManager.localAvatar.GetComponent<AvatarAccessHelper>();
                // accessHelper.RealtimeAvatarVoice.mute = true;
                
                muteUnmuteMic.Mute = false;
            }
        }
    }
}