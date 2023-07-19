using System.Collections;
using UnityEngine;
using ViewR.Core.UI.FloatingUI.ModalWindow;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.SurgeExtensions.StateMachineExt;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing
{
    /// <summary>
    /// A StateMachine state that will wait for <see cref="WaitForSecondsState.seconds"/> and then start the next state automatically
    /// </summary>
    public class WaitForSecondsWithProgressState : WaitForSecondsState
    {
        [Help("Waits for seconds, displays the progress in the Modal UI and automatically goes to the \"Next State\".")]
        [SerializeField]
        private InfoWindowCaller infoWindowCaller;

        protected override IEnumerator NextStateInSeconds()
        {
            var timer = 0.0f;
            var incompleteShowWindow = true;

            // Show the window
            infoWindowCaller.ShowWindow(callback: () => incompleteShowWindow = false);
            
            while(incompleteShowWindow)
            {
                // spin
                yield return null;
            }
            
            // Configure the slider
            var progressbarManager = ModalWindowUIController.Instance.ModalWindowPanel.progressBarManager;
            // Get the fader and force it to be invisible
            progressbarManager.ActivateAndFadeFieldsIn(true);
            progressbarManager.gameObject.SetActive(true);

            while (timer < Seconds)
            {
                timer += Time.deltaTime;

                var progress = timer / Seconds;

                progressbarManager.Slider.value = progress; // will display current progress
                progressbarManager.TextField.text = $"Loading {(progress * 100f):##0.00}%";
                
                // wait.
                yield return null;
            }

            // If this state is still active:
            if (this.gameObject.activeInHierarchy)
                Next(true);
        }
    }
}