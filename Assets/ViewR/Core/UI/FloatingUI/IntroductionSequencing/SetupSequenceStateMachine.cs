using ViewR.HelpersLib.SurgeExtensions.StateMachineExt;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing
{
    /// <summary>
    /// Provides a StateMachine that also provides events to be fired where needed.
    /// <see cref="SetupSequenceEventsWrapper"/> provides a non-static interface for these Events. 
    /// </summary>
    public class SetupSequenceStateMachine : StateMachineExtended
    {
        public delegate void SetupSequenceStarted();
        /// <summary> Invoked once <see cref="SetupSequenceCompleted"/> was set to false. </summary> 
        public static SetupSequenceStarted setupSequenceStarted;

        public delegate void SetupSequenceCompleted();
        /// <summary> Invoked once <see cref="SetupSequenceCompleted"/> was set to true. </summary>
        public static SetupSequenceCompleted setupSequenceCompleted;

        public delegate void SpaceLoadingStarted();
        /// <summary> Gets invoked when the <see cref="ViewR.Managers.SpaceManager"/> loads a new scene via <see cref="ViewR.Managers.SpaceManager.LoadSceneOfCurrentSpace"/> </summary>
        public static SpaceLoadingStarted spaceLoadingStarted;
        
        public delegate void SpaceChanges();
        /// <summary> Gets invoked when the models of the current space get enabled. </summary>
        public static SpaceChanges modelsOfSpaceEnabled;
        /// <summary> Gets invoked when the models of the current space get disabled. </summary>
        public static SpaceChanges modelsOfSpaceReset;


        #region Welcome Sequence Completed

        private bool _setupSequenceComplete;

        public bool SetupSequenceComplete
        {
            get => _setupSequenceComplete;
            set
            {
                _setupSequenceComplete = value;
                if (value)
                    setupSequenceCompleted?.Invoke();
                else
                    setupSequenceStarted?.Invoke();
            }
        }

        public void RestartSequence()
        {
            SetupSequenceComplete = false;
        }

        /// <summary>
        /// Public method to complete the welcome procedure easily via the editor.
        /// </summary>
        public void Complete()
        {
            SetupSequenceComplete = true;
        }

        #endregion
        
    }
}