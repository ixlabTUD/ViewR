using Pixelplacement;
using UnityEngine;
using ViewR.Core.UI.FloatingUI.IntroductionSequencing;

namespace ViewR.Managers
{
    /// <summary>
    ///     Reference manager for the sequenced introduction window.
    ///     Also keeps track whether the WelcomeSequence was completed, and fires an event, if it was completed.
    /// </summary>
    public class WelcomeSequenceReferenceManager : SingletonExtended<WelcomeSequenceReferenceManager>
    {
        [SerializeField] private Transform markerPlacementProcedure;

        [SerializeField] private SetupSequenceStateMachine setupSequenceStateMachine;

        [SerializeField] private GameObject alignment;

        [SerializeField] private GameObject markerProcedureInstructions;

        public Transform MarkerPlacementProcedure
        {
            get => markerPlacementProcedure;
            set => markerPlacementProcedure = value;
        }

        public void RestartAlignment()
        {
            setupSequenceStateMachine.StartMachine();
            setupSequenceStateMachine.SetStateByGameObject(alignment);
        }
    }
}