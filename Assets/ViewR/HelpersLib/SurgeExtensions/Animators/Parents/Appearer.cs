using System;
using System.Collections;
using SurgeExtensions;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.Parents
{
    /// <summary>
    /// Parent class for all tweened "appearers".
    /// </summary>
    public class Appearer : MonoBehaviour
    {
        /// <summary>
        /// Reference to stop the previous coroutine if needed.
        /// This is required to avoid callbacks that are not needed if the method gets interrupted 
        /// </summary>
        internal Coroutine previousDelayedCallback = null;

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void AppearIn() => Appear(true);
        
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void AppearOut() => Appear(false);

        
        public void AppearDynamic(bool appear) => Appear(appear);


        /// <summary>
        /// Makes the component appear or disappear.
        /// Will cancel the callback, if 
        /// </summary>
        /// <param name="appear">true: appear; false: disappear</param>
        /// <param name="invertDirection">For methods that are spatial: inverts direction of movement -- currently experimental</param>
        /// <param name="startFromCurrentValue">Should the tween start from the current value or from start/end values?</param>
        /// <param name="callback">callback, when completed.</param>
        /// <remarks><see cref="invertDirection"/> is currently experimental</remarks>
        public virtual void Appear(bool appear,
            bool invertDirection = false,
            bool startFromCurrentValue = false,
            Action callback = null)
        {
        }

        /// <summary>
        /// Starts the callback method delayed. In doing so, we allow callbacks once, an "Appearing"/Disappearing effect is finished
        ///
        /// The coroutine should be stored in <see cref="previousDelayedCallback"/> to allow the inheriting class to cancel the coroutine, should we interrupt a running animation
        /// (i.e. restart an "appear" while a "disappearing" still in progress - thus, when interrupting the disappearing, we should also cancel its callback.) 
        /// </summary>
        /// <param name="seconds">Delay</param>
        /// <param name="callback">Callback</param>
        /// <returns></returns>
        internal IEnumerator StartCallbackAfterSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);

            callback?.Invoke();
            // reset reference
            previousDelayedCallback = null;
        }

        /// <summary>
        /// Starts the callback delayed if given.
        /// </summary>
        internal void StartCallbackDelayedIfGiven(Action callback, TweenConfig tweenConfig)
        {
            if(callback != null)
            {
                var maxDuration = tweenConfig.Delay + tweenConfig.Duration;
                // StartCoroutine(StartCallbackAfterSeconds(maxDuration, callback));
                previousDelayedCallback = StartCoroutine(StartCallbackAfterSeconds(maxDuration, callback));
            }
        }
    }
}