using System;
using System.ComponentModel;
using Oculus.Interaction;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing.FineTunedAlignment.Translational
{
    /// <summary>
    /// Steers <see cref="TranslationalAlignmentTuningRunner"/>
    /// By setting <see cref="currentDirectionToFix"/>, we can define which direction to modify.
    /// </summary>
    [RequireComponent(typeof(ControllerSelector))]
    public class TranslationalAlignmentTuningManager : MonoBehaviour
    {
        public enum DirectionToFix
        {
            HorizontalX,
            HorizontalZ,
            Vertical,
            Free
        }

        [Help(
            "The direction to modify. To use this with the Oculus Controller front, use the ControllerOffset in the AlitnmentTuningRunner, and set its rotation to (319.5,0,0), and set CurrentDirectionToFix to HorizontalX")]
        public DirectionToFix currentDirectionToFix = DirectionToFix.HorizontalX;

        public ControllerSelector ControllerSelector { get; private set; }

        public static bool Active;

        #region Events

        public delegate void PropertyChangedHandler(
            TranslationalAlignmentTuningManager translationalAlignmentTuningManager);

        /// <summary>
        /// Was the <see cref="TranslationalAlignmentTuningRunner"/> enabled? 
        /// </summary>
        public static event PropertyChangedHandler AlignmentTuningManagerStarted;

        /// <summary>
        /// Was the <see cref="TranslationalAlignmentTuningRunner"/> disabled?
        /// </summary>
        public static event PropertyChangedHandler AlignmentTuningManagerEnded;

        #endregion


        #region Unity Methods

        private void Awake()
        {
            ControllerSelector = GetComponent<ControllerSelector>();
            GetComponent<TranslationalAlignmentTuningRunner>().enabled = false;
        }

        private void OnEnable()
        {
            // Ensure there is only one tweaking manager active
            AlignmentTuningDisabler.TranslationalAlignmentManagersShouldStop += DisableRunner;
        }

        private void OnDisable()
        {
            AlignmentTuningDisabler.TranslationalAlignmentManagersShouldStop -= DisableRunner;
            
            // Ensure we do stop
            DisableRunner();
        }

        #endregion

        /// <summary>
        /// Enables the <see cref="TranslationalAlignmentTuningRunner"/>.
        /// Will be called when the <see cref="ControllerSelector"/> has been pressed.
        /// </summary>
        private void SelectionStarted()
        {
            GetComponent<TranslationalAlignmentTuningRunner>().enabled = true;
        }

        /// <summary>
        /// Disables the <see cref="TranslationalAlignmentTuningRunner"/>.
        /// Will be called when the <see cref="ControllerSelector"/> has been released.
        /// </summary>
        private void SelectionEnded()
        {
            GetComponent<TranslationalAlignmentTuningRunner>().enabled = false;
        }

        /// <summary>
        /// Enables / Disables this scripts logic
        /// </summary>
        public void EnableRunner(bool enable, DirectionToFix directionToFix = DirectionToFix.HorizontalX)
        {
            if (!Enum.IsDefined(typeof(TranslationalAlignmentTuningManager.DirectionToFix), directionToFix))
                throw new InvalidEnumArgumentException(nameof(directionToFix), (int) directionToFix,
                    typeof(TranslationalAlignmentTuningManager.DirectionToFix));

            if (Active == enable)
            {
                Debug.Log($"{nameof(TranslationalAlignmentTuningManager)}.{nameof(EnableRunner)}: Active already == enable. Bailing.", this);
                return;
            }
            
            switch (enable)
            {
                case true:
                    // Set value
                    currentDirectionToFix = directionToFix;
                    // Subscribe
                    ControllerSelector.WhenSelected += SelectionStarted;
                    ControllerSelector.WhenUnselected += SelectionEnded;
                    // Invoke
                    AlignmentTuningManagerStarted?.Invoke(this);
                    break;
                case false:
                    // Unsubscribe
                    ControllerSelector.WhenSelected -= SelectionStarted;
                    ControllerSelector.WhenUnselected -= SelectionEnded;
                    // Invoke
                    AlignmentTuningManagerEnded?.Invoke(this);
                    // Ensure it is turned off.
                    GetComponent<TranslationalAlignmentTuningRunner>().enabled = false;
                    break;
            }

            Active = enable;
        }

        #region Simplified versions of enabling the runner.

        /// <summary>
        /// Enables / Disables this scripts logic
        /// </summary>
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void EnableRunner()
        {
            EnableRunner(true);
        }

        /// <summary>
        /// Enables / Disables this scripts logic
        /// </summary>
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void DisableRunner()
        {
            EnableRunner(false);
        }

        /// <summary>
        /// Tunnels <see cref="DisableRunner()"/>
        /// </summary>
        public void DisableRunner(TranslationalAlignmentTuningManager _)
        {
            DisableRunner();
        }
        
        #endregion

    }
}