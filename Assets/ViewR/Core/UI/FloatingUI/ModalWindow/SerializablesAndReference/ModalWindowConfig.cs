using UnityEngine;

namespace ViewR.Core.UI.FloatingUI.ModalWindow.SerializablesAndReference
{
    [System.Serializable]
    public class ModalWindowConfig
    {
        [Tooltip("If no title is given, the title bar will be hidden")]
        public string title = string.Empty;

        [Tooltip("If no image is given, it will be hidden")]
        public Sprite image = null;

        [Tooltip("The message the user should see. If no message is given, it will be hidden")]
        public string message;

        [Tooltip("The colors of the UI.")] 
        public UIColors uiColors;

        [Tooltip("The layout used for the body content.")]
        public ModalWindowBodyLayout modalWindowBodyLayout = ModalWindowBodyLayout.Vertical;
        
        [Tooltip("Usually, the image will be displayed first.")]
        public bool reverseImageAndTextOrder = false;

        [Tooltip("Whether or not the pin button should be displayed.")]
        public bool showPinButton = true;

        [Tooltip("Should we pin/unpin the window even though it might've been following/pinned?")]
        public UIFollowMode forceFollowState = UIFollowMode.ForceFollowAndPinUponReachingTarget;
        
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