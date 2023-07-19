using System;
using System.Collections;
using SurgeExtensions;
using UnityEngine;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.Movers.Parents
{
    /// <summary>
    /// Parent class for all tweened "mover"s.
    /// </summary>
    public class Mover : MonoBehaviour
    {
        public bool triggerOnEnable;
        public bool restoreOnDisable;
        
        /// <summary>
        /// Reference to stop the previous coroutine if needed.
        /// This is required to avoid callbacks that are not needed if the method gets interrupted 
        /// </summary>
        internal Coroutine previousDelayedCallback = null;

        
        public virtual void Move(bool moveIn, bool invertDirection = false, Action callback = null)
        {
        }
        
        public virtual void Restore(Action callback = null)
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
        internal void StartCallbackDelayedIfGiven(Action callback, TweenConfig tweenConfig, float buffer = 0f)
        {
            if(callback != null)
            {
                var maxDuration = tweenConfig.Delay + tweenConfig.Duration + buffer;
                // StartCoroutine(StartCallbackAfterSeconds(maxDuration, callback));
                previousDelayedCallback = StartCoroutine(StartCallbackAfterSeconds(maxDuration, callback));
            }
        }
    }
}