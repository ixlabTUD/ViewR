using System;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Serialization;
using ViewR.Core.Networking.Normcore.Voice.Settings;
using ViewR.Core.UI.FloatingUI.IntroductionSequencing.FineTunedAlignment.Translational;
using ViewR.HelpersLib.Extensions.General;
using ViewR.HelpersLib.Extensions.General.Date;

namespace ViewR.Managers
{
    /// <summary>
    ///     Stores the references of the persistent scene, granting easy access from other scripts in other scenes.
    /// </summary>
    public class ReferenceManager : SingletonExtended<ReferenceManager>
    {
        [Header("OVR Player")] [SerializeField]
        private OVRCameraRig ovrCameraRig;

        [SerializeField] private Transform markerSpawnTransformLeft;

        [SerializeField] private Transform markerSpawnTransformRight;

        [SerializeField] private GameObject ovrControllerPrefabLeft;

        [SerializeField] private GameObject ovrControllerPrefabRight;

        [SerializeField] private OVRManager ovrManager;

        [SerializeField] private Transform playerControllerTransform;

        [Header("Tracking Points")] [SerializeField]
        private Transform quest1ControllerCenterLeft;

        [SerializeField] private Transform quest1ControllerCenterRight;

        [SerializeField] private Transform quest2ControllerCenterLeft;

        [SerializeField] private Transform quest2ControllerCenterRight;

        [SerializeField] private Transform questProControllerCenterLeft;

        [SerializeField] private Transform questProControllerCenterRight;

        [SerializeField] private Transform centerEye;


        [Header("Sound")] [SerializeField] private MuteUnmuteMic muteUnmuteMic;

        [Header("Alignment")] [FormerlySerializedAs("alignmentTuningManagerControllerLeft")] [SerializeField]
        private TranslationalAlignmentTuningManager translationalAlignmentTuningManagerControllerLeft;

        [FormerlySerializedAs("alignmentTuningManagerControllerRight")] [SerializeField]
        private TranslationalAlignmentTuningManager translationalAlignmentTuningManagerControllerRight;

        private Camera _mainCamera;

        [Obsolete("This call is inefficient. Use GetMainCamera instead.")]
        public Camera MainCamera => Camera.main;

        public OVRCameraRig OvrCameraRig => ovrCameraRig;
        public Transform MarkerSpawnTransformLeft => markerSpawnTransformLeft;
        public Transform MarkerSpawnTransformRight => markerSpawnTransformRight;
        public GameObject OvrControllerPrefabLeft => ovrControllerPrefabLeft;
        public GameObject OvrControllerPrefabRight => ovrControllerPrefabRight;
        public OVRManager OvrManager => ovrManager;
        public Transform PlayerControllerTransform => playerControllerTransform;
        public Transform Quest1ControllerCenterLeft => quest1ControllerCenterLeft;
        public Transform Quest1ControllerCenterRight => quest1ControllerCenterRight;
        public Transform Quest2ControllerCenterLeft => quest2ControllerCenterLeft;
        public Transform Quest2ControllerCenterRight => quest2ControllerCenterRight;
        public Transform QuestProControllerCenterLeft => questProControllerCenterLeft;
        public Transform QuestProControllerCenterRight => questProControllerCenterRight;
        public Transform CenterEye => centerEye;
        public MuteUnmuteMic MuteUnmuteMic => muteUnmuteMic;

        public TranslationalAlignmentTuningManager TranslationalAlignmentTuningManagerControllerLeft =>
            translationalAlignmentTuningManagerControllerLeft;

        public TranslationalAlignmentTuningManager TranslationalAlignmentTuningManagerControllerRight =>
            translationalAlignmentTuningManagerControllerRight;


        public NetworkManager NetworkManager => NetworkManager.Instance;
        public PassthroughManager PassthroughManager => PassthroughManager.Instance;


        private void Start()
        {
            Console.Out.WriteLine("Starting ...".StartWithFrom(GetType()));
        }

        private void OnApplicationQuit()
        {
            Console.Out.WriteLine("Quitting ...".StartWithFrom(GetType()));

            PlayerPrefs.SetInt(PlayerPrefsAccessors.PREFS_LAST_SEEN_UNIX,
                (int)DateTime.Now.ToUniversalTime().GetUnixEpoch());

            PlayerPrefs.Save();
        }

        /// <summary>
        ///     Returns the main camera by calling Camera.main once and caching the value henceforth
        /// </summary>
        /// <param name="fetchMainCameraAgain">Do you wish the method to fetch the camera anew? Camera.main will again be queried. </param>
        public Camera GetMainCamera(bool fetchMainCameraAgain = false)
        {
            if (!_mainCamera || fetchMainCameraAgain)
                _mainCamera = Camera.main;

            return _mainCamera;
        }
    }
}