using System;
using ViewR.Core.UI.FloatingUI.IntroductionSequencing.FineTunedAlignment.Translational;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing.FineTunedAlignment
{
    /// <summary>
    /// Holder of events for the tuned alignment.
    /// Note: Use <see cref="TranslationalAlignmentTuningManager.AlignmentTuningManagerStarted"/> and <see cref="TranslationalAlignmentTuningManager.AlignmentTuningManagerEnded"/> if you want to know about the overall state! 
    /// </summary>
    /// <remarks>
    /// ToDo: Not yet set up with the new <see cref="ViewR.Core.Calibration.Aligner.Aligner"/>. May become obsolete?
    /// </remarks>
    public static class AlignmentEvents
    {
        /// <summary>
        /// Started alignment. In case of <see cref="TranslationalAlignmentTuningRunner"/>, this means the button was pressed.
        /// </summary>
        public static event Action AlignmentStarted;
        /// <summary>
        /// Fires as long as we do actually adjust.
        /// In case of <see cref="TranslationalAlignmentTuningRunner"/>, this means the button is being pressed.
        /// </summary>
        public static event Action AlignmentRunning;
        /// <summary>
        /// Ended alignment. In case of <see cref="TranslationalAlignmentTuningRunner"/>, this means the button was released.
        /// </summary>
        public static event Action AlignmentCompleted;
        
        public static bool AlignmentIsCompleted;

        public static void InvokeAlignmentStarted()
        {
            AlignmentIsCompleted = false;

            AlignmentStarted?.Invoke();
        }

        public static void InvokeAlignmentRunning()
        {
            AlignmentRunning?.Invoke();
        }

        public static void InvokeAlignmentCompleted()
        {
            AlignmentIsCompleted = true;
            
            AlignmentCompleted?.Invoke();
        }
    }
}