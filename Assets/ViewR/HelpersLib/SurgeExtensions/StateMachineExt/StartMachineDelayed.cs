using System.Collections;
using Pixelplacement;
using UnityEngine;

namespace ViewR.HelpersLib.SurgeExtensions.StateMachineExt
{
    public class StartMachineDelayed : MonoBehaviour
    {
        [SerializeField]
        private StateMachine stateMachine;
        [SerializeField]
        private bool runOnEnable;

        private void OnEnable()
        {
            if (runOnEnable)
            {
                StartStateMachineDelayed();
            }
        }

        public void StartStateMachineDelayed()
        {
            StartCoroutine(DoStartStateMachineDelayed());
        }

        private IEnumerator DoStartStateMachineDelayed()
        {
            yield return new WaitForSeconds(.1f);
            stateMachine.StartMachine();
        }

        /// <summary>
        /// Convenience feature
        /// </summary>
        private void OnValidate()
        {
            if (!stateMachine)
                stateMachine = GetComponent<StateMachine>();
        }
    }
}