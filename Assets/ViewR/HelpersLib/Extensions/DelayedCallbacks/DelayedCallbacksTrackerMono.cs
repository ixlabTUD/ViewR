using System;
using System.Collections;
using UnityEngine;

namespace ViewR.HelpersLib.Extensions.DelayedCallbacks
{
    /// <summary>
    /// Keeps track of Coroutine based callbacks.
    /// </summary>
    public class DelayedCallbacksTrackerMono : MonoBehaviour
    {
        /// <summary>
        /// Reference to stop the previous coroutine if needed.
        /// This is required to avoid callbacks that are not needed if the method gets interrupted 
        /// </summary>
        internal Coroutine PreviousDelayedCallback = null;


        /// <summary>
        /// Starts the callback method delayed.
        /// 
        /// The coroutine should be stored in <see cref="PreviousDelayedCallback"/> to allow the inheriting class to cancel the coroutine, should we interrupt a running animation
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
            PreviousDelayedCallback = null;
        }

        /// <summary>
        /// Starts the callback delayed if given.
        /// </summary>
        internal void StartCallbackDelayedIfGiven(Action callback, float maxDuration)
        {
            if (callback != null)
                PreviousDelayedCallback = StartCoroutine(StartCallbackAfterSeconds(maxDuration, callback));
        }
    }
}