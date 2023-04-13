using ViewR.Core.UI.FloatingUI.IntroductionSequencing.FineTunedAlignment.Translational;
using ViewR.Core.UI.FloatingUI.NotificationSystem.CoreSystem;

namespace ViewR.Core.UI.FloatingUI.NotificationSystem.Notifications
{
    /// <summary>
    /// A method reacting to <see cref="TranslationalAlignmentTuningManager"/>s <see cref="TranslationalAlignmentTuningManager.AlignmentTuningManagerStarted"/> and <see cref="TranslationalAlignmentTuningManager.AlignmentTuningManagerEnded"/> to show the user a notification.
    /// Additionally allows the user to click a button to stop it.
    /// </summary>
    public class TranslationalAlignmentTuningActiveNotification : NotificationPanelCaller
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            
            // Subscribe
            TranslationalAlignmentTuningManager.AlignmentTuningManagerStarted += HandleTuningStarted;
            TranslationalAlignmentTuningManager.AlignmentTuningManagerEnded += HandleTuningEnded;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // Unsubscribe
            TranslationalAlignmentTuningManager.AlignmentTuningManagerStarted -= HandleTuningStarted;
            TranslationalAlignmentTuningManager.AlignmentTuningManagerEnded -= HandleTuningEnded;
        }

        /// <summary>
        /// Configures the a button to stop the alignment and shows the notification
        /// </summary>
        private void HandleTuningStarted(TranslationalAlignmentTuningManager translationalAlignmentTuningManager)
        {
            // Show
            ShowWindow();
        }

        /// <summary>
        /// Removes event and closes the window.
        /// </summary>
        private void HandleTuningEnded(TranslationalAlignmentTuningManager translationalAlignmentTuningManager)
        {
            // Close
            CloseWindow();
        }
    }
}
