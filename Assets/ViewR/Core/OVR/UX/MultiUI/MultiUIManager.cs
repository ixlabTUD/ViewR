using UnityEngine;
using ViewR.Core.UI.FloatingUI.Follower;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Core.OVR.UX.MultiUI
{
    /// <summary>
    /// If a window is already active, push the other one one up.
    /// </summary>
    public class MultiUIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TargetSetter targetSetter;

        [Header("Tweaking")]
        [SerializeField]
        private Vector3 pushBackOffset = new Vector3(0.0f, 0.25f, 0.15f);

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;


        private int _activeWindows;

        #region Events

        public delegate void ModalWindowFollowingChanged(bool active, TargetFollower targetFollower);

        public event ModalWindowFollowingChanged WindowFollowingChanged;

        public void FireWindowFollowingChanged(bool active, TargetFollower targetFollower)
        {
            WindowFollowingChanged?.Invoke(active, targetFollower);
        }

        #endregion

        private void OnEnable()
        {
            // Subscribe
            WindowFollowingChanged += OnWindowFollowingChanged;
        }

        private void OnDisable()
        {
            // Unsubscribe
            WindowFollowingChanged -= OnWindowFollowingChanged;
        }


        private void OnWindowFollowingChanged(bool active, TargetFollower targetFollower)
        {
            if (debugging)
                Debug.Log(
                    $"OnWindowFollowingChanged was called. Received: {active} from {targetFollower.gameObject.name}."
                        .StartWithFrom(GetType()), this);

            ProcessValue(active, targetFollower);
        }

        private void ProcessValue(bool active, TargetFollower targetFollower)
        {
            if(active)
                MakeMeActive(targetFollower);
            else
                AWindowWasClosed(targetFollower);
        }

        private void AWindowWasClosed(TargetFollower targetFollower)
        {
            foreach (var follower in targetSetter.ActiveTargetFollowers)
            {
                if (targetFollower == follower)
                {
                    Debug.LogWarning($"Skipping {targetFollower.gameObject.name}, as it was closed.".StartWithFrom(GetType()), this);
                    continue;
                }
                
                if (debugging)
                    Debug.Log($"Pushing forward {follower.gameObject.name}".StartWithFrom(GetType()), this);
                
                follower.PushMeForward(pushBackOffset);
            }
        }

        public void MakeMeActive(TargetFollower supposedToBeFocussedFollower)
        {
            // For each active window that has the follower turned on:
            // Up vertical by "one" step size
            
            // So: Let's count the active ones except this one.
            foreach (var follower in targetSetter.ActiveTargetFollowers)
            {
                // Skip the newcomer
                if (follower == supposedToBeFocussedFollower)
                {
                    if(debugging)
                        Debug.Log($"Skipping {follower.gameObject.name}, as it's the newcomer.".StartWithFrom(GetType()), this);
                    continue;
                }

                var previouslyInactive =
                    supposedToBeFocussedFollower.PosInSequence == TargetFollower.INITIAL_POSITION_VALUE;
                
                // Skip the ones higher up, IF we called from an already active window.
                if(!previouslyInactive)
                    if(follower.PosInSequence > supposedToBeFocussedFollower.PosInSequence)
                    {
                        if(debugging)
                            Debug.Log($"Skipping {follower.gameObject.name}, as it's higher up than the newcomer.".StartWithFrom(GetType()), this);
                        continue;
                    }
                
                if (debugging)
                    Debug.Log($"Pushing back {follower.gameObject.name}".StartWithFrom(GetType()), this);
                
                follower.PushMeBack(pushBackOffset);
            }

            if (debugging)
                Debug.Log($"Set to main: {supposedToBeFocussedFollower.gameObject.name}".StartWithFrom(GetType()), this);

            supposedToBeFocussedFollower.SetMeToMain();
        }


        #region Previous Method

        private readonly float _addedStepSize = 0.3f;
        private readonly float _addedStepSizeZ = -.2f;
        [System.Obsolete("Use MakeMeActive instead!")]
        private void ProcessNewValue(bool active, TargetFollower newTargetFollower)
        {
            // If activated: move it up.
            if (active)
            {
                // For each active window that has the follower turned on:
                // Up vertical by "one" step size
                var countOtherActiveFollowers = 0;
                foreach (var follower in targetSetter.ActiveTargetFollowers)
                {
                    // Skip the newcomer
                    if (follower == newTargetFollower) continue;

                    countOtherActiveFollowers++;
                }

                if (debugging)
                    Debug.Log($"Found {countOtherActiveFollowers} other active followers.".StartWithFrom(GetType()), this);

                // Adjust positions
                if (countOtherActiveFollowers > 0)
                {
                    var target = newTargetFollower.target;
                    var localPosition = target.localPosition;
                    localPosition = new Vector3(
                        localPosition.x,
                        localPosition.y + _addedStepSize * countOtherActiveFollowers,
                        localPosition.z + _addedStepSizeZ * countOtherActiveFollowers);
                    target.localPosition = localPosition;
                }
            }
            // If deactivated: move everyone down
            else
            {
                foreach (var targetFollower in targetSetter.targetFollowers)
                {
                    if (targetFollower == newTargetFollower)
                    {
                        if (debugging)
                            Debug.Log($"Reset Position on {newTargetFollower}.".StartWithFrom(GetType()), this);

                        // Reset
                        targetFollower.target.localPosition = targetSetter.DefaultTargetOffsetFromCamera;
                        continue;
                    }

                    // Reduce all other active followers.
                    // Thus: Skip inactive ones
                    if (!targetFollower.gameObject.activeInHierarchy || !targetFollower.gameObject.activeSelf ||
                        !targetFollower.enabled)
                    {
                        if (debugging)
                            Debug.Log($"Skipping {targetFollower} due to inactivity.".StartWithFrom(GetType()), this);

                        continue;
                    }

                    // And then adjust their target.
                    var dist = targetFollower.target.localPosition - targetSetter.DefaultTargetOffsetFromCamera;

                    // If they are not the same, step one down.
                    if (dist.magnitude > 0.1f)
                    {
                        var tmpLocalPosition = newTargetFollower.target.localPosition;
                        targetFollower.target.localPosition = new Vector3(
                            tmpLocalPosition.x,
                            tmpLocalPosition.y - _addedStepSize,
                            tmpLocalPosition.z - _addedStepSizeZ);

                        if (debugging) Debug.Log($"Adjusted {targetFollower}.".StartWithFrom(GetType()), this);
                    }
                    else
                    {
                        if (debugging)
                            Debug.Log($"Did not have to adjust {targetFollower}.".StartWithFrom(GetType()), this);
                    }
                }
            }
        }

        #endregion
    }
}