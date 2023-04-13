using System;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction;
using Pixelplacement;
using UnityEngine;
using ViewR.Core.UI.FloatingUI.Follower;

namespace ViewR.Core.UI.FloatingUI.NotificationSystem.CoreSystem
{
    public class NotificationPanelManager : SingletonExtended<NotificationPanelManager>
    {
        [SerializeField]
        private Transform multiPanelParent;
        
        [SerializeField, Optional]
        private TargetSetter targetSetter;
        
        private NotificationPanel[] _notificationPanels;

        private Dictionary<NotificationPanelCaller, NotificationPanel> _callerDictionary =
            new Dictionary<NotificationPanelCaller, NotificationPanel>();

        private void Awake()
        {
            _notificationPanels = multiPanelParent.GetComponentsInChildren<NotificationPanel>(true);
        }

        public void ShowNewWindow(NotificationPanelCaller notificationPanelCaller, 
            NotificationPanelConfig notificationPanelConfig, Action callback = null)
        {
            // Get an disabled notification panel
            NotificationPanel unusedPanel = null;
            foreach (var notificationPanel in _notificationPanels)
            {
                if (!notificationPanel.gameObject.activeInHierarchy)
                {
                    unusedPanel = notificationPanel;
                    break;
                }
            }

            // Catch
            if (unusedPanel == null)
            {
                var go = Instantiate(multiPanelParent.GetChild(0).gameObject, multiPanelParent, true);
                unusedPanel = go.GetComponent<NotificationPanelAccessor>().NotificationPanel;
                
                if (targetSetter != null)
                {
                    var targetFollower = go.GetComponent<TargetFollower>();
                    var lookAtFollower = go.GetComponent<LookAtFollower>();
                    targetSetter.SetTargetFollower(targetFollower);
                    targetSetter.SetLookAtFollower(lookAtFollower);
                }
            }
            if (unusedPanel == null)
                throw new MissingReferenceException($"No {nameof(NotificationPanel)} found on {this.name}.");
            
            // Configure window:
            unusedPanel.ShowWindow(notificationPanelConfig, callback);
            
            // Keep track
            if (_callerDictionary.ContainsKey(notificationPanelCaller))
                _callerDictionary.Remove(notificationPanelCaller);
            _callerDictionary.Add(notificationPanelCaller, unusedPanel);
        }

        public void Close(NotificationPanelCaller notificationPanelCaller)
        {
            if (_callerDictionary.TryGetValue(notificationPanelCaller, out var notificationPanel))
            {
                notificationPanel.Close();
                _callerDictionary.Remove(notificationPanelCaller);
            }
            else
            {
                Debug.LogWarning($"Could not find the given {nameof(NotificationPanelCaller)}:{notificationPanelCaller.gameObject.name} in the dictionary...", this);
            }
        }
        
        public void TryRemoveValue(NotificationPanel notificationPanel)
        {
            if (!_callerDictionary.ContainsValue(notificationPanel)) return;
            
            var notificationPanelCaller = _callerDictionary.FirstOrDefault(x => x.Value == notificationPanel).Key;
            _callerDictionary.Remove(notificationPanelCaller);
        }
        
        public bool ContainsPanel(NotificationPanel notificationPanel)
        {
            return _callerDictionary.ContainsValue(notificationPanel);
        }
        
        public bool ContainsCaller(NotificationPanelCaller notificationPanelCaller)
        {
            return _callerDictionary.ContainsKey(notificationPanelCaller);
        }

    }
}