using System;
using UnityEngine;

namespace ViewR.Core.UI.FloatingUI.ModalWindow.SerializablesAndReference
{
    [System.Serializable]
    public class ButtonConfig
    {
        [Tooltip("If no action is assigned in the below events, the button won't be shown.")]
        public string buttonText;

        public Action clickAction;
        
        // Button Color
        [Tooltip("Actually use the below assigned color")]
        public bool useCustomColor;
        [Tooltip("Only gets used if useCustomColor is true!")]
        public Color customButtonColor;

        // Font Color
        [Tooltip("Actually use the below assigned sprite")]
        public bool useCustomFontColor;
        [Tooltip("Only gets used if useCustomFontColor is true!")]
        public Color customFontColor;
        
        // Button sprite
        [Tooltip("Actually use the below assigned sprite")]
        public bool useCustomBackgroundSprite;
        [Tooltip("Only gets used if useCustomSprite is true!")]
        public Sprite customButtonBackgroundSprite;
        
        [Tooltip(" Should we close the window after clicking this button?")]
        public bool closeWindowIfButtonClicked = true;
    }
}