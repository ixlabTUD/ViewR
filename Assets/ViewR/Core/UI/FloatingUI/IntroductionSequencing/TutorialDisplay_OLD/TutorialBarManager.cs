using JetBrains.Annotations;
using Pixelplacement;
using TMPro;
using UnityEngine;
using ViewR.HelpersLib.SurgeExtensions.Animators.UI;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing.TutorialDisplay_OLD
{
    /// <summary>
    /// (Unfinished)
    ///
    /// Manager that allows displaying tasks in the task bar
    /// </summary>
    public class TutorialBarManager : SingletonExtended<TutorialBarManager>
    {
        [Header("Tweens")]
        [SerializeField] private UIFloatAndFadeIn uiFloatAndFadeIn;
        
        [Header("References")]
        [SerializeField] private TMP_Text tmpTaskTitle;
        [SerializeField] private TMP_Text tmpTaskDescription;
        [SerializeField] private TMP_Text tmpTaskFeedbackSuccess;
        [SerializeField] private TMP_Text tmpTaskFeedbackFailure;
        [SerializeField] private GameObject taskBar;
        [Header("Statemachine")]
        [SerializeField] private StateMachine stateMachine;
        [SerializeField] private State description;
        [SerializeField] private State success;
        [SerializeField] private State failure;

        private void Start()
        {
            taskBar.SetActive(false);
        }

        /// <summary>
        /// Displays the new task.
        /// </summary>
        public void SetNewTask([CanBeNull] TutorialTaskOLD tutorialTaskOld)
        {
            if(tutorialTaskOld != null)
            {
                tmpTaskTitle.text = tutorialTaskOld.title;
                tmpTaskDescription.text = tutorialTaskOld.description;
                tmpTaskFeedbackSuccess.text = tutorialTaskOld.descriptionSuccess;
                tmpTaskFeedbackFailure.text = tutorialTaskOld.descriptionFailure;
            }
            else
            {
                tmpTaskTitle.text = "";
                tmpTaskDescription.text = "";
                tmpTaskFeedbackSuccess.text = "";
                tmpTaskFeedbackFailure.text = "";
            }
        }
        
        /// <summary>
        /// Shows or hides the taskbar
        /// </summary>
        public void ShowTaskbar(bool show)
        {
            if(show)
            {
                taskBar.SetActive(true);
                uiFloatAndFadeIn.Appear(appear: true);
            }
            else
                // Fade it out - only if its currently visible.
                if(taskBar.activeSelf)
                    uiFloatAndFadeIn.Appear(appear: false, true, callback: () => taskBar.SetActive(false));
        }

        public void ShowTaskSuccess()
        {
            stateMachine.ChangeState(success.gameObject);
            // stateMachine.currentState.GetComponent<TutorialBarState>().ChangeState(success);
        }

        public void ShowTaskFailure()
        {
            stateMachine.ChangeState(failure.gameObject);
            // stateMachine.currentState.GetComponent<TutorialBarState>().ChangeState(failure);
        }

        public void ShowTaskDescription()
        {
            stateMachine.ChangeState(description.gameObject);
            // stateMachine.currentState.GetComponent<TutorialBarState>().ChangeState(description);
        }
        
    }
}