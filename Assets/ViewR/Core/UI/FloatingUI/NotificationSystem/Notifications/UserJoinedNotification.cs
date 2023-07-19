using Normal.Realtime;
using UnityEngine;
using ViewR.Core.UI.FloatingUI.NotificationSystem.CoreSystem;

namespace ViewR.Core.UI.FloatingUI.NotificationSystem.Notifications
{
    public class UserJoinedNotification : NotificationPanelCaller
    {
        [Header("References")]
        [SerializeField]
        private RealtimeAvatarManager realtimeAvatarManager;
        
        [Header("Window Config")]
        [SerializeField] private NotificationPanelConfig userJoinedNotificationPanelConfig;
        [SerializeField] private NotificationPanelConfig userLeftNotificationPanelConfig;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            // Subscribe
            realtimeAvatarManager.avatarCreated += HandleAvatarCreated;
            realtimeAvatarManager.avatarDestroyed += HandleAvatarDestroyed;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // Unsubscribe
            realtimeAvatarManager.avatarCreated -= HandleAvatarCreated;
            realtimeAvatarManager.avatarDestroyed -= HandleAvatarDestroyed;
        }
        
        private void HandleAvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
        {
            if (!isLocalAvatar)
                ShowWindow(userJoinedNotificationPanelConfig);
        }
        
        private void HandleAvatarDestroyed(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
        {
            if (!isLocalAvatar)
                ShowWindow(userLeftNotificationPanelConfig);
        }
    }
}
