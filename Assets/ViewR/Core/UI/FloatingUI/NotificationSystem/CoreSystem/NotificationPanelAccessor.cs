using UnityEngine;

namespace ViewR.Core.UI.FloatingUI.NotificationSystem.CoreSystem
{
    /// <summary>
    /// Access components of the NotificationPanel easily from the core parent object. Avoid costly getcomponents.
    /// </summary>
    public class NotificationPanelAccessor : MonoBehaviour
    {
        [SerializeField]
        private NotificationPanel notificationPanel;
        public NotificationPanel NotificationPanel => notificationPanel;
    }
}
