using UnityEngine;
using ViewR.Core.UI.FloatingUI.IntroductionSequencing.FineTunedAlignment;
using ViewR.Core.UI.FloatingUI.ModalWindow;
using ViewR.Core.UI.FloatingUI.ModalWindow.SerializablesAndReference;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing
{
    /// <summary>
    /// Ensures we can only continue once we are aligned.
    /// </summary>
    [System.Serializable, RequireComponent(typeof(InfoWindowCaller))]
    public class AlignmentTask : TutorialTask
    {
        internal override void OnEnable()
        {
            base.OnEnable();
            
            // Subscribe
            AlignmentEvents.AlignmentCompleted += AlignmentCompleted;

            // Short cut if already completed.
            if (AlignmentEvents.AlignmentIsCompleted)
            {
                AlignmentCompleted();
                NextIfCompleted();
                return;
            }
        }

        private void OnDisable()
        {
            // Unsubscribe
            AlignmentEvents.AlignmentCompleted -= AlignmentCompleted;
        }

        private void AlignmentCompleted()
        {
            Complete();
            
            ModalWindowUIController.Instance.ModalWindowPanel.SetToFollowMode(UIFollowMode.ForceFollowAndPinUponReachingTarget);
        }
    }

}