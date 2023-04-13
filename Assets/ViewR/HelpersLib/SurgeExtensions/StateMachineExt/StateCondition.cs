namespace ViewR.HelpersLib.SurgeExtensions.StateMachineExt
{
    [System.Serializable]
    public class StateCondition
    {
        public string conditionName;
        public bool conditionMet;

        public void Complete(bool complete = true) => conditionMet = complete;
    }
}