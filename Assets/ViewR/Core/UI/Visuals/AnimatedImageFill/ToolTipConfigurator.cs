using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewR.HelpersLib.SurgeExtensions.Animators.Parents;

namespace ViewR.Core.UI.Visuals.AnimatedImageFill
{
    /// <summary>
    /// Allows to configure an image and a text of a tooltip tool and appear their appearers in/out.
    /// Can be addressed using <see cref="ToolTipSetter"/>.
    /// </summary>
    public class ToolTipConfigurator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Appearer[] appearers;
        [SerializeField]
        private Image image;
        [SerializeField]
        private TMP_Text textField;
        [SerializeField]
        private TMP_Text titleTextField;

        private string _defaultText;
        private string _defaultTitle;

        private void Awake()
        {
            _defaultText = textField.text;
            _defaultTitle = titleTextField.text;
        }

        public void AppearIn(Sprite newSprite, string newText = null, string newTitle = null,
            ImageFillConfig imageFillConfig = null)
        {
            // Cache previous value
            var sameImage = image.sprite == newSprite;

            // Set values
            if (!sameImage && imageFillConfig != null)
            {
                // Force set fill amount if changing the image.
                image.fillAmount = 0;
                image.sprite = newSprite;
            }

            textField.text = string.IsNullOrEmpty(newText) ? _defaultText : newText;
            titleTextField.text = string.IsNullOrEmpty(newTitle) ? _defaultTitle : newTitle;

            // Set image config
            imageFillConfig?.SetImageToConfig(image);

            // Appear in
            foreach (var appearer in appearers)
                appearer.Appear(true, startFromCurrentValue: true);
        }

        public void AppearOut()
        {
            // Appear out
            foreach (var appearer in appearers)
                appearer.Appear(false, startFromCurrentValue: true);
        }
    }
}