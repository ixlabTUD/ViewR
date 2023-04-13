using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewR.Core.UI.FloatingUI.IntroductionSequencing;
using ViewR.HelpersLib.SurgeExtensions.Animators.UI;

namespace ViewR.Core.UI.FloatingUI.ModalWindow.ProgressBar
{
    /// <summary>
    /// An easy access point to the ProgressBar within the <see cref="ModalWindowPanel"/>.
    /// </summary>
    /// <remarks>
    /// This GameObject will currently be disabled via the <see cref="ModalWindowPanel.ShowWindow"/>. Enable it manually after calling this method.
    /// For example: <see cref="WaitForSecondsWithProgressState"/>
    /// </remarks>
    public class ProgressbarManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider slider;
        public Slider Slider => slider;

        [SerializeField] private UIFadeInOutGraphicCanvasGroup fader;
        public UIFadeInOutGraphicCanvasGroup Fader => fader;

        [SerializeField] private TMP_Text tmp;
        public TMP_Text TextField => tmp;

        public void SetValue(float newValue)
        {
            slider.value = newValue;
            TextField.text = $"{(newValue * 100f):##0.00}%";
        }

        public void Fade(bool fadeIn)
        {
            fader.Appear(fadeIn);
        }

        public void FadeIn()
        {
            fader.Appear(true);
        }

        public void FadeOut()
        {
            fader.Appear(false);
        }

        /// <summary>
        /// Enables the fields. Best use <see cref="ActivateAndFadeFieldsIn"/> instead to avoid sudden changes.
        /// </summary>
        /// <param name="show"></param>
        public void ShowFields(bool show)
        {
            this.gameObject.SetActive(show);
            slider.gameObject.SetActive(show);
            TextField.gameObject.SetActive(show);
        }

        /// <summary>
        /// If fading in: Forces alpha = 0, activates components and tweens them visible.
        ///                 Also ensures, slider and text values are set to 0.
        /// If fading out: Tweens alpha to 0. Does not deactivate the components to avoid sudden layout jumps.
        /// </summary>
        /// <param name="fadeIn"></param>
        public void ActivateAndFadeFieldsIn(bool fadeIn)
        {
            if (fadeIn)
            {
                SetValue(0);
                Fader.ForceInvisible();
                ShowFields(true);
                fader.Appear(true);
            }
            else
            {
                fader.Appear(false);
            }
        }
    }
}