using TMPro;
using UnityEngine;

namespace ViewR.HelpersLib.Utils.UI.Slider
{
    public class SliderValueToText : MonoBehaviour
    {
        [SerializeField]
        internal TMP_Text textField;

        public virtual void UpdateText(float sliderValue)
        {
            UpdateVisuals(sliderValue);
        }

        protected virtual void UpdateVisuals(float newValue)
        {
            textField.text = newValue.ToString("P0");
        }
    }
}
