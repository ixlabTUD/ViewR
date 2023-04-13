using Pixelplacement;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing.TutorialDisplay_OLD
{
    public class TutorialTaskManagerOLD : StateMachine
    {
        // ReSharper disable once InconsistentNaming
        [SerializeField, ReadOnly] private TutorialTaskOLD currentTutorialTaskOld;

        /// <summary>
        /// The currently active task
        /// </summary>
        public TutorialTaskOLD CurrentTutorialTaskOld
        {
            get => currentTutorialTaskOld;
            set
            {
                currentTutorialTaskOld = value;
                TutorialBarManager.Instance.SetNewTask(value);
            }
        }
        
        /// <summary>
        /// You could also just set <see cref="CurrentTutorialTaskOld"/>.
        /// </summary>
        /// <param name="newTaskOld"></param>
        public void SetNewTask(TutorialTaskOLD newTaskOld)
        {
            CurrentTutorialTaskOld = newTaskOld;
        }

        public void DisplayTask(bool show) => TutorialBarManager.Instance.ShowTaskbar(show);
        
        public void StartTask()
        {
            if(CurrentTutorialTaskOld != null)
                CurrentTutorialTaskOld.StartTask();
        }
        public void CompleteTask()
        {
            if(CurrentTutorialTaskOld != null)
                CurrentTutorialTaskOld.Complete();
        }

        public void ShowSuccess()
        {
            TutorialBarManager.Instance.ShowTaskSuccess();
        }
        public void ShowFailure()
        {
            TutorialBarManager.Instance.ShowTaskFailure();
        }
        public void ShowDescription()
        {
            TutorialBarManager.Instance.ShowTaskDescription();
        }
    }
}