using UnityEngine;
using UnityEngine.UI;
using ViewR.Core.OVR.ControllerCenterPoint;
using ViewR.HelpersLib.SurgeExtensions.Animators.UI.Color;
using ViewR.HelpersLib.Universals.UI.Toggle;
using ViewR.HelpersLib.Utils.ToggleObjects;

namespace ViewR.Tools.CSVWriter
{
    /// <summary>
    /// Processes toggle clicks
    /// </summary>
    public class DataRecorderManager : MonoBehaviour
    {
        [SerializeField]
        private ToggleGroupController toggleGroupController;
        [SerializeField]
        private UIColorTween uiColorTweenStopButton;
        [SerializeField]
        private Button stopButton;
        [SerializeField]
        private ObjectsToToggle objectsToToggleWithRecordingState;

        public bool CurrentlyRecording { get; private set; }

        /// <summary>
        /// Disables all toggles and sets the title of the path to the chosen buttons text.
        /// </summary>
        public void ProcessToggleClick(Toggle findTextInChildToggle/*Toggle exceptThisToggle*/)
        {
            // toggleGroupController.DeactivateInteractiveOfAllTogglesExcept(exceptThisToggle);
            toggleGroupController.DeactivateInteractiveOfAllToggles();
            ProcessTogglePress();
        }

        public void ProcessToggleClick()
        {
            toggleGroupController.DeactivateInteractiveOfAllToggles();
            ProcessTogglePress();
        }
        
        public void ProcessStop()
        {
            toggleGroupController.ActivateInteractiveOfAllToggles();
            
            // Update Stop Button
            ProcessRecordingChange(false);
        }

        private void ProcessTogglePress()
        {
            // Update Stop Button
            ProcessRecordingChange(true);
        }

        private void ProcessRecordingChange(bool startedRecording)
        {
            // Tween Color
            uiColorTweenStopButton.Appear(startedRecording);
                
            // Set Stop Button
            stopButton.interactable = startedRecording;
            
            // Toggle placement hint
            if (!ControllerCenterPointManager.IsInstanceRegistered)
                Debug.LogError($"There is no {nameof(ControllerCenterPointManager)}. There should probably be one though.");
            else
                ControllerCenterPointManager.Instance.ShowHints(startedRecording);
            
            // Turn on/off the objects to toggle.
            objectsToToggleWithRecordingState.Enable(startedRecording);

            // Set bool
            CurrentlyRecording = startedRecording;
        }
    }
}
