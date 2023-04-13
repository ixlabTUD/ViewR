using UnityEngine;
using UnityEngine.UI;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing.TutorialDisplay_OLD
{
    /// <summary>
    /// Allows reference to buttons.
    /// Disables all buttons on awake
    /// </summary>
    public class TutorialButtons : MonoBehaviour
    {
        public Button[] availableButtons;
        [SerializeField] private bool disableAllButtonsOnDisable;

        private void Awake()
        {
            if(availableButtons.Length != 0)
                foreach (var availableButton in availableButtons)
                    availableButton.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if(disableAllButtonsOnDisable)
                foreach (var availableButton in availableButtons)
                    availableButton.gameObject.SetActive(false);
        }
    }
}