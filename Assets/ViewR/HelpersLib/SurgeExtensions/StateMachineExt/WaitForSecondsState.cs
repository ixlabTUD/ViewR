using System.Collections;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.HelpersLib.SurgeExtensions.StateMachineExt
{
    /// <summary>
    /// A StateMachine state that will wait for <see cref="seconds"/> and then start the next state automatically
    /// </summary>
    public class WaitForSecondsState : StateExtended
    {
        [Help("This will auto-start the next state after the given amount of seconds.")]
        [SerializeField]
        private float seconds;

        protected float Seconds => seconds;

        protected Coroutine CurrentCoroutine;

        protected virtual void OnEnable()
        {
            CurrentCoroutine = StartCoroutineIfActive(NextStateInSeconds());
        }

        protected virtual void OnDisable()
        {
            StopCoroutine(CurrentCoroutine);
        }

        protected virtual IEnumerator NextStateInSeconds()
        {
            yield return new WaitForSeconds(seconds);
    
            // If this state is still active:
            if(this.gameObject.activeInHierarchy)
                Next(true);
        }
    }
}
