using Pixelplacement;
using SurgeExtensions.ScriptableObjects;
using UnityEngine;

namespace ViewR.HelpersLib.SurgeExtensions.Animators
{
    /// <summary>
    /// Gets the CanvasGroup and fades it in according to the configured profile
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeInCanvasOnEnable : MonoBehaviour
    {
        [SerializeField] private TweenConfigFade tweenConfigFade;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            Tween.CanvasGroupAlpha(_canvasGroup, 0, 1, tweenConfigFade.Duration, tweenConfigFade.Delay,
                tweenConfigFade.AnimationCurve, tweenConfigFade.loopType);
        }
    }
}
