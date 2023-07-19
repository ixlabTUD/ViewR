using Pixelplacement;
using UnityEngine;
using ViewR.Core.OVR.Interactions.HandInputFix;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.SurgeExtensions.Animators.UI;

namespace ViewR.Core.OVR.UX
{
    /// <summary>
    /// Toggles the main UI
    /// 
    /// Can be toggled on and off by <see cref="ChangeMainMenuAccess(bool)"/>, see <see cref="ToggleModalUI"/> for an example.
    /// </summary>
    [DisallowMultipleComponent]
    public class ToggleMainUI : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField]
        private GameObject mainMenu;
        [SerializeField]
        private UIFadeScale mainMenuUIFadeScale;
        [SerializeField]
        private StateMachine tabStateMachine;
        [SerializeField]
        private OVRInput.Button input = OVRInput.Button.Start;

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        private void Awake()
        {
            // Subscribe
            MainMenuAccessChanged += OnMainMenuAccessChanged;
        }

        private void OnDestroy()
        {
            // Unsubscribe
            MainMenuAccessChanged -= OnMainMenuAccessChanged;
        }

        #region Ensure we disable this when requested.

        public delegate void ToggleMainMenuAccessChanged(bool allowAccess);
        public static event ToggleMainMenuAccessChanged MainMenuAccessChanged;

        public static void ChangeMainMenuAccess(bool allowAccess)
        {
            MainMenuAccessChanged?.Invoke(allowAccess);
        }

        /// <summary>
        /// Enables and Disables this component
        /// </summary>
        private void OnMainMenuAccessChanged(bool allowAccess) => this.enabled = allowAccess;

        #endregion


        private void OnEnable()
        {
            // Subscribe
            HandStartButtonWorkaround.SystemGestureCalled += ProcessStartPress;
        }

        private void OnDisable()
        {
            // Unsubscribe
            HandStartButtonWorkaround.SystemGestureCalled -= ProcessStartPress;
        }
        
        
        private void Update()
        {
            var startPressed = OVRInput.GetDown(input);
            
            if (startPressed)
                ProcessStartPress();
        }


        /// <summary>
        /// Tunnels <see cref="ProcessStartPress()"/>.
        /// </summary>
        private void ProcessStartPress(bool __, HandStartButtonWorkaround _) => ProcessStartPress();

#if UNITY_EDITOR
        [ExposeMethodInEditor]
        private void SimulateStartButtonPress() => ProcessStartPress();
#endif

        private void ProcessStartPress(bool resetToFirstTab = false)
        {
            if(mainMenu.activeInHierarchy)
                // Close the window if currently open.
                mainMenuUIFadeScale.Appear(false);
            else
            {
                // If we should restart on menu button down
                if(resetToFirstTab)
                    tabStateMachine.StartMachine();
                
                // Open the UI
                mainMenuUIFadeScale.Appear(true);
            }
        }
    }
}