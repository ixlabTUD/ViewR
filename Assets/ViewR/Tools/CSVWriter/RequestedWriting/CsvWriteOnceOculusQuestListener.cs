using UnityEngine;
using ViewR.Core.OVR.Interactions.Input;
using ViewR.Core.UI.FloatingUI.NotificationSystem.CoreSystem;
using ViewR.Tools.CSVWriter.Accessors;

namespace ViewR.Tools.CSVWriter.RequestedWriting
{
    /// <summary>
    /// A class that listens to the Quests "one"-buttons on both controllers and processes their input by querying <see cref="csvWriterRequester"/> to <see cref="CsvWriterRequester.RequestToWriteOnce"/>.
    /// </summary>
    public class CsvWriteOnceOculusQuestListener : NotificationPanelCaller
    {
        [Space]
        [Header("CSV References")]
        [SerializeField]
        private CsvWriterRequester csvWriterRequester;
        
        private bool _running;
        
        private void Update()
        {
            // Bail if not on controllers
            if (!OVRInputState.ControllersActive)
                return;

            // Process
            CheckAndProcessButtonInput();
        }


        /// <summary>
        /// Checks for user input and processes press or release of button.
        /// </summary>
        private void CheckAndProcessButtonInput()
        {
            if ( 
                // Currently not running
                !_running && 
                (
                    // Button on L was already pressed and R was just clicked 
                    OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch) &&
                    OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch) ||
                    // OR Button on R was already pressed and L was just clicked 
                    OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch) &&
                    OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch)
                )
            )
            {
                // Run logic for when buttons where pressed
                ProcessBothButtonsPressed();
                
                // Set flag
                _running = true;
            }

            if (
                // Currently running
                _running && 
                (
                    // And None of the controller buttons is pressed anymore 
                    !OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch) &&
                    !OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch)
                )
            )
            {
                // Run logic for when buttons where released
                ProcessBothButtonsReleased();
                
                // Set flag
                _running = false;
            }
        }

        private void ProcessBothButtonsPressed()
        {
            // Do Write
            csvWriterRequester.RequestToWriteOnce();
            
            // Show notification
            ShowWindow();
        }

        private void ProcessBothButtonsReleased()
        {
        }
    }
}
