using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.UI.Color
{
    public class UIColorTween : UIAppearer
    {
        [Header("Surge Tween")] 
        [SerializeField]
        private TweenConfig colorTweenConfig;
        
        [Header("Setup")]
        [SerializeField] 
        private bool applyOnEnable;
        [SerializeField]
        private Graphic image;
        [SerializeField]
        private UnityEngine.Color targetColor;
        
        private TweenBase _colorFadeTweenBase;
        private UnityEngine.Color _defaultColor;
        private UnityEngine.Color _defaultTargetColor;


        private void Awake()
        {
            _defaultColor = image.color;
            _defaultTargetColor = targetColor;
        }

        private void OnEnable()
        {
            if (applyOnEnable)
            {
                Appear(true);
            }
        }

        /// <summary>
        /// Only overwrites the <see cref="targetColor"/>, does not start <see cref="Appear"/>.
        /// </summary>
        public void SetTargetColor(UnityEngine.Color newTargetColor)
        {
            targetColor = newTargetColor;
        }

        /// <summary>
        /// Only overwrites the <see cref="targetColor"/>, does not start <see cref="Appear"/>.
        /// </summary>
        public void RestoreDefaultTargetColor()
        {
            targetColor = _defaultTargetColor;
        }
        
        /// <summary>
        /// Overwrites the <see cref="targetColor"/> and starts <see cref="Appear"/>
        /// </summary>
        public void AppearWithTargetColor(UnityEngine.Color newTargetColor, bool appear, bool invertDirection = false, 
            bool startFromCurrentValue = false, Action callback = null)
        {
            SetTargetColor(newTargetColor);
            Appear(appear, invertDirection, startFromCurrentValue, callback);
        }
        
        public override void Appear(bool appear, bool invertDirection = false, bool startFromCurrentValue = false, 
            Action callback = null)
        {
            base.Appear(appear, invertDirection, startFromCurrentValue, callback);
            
            // Stop current animations
            _colorFadeTweenBase?.Stop();
            
            // Start a new one
            _colorFadeTweenBase = Tween.Color(image,
                startValue: startFromCurrentValue ? image.color : appear ? _defaultColor : targetColor,
                appear ? targetColor : _defaultColor,
                colorTweenConfig.Duration,
                colorTweenConfig.Delay,
                colorTweenConfig.AnimationCurve,
                colorTweenConfig.loopType,
                obeyTimescale: colorTweenConfig.obeyTimescale,
                completeCallback: callback);
            
            
            // Stop previous callback if there is one
            if(previousDelayedCallback!= null)
                StopCoroutine(previousDelayedCallback);
            
            // Start callback delayed, if given.
            if(callback != null)
            {
                var maxDuration = colorTweenConfig.Delay + colorTweenConfig.Duration;
                previousDelayedCallback = StartCoroutine(StartCallbackAfterSeconds(maxDuration, callback));
            }
        }

        private void RestoreInitialButtonColor()
        {
            Appear(false);
        }


        /// <summary>
        /// Convenience feature.
        /// </summary>
        private void OnValidate()
        {
            image = GetComponent<Image>();
        }
    }
}