using Oculus.Interaction;
using UnityEngine;

namespace ViewR.Core.UI.Visuals.AnimatedImageFill
{
    /// <summary>
    /// Allows to configure an image and a text for a tooltip tool to appear correctly.
    /// Works with <see cref="toolTipConfigurator"/>.
    /// Can be used together with <see cref="Tooltip"/> to be toggled.
    /// </summary>
    public class ToolTipSetter : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private Sprite displayedSprite;
        [SerializeField, Optional]
        private string displayedText = "";
        [SerializeField, Optional]
        private string displayedTitle = "";
        
        [Header("Fill Config")]
        [SerializeField]
        private bool useImageFillConfig;
        [SerializeField]
        private ImageFillConfig imageFillConfig;

        [Header("References")]
        [SerializeField]
        private ToolTipConfigurator toolTipConfigurator;

        public void ShowTooltip()
        {
            toolTipConfigurator.AppearIn(displayedSprite,
                displayedText,
                displayedTitle,
                imageFillConfig: useImageFillConfig ? imageFillConfig : null);
        }

        public void HideTooltip()
        {
            toolTipConfigurator.AppearOut();
        }
    }
}
