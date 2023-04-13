using Pixelplacement;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using ViewR.Core.UI.FloatingUI.ModalWindow;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing
{
    [System.Serializable, RequireComponent(typeof(InfoWindowCaller))]
    public class TutorialTask : State
    {
        [Header("Config")]
        [SerializeField] private bool showInfoBarOnEnable = true;
        [SerializeField, Tooltip("If left empty, will be auto-populated on awake.")] internal InfoWindowCaller infoWindowCaller;
        
        [Header("Task")]
        [Help("Use the InfoWindowCaller to configure title, image and description.", MessageType.Info)]
        public string descriptionSuccess;
        public string descriptionFailure;
        public UnityEvent taskStarted;
        public UnityEvent taskCompleted;

        public bool TaskIsActive { get; private set; }
        public bool Completed { get; private set; }


        internal virtual void Awake()
        {
            // Get refs
            if(!infoWindowCaller)
                infoWindowCaller = GetComponent<InfoWindowCaller>();
        }

        internal virtual void OnEnable()
        {
            if(showInfoBarOnEnable)
                infoWindowCaller.ShowWindow();
                // _stateMachine.DisplayTask(infoWindowCaller.modalWindowConfig);
        }

        public void StartTask()
        {
            taskStarted?.Invoke();
            TaskIsActive = true;
            Completed = false;
        }
        
        public void Complete()
        {
            taskCompleted?.Invoke();
            TaskIsActive = false;
            Completed = true;
        }

        public void NextState()
        {
            Next(true);
        }
        public void PreviousState()
        {
            Previous(false);
        }
        
        
        /// <summary>
        /// Only allows the user to continue to next, <see cref="Completed"/> == false.
        /// </summary>
        public void NextIfCompleted()
        {
            if(!Completed)
            {
                // Not permitted!
                ModalWindowUIController.Instance.ModalWindowPanel.Shake();
                ModalWindowUIController.Instance.ModalWindowAudioSource.PlayOneShot(ModalWindowUIController.Instance.ErrorSound);
                return;
            }

            // Granted!
            ModalWindowUIController.Instance.ModalWindowPanel.Close();
            NextState();
        }
    }

}