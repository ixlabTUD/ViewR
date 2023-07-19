using Normal.Realtime;
using UnityEngine;
using ViewR.Core.Networking.Normcore.RealtimeInstanceManagement;
using ViewR.Core.UI.FloatingUI.NotificationSystem.CoreSystem;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;
using ViewR.Managers;

namespace ViewR.Core.UI.FloatingUI.NotificationSystem.Notifications
{
    [RequireComponent(typeof(NotificationPanelCaller))]
    public class ConnectionChangedNotification : RealtimeReferencer
    {
        [Help("This code will overwrite the messages configured in these Configs.")]
        [ReadOnly, SerializeField]
        private bool readMeAbove;
        [Header("Window Config")]
        [SerializeField] private NotificationPanelConfig connectedNotificationPanelConfig;
        [SerializeField] private NotificationPanelConfig disconnectNotificationPanelConfig;
        
        private NotificationPanelCaller _notificationPanelCaller;
        private bool _didSubscribeToRealtime;

        private void Awake()
        {
            _notificationPanelCaller = GetComponent<NotificationPanelCaller>();
        }

        #region Subscribe to Singleton registration

        private void OnEnable()
        {
            if (!_notificationPanelCaller)
                _notificationPanelCaller = GetComponent<NotificationPanelCaller>();

            // Subscribe Once Manager Registered
            NetworkManager.SingletonWasRegistered += DoSubscribeToRealtimeEvents;
            NetworkManager.SingletonWasUnregistered += DoUnsubscribeFromRealtimeEvents;
        }

        private void OnDisable()
        {
            // Unsubscribe
            NetworkManager.SingletonWasRegistered -= DoSubscribeToRealtimeEvents;
            NetworkManager.SingletonWasUnregistered -= DoUnsubscribeFromRealtimeEvents;

            if (_didSubscribeToRealtime)
                DoUnsubscribeFromRealtimeEvents();
        }
        
        #endregion

        #region Subscribe to Realtime

        private void DoSubscribeToRealtimeEvents()
        {
            // Subscribe
            RealtimeToUse.didConnectToRoom += HandleConnect;
            RealtimeToUse.didDisconnectFromRoom += HandleDisconnect;
            _didSubscribeToRealtime = true;
        }
        
        private void DoUnsubscribeFromRealtimeEvents()
        {
            // Unsubscribe
            RealtimeToUse.didConnectToRoom -= HandleConnect;
            RealtimeToUse.didDisconnectFromRoom -= HandleDisconnect;
            _didSubscribeToRealtime = false;
        }

        #endregion

        #region Actual Logic

        private void HandleConnect(Realtime realtime)
        {
            connectedNotificationPanelConfig.message = $"Did connect to {realtime.room.name}";

            _notificationPanelCaller.ShowWindow(connectedNotificationPanelConfig);
        }

        private void HandleDisconnect(Realtime realtime)
        {
            connectedNotificationPanelConfig.message = $"Did disconnect from {realtime.room.name}";

            _notificationPanelCaller.ShowWindow(disconnectNotificationPanelConfig);
        }


        #endregion
    }
}