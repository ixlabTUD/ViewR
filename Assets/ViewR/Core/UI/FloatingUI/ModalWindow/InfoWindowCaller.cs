using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using ViewR.Core.UI.FloatingUI.ModalWindow.SerializablesAndReference;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.Core.UI.FloatingUI.ModalWindow
{
    /// <summary>
    /// Class that allows to configure and call the info-window.
    /// Both, from within the editor or through code.
    /// </summary>
    public class InfoWindowCaller : MonoBehaviour
    {
        [Header("Window Configuration")]
        public ModalWindowConfig modalWindowConfig;
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
        public ModalWindowPanel LocalModalWindowPanel;

        private void OnEnable()
        {
            if (!triggerOnEnable) return;

            ShowWindow();
        }

        private void OnDisable()
        {
            if(!closeOnDisable) return;
            
            CloseWindow();
        }

        /// <summary>
        /// Hooks up the button events and shows the Modal Window.
        /// </summary>
        public void ShowWindow(Action callback = null)
        {
            // Add events
            ConfigureButtonEvent(ref modalWindowConfig.confirmButtonConfig, ref onConfirmCallback);
            ConfigureButtonEvent(ref modalWindowConfig.declineButtonConfig, ref onDeclineCallback);
            ConfigureButtonEvent(ref modalWindowConfig.alternate1ButtonConfig, ref onAlternate1Callback);
            ConfigureButtonEvent(ref modalWindowConfig.alternate2ButtonConfig, ref onAlternate2Callback);
            ConfigureButtonEvent(ref modalWindowConfig.alternate3ButtonConfig, ref onAlternate3Callback);
            
            // Show it!
            if(!LocalModalWindowPanel)
                ModalWindowUIController.Instance.ModalWindowPanel.ShowWindow(modalWindowConfig, callback);
            else
                LocalModalWindowPanel.ShowWindow(modalWindowConfig, callback);
            
            onShowWindow?.Invoke();
        }
        
        /// <summary>
        /// Hides the window.
        /// Note: The window may get hidden anyway when the user selects a button! Depending on the buttons configuration
        /// </summary>
        public void CloseWindow()
        {
            if(!LocalModalWindowPanel)
                ModalWindowUIController.Instance.ModalWindowPanel.Close();
            else
                LocalModalWindowPanel.Close();
            
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
            // ModalWindowConfig.confirmButtonConfig = new ButtonConfig { clickAction = onConfirmCallback.Invoke };
            else
                buttonConfig.clickAction = null;
        }

        #endregion
    }
}