using Pixelplacement;
using UnityEngine;
using ViewR.HelpersLib.SurgeExtensions.Animators.UI;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing.TutorialDisplay_OLD
{
    [RequireComponent(typeof(UIFloatAndFadeIn))]
    public class TutorialBarState : State
    {
        private UIFloatAndFadeIn _uiFloatAndFadeIn;

        private void Awake()
        {
            _uiFloatAndFadeIn = GetComponent<UIFloatAndFadeIn>();
        }

        private void OnEnable()
        {
            _uiFloatAndFadeIn.Appear(appear: true);
        }

        public void ChangeState(State state)
        {
            // Fades out and toggles next state upon being done.
            _uiFloatAndFadeIn.Appear(appear: false, true, callback: () => state.StateMachine.ChangeState(state.gameObject));
        }
    }
}