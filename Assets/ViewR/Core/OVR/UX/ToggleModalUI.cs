using System;
using Pixelplacement;
using UnityEditor;
using UnityEngine;
using ViewR.Core.OVR.Interactions.HandInputFix;
using ViewR.Core.UI.FloatingUI.ModalWindow;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Core.OVR.UX
{
    /// <summary>
    /// Toggles the modal UI
    /// </summary>
    public class ToggleModalUI : MonoBehaviour
    {
        [Header("Setup")]
        public bool restartSequenceOnMenuClick;
        public bool disallowAccessToMainMenuWhileActive = true;
        [SerializeField]
        private OVRInput.Button input = OVRInput.Button.Start;
        [Space]
        [Header("Modal Window Panel")]
        [Help("Leave this empty to address the global menu window! \nOnly assign values here for local UIs!", MessageType.Warning)]
        public ModalWindowPanel modalWindowConfig;

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;
        
        private StateMachine _stateMachine;

        private void OnEnable()
        {
            // Subscribe
            HandStartButtonWorkaround.SystemGestureCalled += ProcessStartPress;

            if(disallowAccessToMainMenuWhileActive)
                ToggleMainUI.ChangeMainMenuAccess(false);
        }

        private void OnDisable()
        {
            // Unsubscribe
            HandStartButtonWorkaround.SystemGestureCalled -= ProcessStartPress;
            
            if(disallowAccessToMainMenuWhileActive)
                ToggleMainUI.ChangeMainMenuAccess(true);
        }

        private void Update()
        {
            // Ensure we have the references
            if(!_stateMachine)
                _stateMachine = GetComponent<StateMachine>();
            try
            {
                if (!modalWindowConfig && ModalWindowUIController.IsInstanceRegistered)
                    modalWindowConfig = ModalWindowUIController.Instance.ModalWindowPanel;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Probably no UIController available yet... ; Exception: \n{e}".Blue().StartWithFrom(GetType()), this);
            }
            
            // Bail if not set up
            if(!_stateMachine || !modalWindowConfig) 
                return;
            
            
            var startPressed = OVRInput.GetDown(input);
            
            if (startPressed)
            {
                ProcessStartPress();
            }

            if(debugging)
            {
                if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.Hands))
                    Debug.LogWarning("Hand-Tracking Start Button DOWN".Green().StartWithFrom(GetType()), this);

                if (OVRInput.GetUp(OVRInput.Button.Start, OVRInput.Controller.Hands))
                    Debug.LogWarning("Hand-Tracking Start Button UP".Green().StartWithFrom(GetType()), this);
            }
        }


        /// <summary>
        /// Tunnels <see cref="ProcessStartPress()"/>.
        /// </summary>
        private void ProcessStartPress(bool __, HandStartButtonWorkaround _) => ProcessStartPress();

#if UNITY_EDITOR
        [ExposeMethodInEditor]
        private void SimulateStartButtonPress() => ProcessStartPress();
#endif

        private void ProcessStartPress()
        {
            if(modalWindowConfig.modalWindowBoxTransform.gameObject.activeInHierarchy)
                // Close the window if currently open.
                modalWindowConfig.Close();
            else
            {
                // If we should restart on menu button down
                if(restartSequenceOnMenuClick)
                {
                    // State machine won't re-start if the current state is already the default state. So we will re-open the window instead.
                    if(_stateMachine.defaultState == _stateMachine.currentState)
                        modalWindowConfig.ReEnableTheCurrentWindow();
                    else
                        _stateMachine.StartMachine();
                }
                else
                {
                    // either restart the machine, if there is no current state, or re-open the window.
                    if (_stateMachine.currentState == null)
                        _stateMachine.StartMachine();
                    else
                        modalWindowConfig.ReEnableTheCurrentWindow();
                }
            }
        }
    }
}