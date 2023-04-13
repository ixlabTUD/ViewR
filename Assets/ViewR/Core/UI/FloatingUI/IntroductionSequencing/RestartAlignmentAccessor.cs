using UnityEngine;
using ViewR.Managers;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing
{
    public class RestartAlignmentAccessor : MonoBehaviour
    {
        public void RestartAlignment()
        {
            WelcomeSequenceReferenceManager.Instance.RestartAlignment();
        }
    }
}
