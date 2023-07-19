using System;
using System.Collections;
using Normal.Realtime;
using UnityEngine;
using ViewR.Core.Calibration.Aligner.Scripts;
using ViewR.Core.UI.FloatingUI.IntroductionSequencing.FineTunedAlignment;
using ViewR.Managers;

namespace ViewR.Core.Avatar.Hands.Scripts
{
    /// <summary>
    /// Repositions the avatar to the PlayerController - once upon joining the network, and then whenever re-aligning
    /// </summary>
    public class RepositionAvatarOnAlignmentIfOwner : MonoBehaviour
    {

        [SerializeField]
        private Transform networkedVRPlayer;

        private RealtimeView _realtimeView;
        [SerializeField]
        private float delayTime = .1f;

        private bool OwnedLocallySelf => _realtimeView.isOwnedLocallySelf;


        private void Awake()
        {
            // Get refs
            _realtimeView = GetComponent<RealtimeView>();
        }

        /// <summary>
        /// As this will only be called, once we connected, we need to initialize our position correctly.
        /// </summary>
        private void Start()
        {
            StartCoroutine(WaitForSetup());
        }

        private IEnumerator WaitForSetup()
        {
            // Wait for a bit to ensure the realtime scripts had the chance to load.
            yield return new WaitForSeconds(.05f);

            // Wait until connected and setup
            while (!_realtimeView.realtime.connected)
            {
                yield return new WaitForSeconds(.1f);
            }

            // Ensure we bail if it is not ours
            if (!OwnedLocallySelf) 
                yield break;
            
            ProcessNewAlignment();
        }

        private void OnEnable()
        {
            StartCoroutine(StartCallbackAfterSeconds(delayTime, Subscribe));
        }
        private void OnDisable()
        {
            // Unsubscribe
            StartCoroutine(StartCallbackAfterSeconds(delayTime, Unsubscribe));
        }

        private void Subscribe()
        {
            // Subscribe to new successful alignments
            AlignmentEvents.AlignmentRunning += ProcessNewAlignment;
            AlignmentEvents.AlignmentCompleted += ProcessNewAlignment;
            CalibrationEvents.CalibrationPerformed += ProcessNewAlignment;
        }

        private void Unsubscribe()
        {
            // Unsubscribe
            AlignmentEvents.AlignmentRunning -= ProcessNewAlignment;
            AlignmentEvents.AlignmentCompleted -= ProcessNewAlignment;
            CalibrationEvents.CalibrationPerformed -= ProcessNewAlignment;
        }

        private void ProcessNewAlignment(bool firstcalibration)
        {
            ProcessNewAlignment();
        }


        /// <summary>
        /// Re-aligning the networked Player with the PlayerController (which gets moved by the alignment) 
        /// </summary>
        private void ProcessNewAlignment()
        {
            // Ensure we bail if it is not ours
            if (!OwnedLocallySelf) return;
            
            networkedVRPlayer.position = ReferenceManager.Instance.PlayerControllerTransform.position;
            networkedVRPlayer.rotation = ReferenceManager.Instance.PlayerControllerTransform.rotation;
            networkedVRPlayer.localScale = ReferenceManager.Instance.PlayerControllerTransform.localScale;

        }
        
        private static IEnumerator StartCallbackAfterSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
        
    }
}
