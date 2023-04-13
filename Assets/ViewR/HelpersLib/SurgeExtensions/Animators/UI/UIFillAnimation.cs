using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.UI
{
    [RequireComponent(typeof(Image))]
    public class UIFillAnimation : UIAppearer
    {
        [SerializeField] private bool applyOnEnable;

        [Header("Surge Tween")] 
        [SerializeField] private TweenConfigFill tweenConfigFill;
        
        private Image _image;
        private TweenBase _tweenBase;

        private void Awake()
        {
            // Get refs
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            if (applyOnEnable)
            {
                Appear(true);
            }
        }

        public override void Appear(bool appear, bool invertDirection = false, bool startFromCurrentValue = false, Action callback = null)
        {
            if (!startFromCurrentValue)
                _image.fillAmount = appear ? 0f : 1f;
            
            base.Appear(appear, invertDirection, startFromCurrentValue, callback);
            
            // Stop current animations
            _tweenBase?.Stop();
            
            // Start a new one
            _tweenBase = Tween.Value(
                    startValue: startFromCurrentValue ? _image.fillAmount : appear ? 0f : 1f,
                    endValue: appear ? 1f : 0f,
                    duration: tweenConfigFill.Duration,
                    delay: tweenConfigFill.Delay,
                    easeCurve: tweenConfigFill.AnimationCurve,
                    loop: tweenConfigFill.loopType,
                    valueUpdatedCallback: newValue => _image.fillAmount = newValue)
                ;
            
            // Stop previous callback if there is one
            if(previousDelayedCallback!= null)
                StopCoroutine(previousDelayedCallback);
            
            // Start callback delayed, if given.
            if(callback != null)
            {
                var maxDuration = tweenConfigFill.Delay + tweenConfigFill.Duration;
                previousDelayedCallback = StartCoroutine(StartCallbackAfterSeconds(maxDuration, callback));
            }
        }

    }
}