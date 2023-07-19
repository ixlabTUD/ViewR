using UnityEngine;
using UnityEngine.Events;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing
{
    /// <summary>
    /// Exposes the static events of <see cref="SetupSequenceStateMachine"/> to unity events to be used anywhere within the editor.
    /// </summary>
    public class SetupSequenceEventsWrapper : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent setupSequenceStarted;
        [SerializeField]
        private UnityEvent setupSequenceCompleted;
        [SerializeField]
        private UnityEvent spaceLoadingStarted;
        [SerializeField]
        private UnityEvent modelsOfSpaceEnabled;
        [SerializeField]
        private UnityEvent modelsOfSpaceReset;
        
        private void OnEnable()
        {
            // Subscribe
            SetupSequenceStateMachine.setupSequenceStarted += OnSetupSequenceStarted;
            SetupSequenceStateMachine.setupSequenceCompleted += OnSetupSequenceCompleted;
            SetupSequenceStateMachine.spaceLoadingStarted += OnSpaceLoadingStarted;
            SetupSequenceStateMachine.modelsOfSpaceEnabled += OnModelsOfSpaceEnabled;
            SetupSequenceStateMachine.modelsOfSpaceReset += OnModelsOfSpaceReset;
        }

        private void OnDisable()
        {
            // Unsubscribe
            SetupSequenceStateMachine.setupSequenceStarted -= OnSetupSequenceStarted;
            SetupSequenceStateMachine.setupSequenceCompleted -= OnSetupSequenceCompleted;
            SetupSequenceStateMachine.spaceLoadingStarted -= OnSpaceLoadingStarted;
            SetupSequenceStateMachine.modelsOfSpaceEnabled -= OnModelsOfSpaceEnabled;
            SetupSequenceStateMachine.modelsOfSpaceReset -= OnModelsOfSpaceReset;
        }

        #region Match exposed Unity events with static events.

        private void OnSetupSequenceStarted() => setupSequenceStarted?.Invoke();
        private void OnSetupSequenceCompleted() => setupSequenceCompleted?.Invoke();
        private void OnSpaceLoadingStarted() => spaceLoadingStarted?.Invoke();
        private void OnModelsOfSpaceEnabled() => modelsOfSpaceEnabled?.Invoke();
        private void OnModelsOfSpaceReset() => modelsOfSpaceReset?.Invoke();

        #endregion
    }
}