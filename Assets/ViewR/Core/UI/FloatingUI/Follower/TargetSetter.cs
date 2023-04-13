using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.Extensions.EditorExtensions.ShowAttribute;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Core.UI.FloatingUI.Follower
{
    /// <summary>
    /// Sets the target and current camera on the given <see cref="LookAtFollower"/>s and <see cref="TargetFollower"/>s.
    /// </summary>
    public class TargetSetter : MonoBehaviour
    {
        [Help("Spawns a target and sets the current camera.main on the given LookAtFollower and TargetFollower.")]
        [SerializeField]
        private bool setTargetsOnStart = true;
        [SerializeField]
        private bool forceAutoPopulateOnStart = false;
        
        [FormerlySerializedAs("targetOffsetFromCamera")]
        [SerializeField]
        private Vector3 defaultTargetOffsetFromCamera = new Vector3(0, (float) -0.091, (float) 0.482);

        public bool useFixedHeight;
        [ShowIf(ActionOnConditionFail.DisableInspectorEditing, ConditionOperator.OR, nameof(useFixedHeight))]
        public float headHeightOffset = 0.5f;
        [ShowIf(ActionOnConditionFail.DisableInspectorEditing, ConditionOperator.OR, nameof(useFixedHeight))]
        public float headOffsetXZ = 0.5f;
        
        public Vector3 DefaultTargetOffsetFromCamera => defaultTargetOffsetFromCamera;

        [ShowIf(ActionOnConditionFail.DisableInspectorEditing, ConditionOperator.OR, nameof(DoNotForceAutoPopulateOnStart))]
        public LookAtFollower[] lookAtFollowers;
        [ShowIf(ActionOnConditionFail.DisableInspectorEditing, ConditionOperator.OR, nameof(DoNotForceAutoPopulateOnStart))]
        public TargetFollower[] targetFollowers;

        private Camera _mainCamera;
        private bool _initialized;
        
        
        private bool DoNotForceAutoPopulateOnStart() => !forceAutoPopulateOnStart;
        
        /// <summary>
        /// Returns a list of <see cref="TargetFollower"/>s that are currently active and whos GameObject is activeSelf and activeInHierarchy.
        /// </summary>
        public TargetFollower[] ActiveTargetFollowers
        {
            get
            {
                return targetFollowers.Where(follower =>
                {
                    GameObject o;
                    return (o = follower.gameObject).activeInHierarchy && o.activeSelf && follower.enabled;
                }).ToArray();
            }
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize(bool overwrite = false)
        {
            if (_initialized && !overwrite)
                return;

            _initialized = true;
            
            try
            {
                if (forceAutoPopulateOnStart)
                    ForceAutoPopulate();
            
                if (setTargetsOnStart)
                    SetTargets();
            }
            catch (Exception e)
            {
                Debug.LogError(e, this);
                _initialized = false;
            }
        }

        /// <summary>
        /// Sets the targets for <see cref="lookAtFollowers"/> and <see cref="targetFollowers"/> set in the inspector
        /// </summary>
        public void SetTargets()
        {
            SetTargets(lookAtFollowers, targetFollowers);
        }


        /// <summary>
        /// Sets the targets for the given values
        /// </summary>
        /// <param name="setLookAtFollowers">The <see cref="LookAtFollower"/> that should follow the new target</param>
        /// <param name="setTargetFollowers">The <see cref="TargetFollower"/> that should follow the new target</param>
        /// <param name="fetchMainCameraAgain">The main camera will only be fetched once, unless this is set to true.</param>
        /// <exception cref="Exception">No Camera.main present.</exception>
        public void SetTargets([NotNull] IEnumerable<LookAtFollower> setLookAtFollowers,
            [NotNull] IEnumerable<TargetFollower> setTargetFollowers, bool fetchMainCameraAgain = false)
        {
            if (setLookAtFollowers == null) throw new ArgumentNullException(nameof(setLookAtFollowers));
            if (setTargetFollowers == null) throw new ArgumentNullException(nameof(setTargetFollowers));

            Initialize();

            // Destroy all Targets.
            if (targetFollowers.Length > 0)
                foreach (var targetFollower in targetFollowers)
                    if(targetFollower.target)
                        Destroy(targetFollower.target.gameObject);

            // This runs very infrequent. We will still cache the value anyways.
            if (!_mainCamera || fetchMainCameraAgain)
            {
                _mainCamera = Camera.main;
                if (!_mainCamera)
                    throw new Exception("A main camera is missing! There should always be a main camera!");
            }

            var mainCameraTransform = _mainCamera.transform;

            foreach (var lookAtFollower in setLookAtFollowers)
                SetLookAtFollower(lookAtFollower);

            foreach (var targetFollower in setTargetFollowers)
                SetTargetFollower(targetFollower);
        }

        public void SetLookAtFollower(LookAtFollower lookAtFollower, bool fetchMainCameraAgain = false)
        {
            // This runs very infrequent. We will still cache the value anyways.
            if (!_mainCamera || fetchMainCameraAgain)
            {
                _mainCamera = Camera.main;
                if (!_mainCamera)
                    throw new Exception("A main camera is missing! There should always be a main camera!");
            }
            // Set:
            lookAtFollower.target = _mainCamera.transform;
            lookAtFollower.definingTargetSetter = this;

            // Keep array up to date:
            if (!lookAtFollowers.Contains(lookAtFollower))
            {
                Debug.Log($"{nameof(TargetSetter)}.{nameof(SetLookAtFollower)}: #1 {lookAtFollowers.Length} lookAtFollowers.", this);
                lookAtFollowers = ArrayExtensionMethods.Append(lookAtFollowers, lookAtFollower);
                Debug.Log($"{nameof(TargetSetter)}.{nameof(SetLookAtFollower)}: #2 {lookAtFollowers.Length} lookAtFollowers.", this);
            }
        }

        public void SetTargetFollower(TargetFollower targetFollower, bool fetchMainCameraAgain = false)
        {
            // This runs very infrequent. We will still cache the value anyways.
            if (!_mainCamera || fetchMainCameraAgain)
            {
                _mainCamera = Camera.main;
                if (!_mainCamera)
                    throw new Exception("A main camera is missing! There should always be a main camera!");
            }
            // Fetch value
            var mainCameraTransform = _mainCamera.transform;

            // Create a new Target GameObject and reference its transform again.
            targetFollower.target = new GameObject("Target")
            {
                transform =
                {
                    parent = mainCameraTransform,
                    localPosition = defaultTargetOffsetFromCamera
                }
            }.transform;
            targetFollower.cameraTransform = mainCameraTransform;
            targetFollower.definingTargetSetter = this;
            // Apply fixed height settings
            targetFollower.useFixedHeight = useFixedHeight;
            targetFollower.headHeightOffset = headHeightOffset;
            targetFollower.headOffsetXZ = headOffsetXZ;
            
            // Keep array up to date:
            if (!targetFollowers.Contains(targetFollower))
            {
                Debug.Log($"{nameof(TargetSetter)}.{nameof(SetTargetFollower)}: #1 {targetFollowers.Length} targetFollowers.", this);
                targetFollowers = ArrayExtensionMethods.Append(targetFollowers, targetFollower);
                Debug.Log($"{nameof(TargetSetter)}.{nameof(SetTargetFollower)}: #2 {targetFollowers.Length} targetFollowers.", this);
            }
        }

        /// <summary>
        /// Quick method to remove the <see cref="TargetFollower"/> and/or <see cref="LookAtFollower"/>.
        /// This way, the arrays can be kept up to date
        /// </summary>
        public void RequestRemove(TargetFollower targetFollower = null, LookAtFollower lookAtFollower = null)
        {
            if (!Application.isPlaying)
                return;

            if (targetFollower)
            {
                // Check if targetFollower is in array.
                var contains = targetFollowers.Contains(targetFollower);
                if (contains)
                {
                    // Remove follower from list and overwrite array.
                    var targetFollowersList = targetFollowers.ToList();
                    targetFollowersList.Remove(targetFollower);
                    targetFollowers = targetFollowersList.ToArray();
                }
            }

            if (lookAtFollower)
            {
                // Check if targetFollower is in array.
                var contains = lookAtFollowers.Contains(lookAtFollower);
                if (contains)
                {
                    // Remove follower from list and overwrite array.
                    var lookAtFollowerList = lookAtFollowers.ToList();
                    lookAtFollowerList.Remove(lookAtFollower);
                    lookAtFollowers = lookAtFollowerList.ToArray();
                }
            }
        }

        #region Convenience features

        [ContextMenu("ForceAutoPopulate With Children")]
        private void ForceAutoPopulate()
        {
            AutoPopulate(true);
        }
        private void AutoPopulate(bool overwriteAnyway = false)
        {
            if (targetFollowers.Length == 0 || overwriteAnyway)
            {
                targetFollowers = GetComponentsInChildren<TargetFollower>(true);
            }

            if (lookAtFollowers.Length == 0 || overwriteAnyway)
            {
                lookAtFollowers = GetComponentsInChildren<LookAtFollower>(true);
            }
        }
        
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

            AutoPopulate();
        }

        #endregion
    }
}