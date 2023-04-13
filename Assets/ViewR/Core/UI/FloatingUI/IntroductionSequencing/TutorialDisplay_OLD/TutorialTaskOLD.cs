using Pixelplacement;
using UnityEngine;
using UnityEngine.Events;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing.TutorialDisplay_OLD
{
    [System.Serializable]
    public class TutorialTaskOLD : State
    {
        [Header("Config")]
        [SerializeField] private bool showInfoBarOnStart = false;
        [Header("Task")]
        public string title;
        public string description;
        public string descriptionSuccess;
        public string descriptionFailure;
        public UnityEvent taskStarted;
        public UnityEvent taskCompleted;

        private TutorialTaskManagerOLD _stateMachine;
        
        
        public bool IsActive { get; private set; }

        private void OnEnable()
        {
            // set this to the current task
            _stateMachine = (TutorialTaskManagerOLD) this.StateMachine;
            _stateMachine.CurrentTutorialTaskOld = this;
            if(showInfoBarOnStart)
                _stateMachine.DisplayTask(true);
        }

        public void StartTask()
        {
            taskStarted?.Invoke();
            IsActive = true;
        }
        
        public void Complete()
        {
            taskCompleted?.Invoke();
            IsActive = false;
        }

        public void NextState()
        {
            Next();
        }
    }

}