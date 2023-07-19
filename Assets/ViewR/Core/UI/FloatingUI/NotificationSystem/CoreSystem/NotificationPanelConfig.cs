using UnityEngine;
using UnityEngine.Serialization;
using ViewR.Core.UI.FloatingUI.ModalWindow.SerializablesAndReference;

namespace ViewR.Core.UI.FloatingUI.NotificationSystem.CoreSystem
{
    [System.Serializable]
    public class NotificationPanelConfig
    {
        [Tooltip("If no title is given, the title bar will be hidden")]
        public string title = string.Empty;

        [Tooltip("If no image is given, it will be hidden")]
        public Sprite image = null;

        [Tooltip("The message the user should see. If no message is given, it will be hidden")]
        public string message;

        [Tooltip("Should the window close automatically?")]
        public bool shouldAutoClose = true;

        [Tooltip("If the window should close automatically, after what time?")]
        public float autoCloseTimeOut = 5f;

        [Tooltip("The colors of the UI.")] 
        public UIColors uiColors;

        [Tooltip("Usually, the image will be displayed first.")]
        public bool reverseImageAndTextOrder = false;

        [FormerlySerializedAs("showCloseButton")]
        [Tooltip("Whether or not the close button should be displayed. Note, that the close button will always be displayed, if the notification itself does not contain any buttons.")]
        public bool forceShowCloseButton = false;
        
        [Tooltip("Whether or not the pin button should be displayed.")]
        public bool showPinButton = true;

        [Tooltip("Should we pin/unpin the window even though it might've been following/pinned?")]
        public UIFollowMode forceFollowState = UIFollowMode.ForceFollowing;
        
        [Tooltip("If no action is assigned, the respective button won't be shown.")]
        public ButtonConfig confirmButtonConfig = null;

        [Tooltip("If no action is assigned, the respective button won't be shown.")]
        public ButtonConfig declineButtonConfig = null;

        [Tooltip("If no action is assigned, the respective button won't be shown.")]
        public ButtonConfig alternate1ButtonConfig = null;

        [Tooltip("If no action is assigned, the respective button won't be shown.")]
        public ButtonConfig alternate2ButtonConfig = null;

        [Tooltip("If no action is assigned, the respective button won't be shown.")]
        public ButtonConfig alternate3ButtonConfig = null;
    }
}