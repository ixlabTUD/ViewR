using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions;
using SurgeExtensions.ScriptableObjects;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ShowAttribute;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.UI
{
    /// <summary>
    /// Configures how the UI element appears!
    /// </summary>
    public class UIFloatAndFadeIn : UIAppearer
    {
        
        [Header("Refs")]
        [SerializeField] 
        private RectTransform rectTransform;
        [SerializeField, Tooltip("Only used if overwriteGoalPosOnAwake is set to false")] 
        private Vector2 goalPos;
        [SerializeField, Tooltip("Only used if tweenConfig.useDistanceInsteadOfStartPos is set to false")] 
        private Vector2 animStartPos;
        [SerializeField, Tooltip("We will overwrite the current goal pos value on awake.")] 
        private bool overwriteGoalPosOnAwake = true;

        [Header("Surge Tween")] 
        [SerializeField] 
        private TweenConfigAnchoredPositionSO tweenConfigAnchoredPositionSo;
        [SerializeField] 
        private bool alsoFadeIn = true;
        [SerializeField, ShowIf(ActionOnConditionFail.DisableInspectorEditing, ConditionOperator.OR, nameof(alsoFadeIn))] 
        private CanvasGroup canvasGroupToFade;
        [SerializeField, ShowIf(ActionOnConditionFail.DisableInspectorEditing, ConditionOperator.OR, nameof(alsoFadeIn))] 
        private bool hideCanvasGroupAlphaOnStart;
        [SerializeField, ShowIf(ActionOnConditionFail.DisableInspectorEditing, ConditionOperator.OR, nameof(alsoFadeIn))] 
        private bool fadeInWithSameConfig = true;
        [SerializeField, Tooltip("Only used if fadeInWithSameConfig is set to false"), ShowIf(ActionOnConditionFail.DisableInspectorEditing, ConditionOperator.AND, nameof(alsoFadeIn), nameof(DoNotFadeInWithSameConfig))] 
        private TweenConfigFade tweenConfigFade;

        private bool DoNotFadeInWithSameConfig() => !fadeInWithSameConfig;
        
        public TweenBase TweenBasePos { get; private set; }
        public TweenBase TweenBasesFade { get; private set; }


        private void Awake()
        {
            if (overwriteGoalPosOnAwake)
                goalPos = rectTransform.anchoredPosition;
            if (!rectTransform)
                rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            if (hideCanvasGroupAlphaOnStart && alsoFadeIn)
                canvasGroupToFade.alpha = 0;
        }

        public override void Appear(bool appear, bool invertDirection = false, bool startFromCurrentValue = false,
            Action callback = null)
        {
            base.Appear(appear, invertDirection, startFromCurrentValue, callback);
            
            var wasRunning = TweenBasePos?.Status == Tween.TweenStatus.Running;
            
            // stop any current position
            TweenBasePos?.Stop();
            TweenBasesFade?.Stop();

            if (tweenConfigAnchoredPositionSo.useDistanceInsteadOfStartPos)
                animStartPos = tweenConfigAnchoredPositionSo.CalculateStartPosition(goalPos, invertDirection);
            
            TweenBasePos = Tween.AnchoredPosition( rectTransform,
                // startValue: floatIn ? animStartPos : goalPos,
                startValue: (wasRunning || startFromCurrentValue) ? rectTransform.anchoredPosition : appear ? animStartPos : goalPos,
                endValue: appear ? goalPos : animStartPos,
                duration: tweenConfigAnchoredPositionSo.Duration,
                delay: tweenConfigAnchoredPositionSo.Delay,
                easeCurve: tweenConfigAnchoredPositionSo.AnimationCurve,
                loop: tweenConfigAnchoredPositionSo.loopType,
                obeyTimescale: tweenConfigAnchoredPositionSo.obeyTimescale);

            if(alsoFadeIn)
            {
                // Get the right config file:
                var localTweenConfigFade = fadeInWithSameConfig
                    ? (TweenConfig) tweenConfigAnchoredPositionSo
                    : tweenConfigFade;

                // Fade 
                TweenBasesFade = Tween.CanvasGroupAlpha(canvasGroupToFade,
                    startFromCurrentValue ? canvasGroupToFade.alpha : appear ? 0f : 1f,
                    appear ? 1f : 0f,
                    duration: localTweenConfigFade.Duration,
                    delay: localTweenConfigFade.Delay,
                    easeCurve: localTweenConfigFade.AnimationCurve,
                    loop: localTweenConfigFade.loopType,
                    obeyTimescale: localTweenConfigFade.obeyTimescale);
            }
            
            if(previousDelayedCallback!= null)
                StopCoroutine(previousDelayedCallback);
            
            // Start callback delayed, if given.
            if(callback != null)
            {
                // Get max value
                float maxDuration;
                if(!tweenConfigFade)
                    maxDuration = tweenConfigAnchoredPositionSo.Duration + tweenConfigAnchoredPositionSo.Delay;
                else if (!tweenConfigAnchoredPositionSo)
                    maxDuration = tweenConfigFade.Duration + tweenConfigFade.Delay;
                else
                    maxDuration = Mathf.Max(
                        tweenConfigAnchoredPositionSo.Duration + tweenConfigAnchoredPositionSo.Delay,
                        tweenConfigFade.Delay + tweenConfigFade.Duration
                    );

                previousDelayedCallback = StartCoroutine(StartCallbackAfterSeconds(maxDuration, callback));
            }
        }

    }
}