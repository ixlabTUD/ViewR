namespace ViewR.HelpersLib.Utils.UI.Slider
{
    public class SliderValueToTextPercent : SliderValueToText
    {
        protected override void UpdateVisuals(float newValue)
        {
            textField.text = newValue.ToString("P0");
        }
    }
}