using UnityEngine;
using ViewR.Core.Networking.Normcore.Voice.Settings;
using ViewR.Core.UI.FloatingUI.NotificationSystem.CoreSystem;

namespace ViewR.Core.UI.FloatingUI.NotificationSystem.Notifications
{
    /// <summary>
    /// Easily show notifications for Mute changes from ANYWHERE! :)
    /// </summary>
    public class MicUpdatedNotification : NotificationPanelCaller
    {
        [Header("MuteMic Window Config")]
        [SerializeField] private NotificationPanelConfig mutedNotificationPanelConfig;
        [SerializeField] private NotificationPanelConfig unmutedNotificationPanelConfig;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            // Subscribe
            MuteUnmuteMic.OnMuteChanged += HandleMuteChange;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // Unsubscribe
            MuteUnmuteMic.OnMuteChanged -= HandleMuteChange;
        }

        /// <summary>
        /// Shows the window configured above.
        /// </summary>
        private void HandleMuteChange(object sender, MuteUnmuteMic.OnMuteChangedEventArgs eventArgs)
        {
            if (!Application.isPlaying)
                return;
            ShowWindow(eventArgs.Muted ? mutedNotificationPanelConfig : unmutedNotificationPanelConfig);
        }
    }
}
