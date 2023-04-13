using Pixelplacement;
using UnityEngine;

namespace ViewR.HelpersLib.SurgeExtensions.StateMachineExt
{
    /// <summary>
    /// Could be using <see cref="StateMachineExt.StateMachineExtended"/> instead.
    /// </summary>
    [RequireComponent(typeof(StateMachine))]
    public class StateMachineController : MonoBehaviour
    {
        private StateMachine _stateMachine;

        private void Awake()
        {
            if(!_stateMachine)
                _stateMachine = GetComponent<StateMachine>();
        }

        public void NextState(bool exitIfLast = false)
        {
            _stateMachine.Next(exitIfLast);
        }

        public void PreviousState(bool exitIfFirst = false)
        {
            _stateMachine.Previous(exitIfFirst);
        }
        
        public void SetStateByString(string newState)
        {
            _stateMachine.ChangeState(newState);
        }
        public void SetStateByGameObject(GameObject newState)
        {
            _stateMachine.ChangeState(newState);
        }
        public void SetStateByInt(int newState)
        {
            _stateMachine.ChangeState(newState);
        }

        
    }
}