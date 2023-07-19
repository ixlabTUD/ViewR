using Pixelplacement;
using UnityEngine;

namespace ViewR.HelpersLib.SurgeExtensions.StateMachineExt
{
    /// <summary>
    /// Extends the state machine to contain methods that can be easily called from buttons without code.
    /// </summary>
    public class StateMachineExtended : StateMachine
    {
        public void SetStateByString(string newState)
        {
            ChangeState(newState);
        }
        public void SetStateByGameObject(GameObject newState)
        {
            ChangeState(newState);
        }
        public void SetStateByInt(int newState)
        {
            ChangeState(newState);
        }
        
        public void NextState(bool exitIfLast = false)
        {
            Next(exitIfLast);
        }

        public void PreviousState(bool exitIfFirst = false)
        {
            Previous(exitIfFirst);
        }
    }
}
