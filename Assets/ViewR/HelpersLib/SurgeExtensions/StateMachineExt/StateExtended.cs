using Pixelplacement;

namespace ViewR.HelpersLib.SurgeExtensions.StateMachineExt
{
    public class StateExtended : State
    {
        public void NextWithoutReturnType(bool exitIfLast = false)
        {
            StateMachine.Next(exitIfLast);
        }
    }
}