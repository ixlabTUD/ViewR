using UnityEngine;

namespace ViewR.Core.UI.FloatingUI.ModalWindow.SerializablesAndReference
{
    /// <summary>
    /// A simple way to configure UI Colors as we go.
    /// </summary>
    [System.Serializable]
    public class UIColors
    {
        [Tooltip("If set to false, the default values will be applied.")]
        public bool useCustomColors;

        public Color headerColor;
        public Color bodyColor;
        public Color footerColor;
    }
}