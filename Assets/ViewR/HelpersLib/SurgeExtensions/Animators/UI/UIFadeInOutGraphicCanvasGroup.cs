using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions.ScriptableObjects;
using UnityEngine;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.UI
{
    /// <summary>
    /// Fades in a CanvasGroup on <see cref="Appear"/>
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIFadeInOutGraphicCanvasGroup : UIAppearer
    {
        [Header("Surge Tween")] 
        [SerializeField] private TweenConfigFade tweenConfigFade;

        private TweenBase _tweenBaseFade;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            // Get refs
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Appear(bool appear, bool invertDirection = false, bool startFromCurrentValue = false, Action callback = null)
        {
            base.Appear(appear, invertDirection, startFromCurrentValue, callback);

            // Stop current animations
            _tweenBaseFade?.Stop();
            
            // Start a new one
            _tweenBaseFade = Tween.CanvasGroupAlpha(
                _canvasGroup,
                startFromCurrentValue ? _canvasGroup.alpha : appear ? 0f : 1f,
                appear ? 1f : 0f,
                duration: tweenConfigFade.Duration,
                delay: tweenConfigFade.Delay,
                easeCurve: tweenConfigFade.AnimationCurve,
                loop: tweenConfigFade.loopType);

            // Stop previous callback if there is one
            if(previousDelayedCallback!= null)
                StopCoroutine(previousDelayedCallback);
            
            // Start callback delayed, if given.
            if(callback != null)
            {
                var maxDuration = tweenConfigFade.Delay + tweenConfigFade.Duration;
                previousDelayedCallback = StartCoroutine(StartCallbackAfterSeconds(maxDuration, callback));
            }
        }

        /// <summary>
        /// Sets the canvas' alpha to 0, without animation
        /// </summary>
        public void ForceInvisible()
        {
            // Stop current animations
            _tweenBaseFade?.Stop();
            
            if(!_canvasGroup)
                _canvasGroup = GetComponent<CanvasGroup>();
            
            _canvasGroup.alpha = 0;
        }

        /// <summary>
        /// Sets the canvas' alpha to 1, without animation
        /// </summary>
        public void ForceVisible()
        {
            // Stop current animations
            _tweenBaseFade?.Stop();
            
            if(!_canvasGroup)
                _canvasGroup = GetComponent<CanvasGroup>();
            
            _canvasGroup.alpha = 1;
        }


        #region Old version: fetches all child canvas group - thus less efficient

        
        // private List<TweenBase> _tweenBaseFade = new List<TweenBase>();
        //
        //
        // public override void Appear(bool appear, bool invertDirection = false, Action callback = null)
        // {
        //     base.Appear(appear, invertDirection, callback);
        //     
        //     // Stop current animations
        //     if(_tweenBaseFade.Count > 0)
        //     {
        //         foreach (var tweenBase in _tweenBaseFade)
        //             tweenBase.Stop();
        //         _tweenBaseFade.Clear();
        //     }
        //
        //     
        //     foreach (var canvasGroup in this.GetComponentsInChildren<CanvasGroup>())
        //     {
        //         _tweenBaseFade.Add(
        //             Tween.CanvasGroupAlpha(canvasGroup,
        //                 appear ? 0f : 1f,
        //                 appear ? 1f : 0f,
        //                 duration: tweenConfigFade.duration,
        //                 delay: tweenConfigFade.delay,
        //                 easeCurve: tweenConfigFade.AnimationCurve,
        //                 loop: tweenConfigFade.loopType)
        //         );
        //     }

        #endregion
        
    }
}