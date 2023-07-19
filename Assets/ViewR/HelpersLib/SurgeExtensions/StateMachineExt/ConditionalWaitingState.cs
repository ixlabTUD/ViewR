using System.Collections.Generic;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.HelpersLib.SurgeExtensions.StateMachineExt
{
    /// <summary>
    /// A StateMachine state that will wait all conditions to be met before starting the next state
    /// </summary>
    public class ConditionalWaitingState : StateExtended
    {
        [Help("This will wait for all conditions to be met.\nOnly continues when all conditions are met.")]
        public List<StateCondition> stateConditions;

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        private bool _completed;

        private void Update()
        {
            if (!this.gameObject.activeSelf)
                return;

            // Check if any of the given conditions is not met.
            var foundFalseValue = false;
            if (stateConditions.Count > 0)
                foreach (var stateCondition in stateConditions)
                {
                    if (stateCondition.conditionMet) continue;

                    // Found a not fulfilled condition!
                    foundFalseValue = true;
                    // Skip remaining loop if there is a not-met condition.
                    break;
                }

            // Don't continue, if there is a not-fulfilled condition!
            if (foundFalseValue)
            {
                if (debugging) Debug.Log("Found a false value (a not met condition).", this);
                return;
            }
            else
            {
                ProcessComplete();
            }
        }

        /// <summary>
        /// Processes complete.
        /// Contains a flag to ensure this is not run twice before we disable this game object (which could lead to state-skipping.).
        /// </summary>
        private void ProcessComplete()
        {
            // Ensure we are not yet completed and that this GameObject (aka state) is still active.
            if (!_completed && this.gameObject.activeSelf)
                Next();
            else
                Debug.LogWarning("Already completed. Bailing.", this);

            // Change flag.
            _completed = true;
        }
    }
}