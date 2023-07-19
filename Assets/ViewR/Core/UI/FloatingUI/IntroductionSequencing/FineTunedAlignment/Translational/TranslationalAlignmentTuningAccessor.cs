using System;
using System.ComponentModel;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.Managers;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing.FineTunedAlignment.Translational
{
    /// <summary>
    /// Accessor / Setter of the <see cref="TranslationalAlignmentTuningManager"/> and <see cref="TranslationalAlignmentTuningRunner"/>.
    /// </summary>
    public class TranslationalAlignmentTuningAccessor : MonoBehaviour
    {
        [Help("Accessor to the AlignmentTuningManager and AlignmentTuningRunner")]
        [HelpersLib.Extensions.EditorExtensions.ReadOnly.ReadOnly]
        public bool active;
        
        /// <summary>
        /// Enables the runner to <see cref="enable"/> and sets the <see cref="TranslationalAlignmentTuningManager.currentDirectionToFix"/> to <see cref="directionToFix"/>.
        /// </summary>
        public void EnableRunner(bool enable, TranslationalAlignmentTuningManager.DirectionToFix directionToFix = TranslationalAlignmentTuningManager.DirectionToFix.HorizontalX)
        {
            if (!Enum.IsDefined(typeof(TranslationalAlignmentTuningManager.DirectionToFix), directionToFix))
                throw new InvalidEnumArgumentException(nameof(directionToFix), (int) directionToFix, typeof(TranslationalAlignmentTuningManager.DirectionToFix));
            
            switch (UserConfig.Instance.Handedness)
            {
                case OVRPlugin.Handedness.LeftHanded:
                    ReferenceManager.Instance.TranslationalAlignmentTuningManagerControllerLeft.EnableRunner(enable, directionToFix);
                    break;
                case OVRPlugin.Handedness.RightHanded:
                    ReferenceManager.Instance.TranslationalAlignmentTuningManagerControllerRight.EnableRunner(enable, directionToFix);
                    break;
                case OVRPlugin.Handedness.Unsupported:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            active = enable;
        }

        #region Enable Runner in horzontal X

        /// <summary>
        /// Sets the runner run with only the controllers front.
        /// </summary>
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void EnableRunnerHorizontalX()
        {
            EnableRunner(true, TranslationalAlignmentTuningManager.DirectionToFix.HorizontalX);
        }

        /// <summary>
        /// Sets the runner run with only the controllers front.
        /// </summary>
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void DisableRunnerHorizontalX()
        {
            EnableRunner(false, TranslationalAlignmentTuningManager.DirectionToFix.HorizontalX);
        }

        /// <summary>
        /// Sets the runner run with only the controllers front.
        /// </summary>
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void ToggleRunnerHorizontalX()
        {
            EnableRunner(!TranslationalAlignmentTuningManager.Active, TranslationalAlignmentTuningManager.DirectionToFix.HorizontalX);
        }

        #endregion

        #region Enable Runner in 3DOF

        /// <summary>
        /// Sets the runner run with three degrees of freedom.
        /// </summary>  
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void EnableRunnerThreeDof()
        {
            EnableRunner(true, TranslationalAlignmentTuningManager.DirectionToFix.Free);
        }

        /// <summary>
        /// Sets the runner run with three degrees of freedom.
        /// </summary>
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void DisableRunnerThreeDof()
        {
            EnableRunner(false, TranslationalAlignmentTuningManager.DirectionToFix.Free);
        }

        /// <summary>
        /// Sets the runner run with three degrees of freedom.
        /// </summary>
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void ToggleRunnerThreeDof()
        {
            EnableRunner(!TranslationalAlignmentTuningManager.Active, TranslationalAlignmentTuningManager.DirectionToFix.Free);
        }
        
        #endregion

        #region Enable Runner with the given DirectionToFix

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void EnableRunner(TranslationalAlignmentTuningManager.DirectionToFix directionToFix = TranslationalAlignmentTuningManager.DirectionToFix.HorizontalX)
        {
            EnableRunner(true, directionToFix);
        }

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void DisableRunner(TranslationalAlignmentTuningManager.DirectionToFix directionToFix = TranslationalAlignmentTuningManager.DirectionToFix.HorizontalX)
        {
            EnableRunner(false, directionToFix);
        }


#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void ToggleRunner(TranslationalAlignmentTuningManager.DirectionToFix directionToFix = TranslationalAlignmentTuningManager.DirectionToFix.HorizontalX)
        {
            EnableRunner(!TranslationalAlignmentTuningManager.Active, directionToFix);
        }

        #endregion
    }
}
