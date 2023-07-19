using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using ViewR.Core.UI.FloatingUI.ModalWindow.SerializablesAndReference;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.Core.UI.FloatingUI.NotificationSystem.CoreSystem
{
    public class NotificationPanelCaller : MonoBehaviour
    {
        [Header("Window Configuration")]
        public NotificationPanelConfig notificationPanelConfig;
        [Header("Enabled by...")]
        public bool triggerOnEnable;
        public bool closeOnDisable;
        [Header("Events")] 
        public UnityEvent onConfirmCallback;
        public UnityEvent onDeclineCallback;
        public UnityEvent onAlternate1Callback;
        public UnityEvent onAlternate2Callback;
        public UnityEvent onAlternate3Callback;
        [Header("Callback")] 
        public UnityEvent onShowWindow;
        public UnityEvent onCloseWindow;
        [Space]
        [Header("Modal Window Panel")]
        [Help("Leave this empty to address the menu window! \nOnly assign values here for local UIs!", MessageType.Warning)]
        public NotificationPanel localNotificationPanel;

        protected virtual void OnEnable()
        {
            if (!triggerOnEnable) return;

            ShowWindow();
        }

        protected virtual  void OnDisable()
        {
            if(!closeOnDisable) return;
            
            CloseWindow();
        }

        
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void ShowWindow()
        {
            ShowWindow(null);
        }

        /// <summary>
        /// Hooks up the button events and shows the Notification Window with the configured (<see cref="notificationPanelConfig"/>).
        /// </summary>
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void ShowWindow(Action callback = null)
        {
            // Add events
            ConfigureButtonEvent(ref notificationPanelConfig.confirmButtonConfig, ref onConfirmCallback);
            ConfigureButtonEvent(ref notificationPanelConfig.declineButtonConfig, ref onDeclineCallback);
            ConfigureButtonEvent(ref notificationPanelConfig.alternate1ButtonConfig, ref onAlternate1Callback);
            ConfigureButtonEvent(ref notificationPanelConfig.alternate2ButtonConfig, ref onAlternate2Callback);
            ConfigureButtonEvent(ref notificationPanelConfig.alternate3ButtonConfig, ref onAlternate3Callback);
            
            // Show it!
            if(!localNotificationPanel)
                NotificationPanelManager.Instance.ShowNewWindow(this, notificationPanelConfig, callback);
            else
                // Does not instantiate it
                localNotificationPanel.ShowWindow(notificationPanelConfig, callback);
            
            onShowWindow?.Invoke();
        }

        /// <summary>
        /// Hooks up the button events and shows the Notification Window.
        /// Uses <see cref="newNotificationPanelConfig"/> instead of the local one.
        /// </summary>
        /// <remarks>
        /// Take care of closing the window if not set to auto-close.
        /// </remarks>
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void ShowWindow(NotificationPanelConfig newNotificationPanelConfig, Action callback = null)
        {
            // Add events
            ConfigureButtonEvent(ref newNotificationPanelConfig.confirmButtonConfig, ref onConfirmCallback);
            ConfigureButtonEvent(ref newNotificationPanelConfig.declineButtonConfig, ref onDeclineCallback);
            ConfigureButtonEvent(ref newNotificationPanelConfig.alternate1ButtonConfig, ref onAlternate1Callback);
            ConfigureButtonEvent(ref newNotificationPanelConfig.alternate2ButtonConfig, ref onAlternate2Callback);
            ConfigureButtonEvent(ref newNotificationPanelConfig.alternate3ButtonConfig, ref onAlternate3Callback);
            
            // Show it!
            if(!localNotificationPanel)
                NotificationPanelManager.Instance.ShowNewWindow(this, newNotificationPanelConfig, callback);
            else
                // Does not instantiate it
                localNotificationPanel.ShowWindow(newNotificationPanelConfig, callback);
            
            onShowWindow?.Invoke();
        }
        
        /// <summary>
        /// Hides the window.
        /// Note: The window may get hidden anyway when the user selects a button! Depending on the buttons configuration
        /// </summary>
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void CloseWindow()
        {
            if(!localNotificationPanel)
                NotificationPanelManager.Instance.Close(this);
            else
                localNotificationPanel.Close();
            
            onCloseWindow?.Invoke();
        }

        #region Methods for the internal workings.

        /// <summary>
        /// Configures the button events and sets them to null if no event is given.
        /// </summary>
        private void ConfigureButtonEvent(ref ButtonConfig buttonConfig, ref UnityEvent eventToCall)
        {
            if (eventToCall.GetPersistentEventCount() > 0)
                buttonConfig.clickAction = eventToCall.Invoke;
            // NotificationPanelConfig.confirmButtonConfig = new ButtonConfig { clickAction = onConfirmCallback.Invoke };
            else
                buttonConfig.clickAction = null;
            
        }

        #endregion
    }
}