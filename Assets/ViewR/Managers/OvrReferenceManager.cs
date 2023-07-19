using System;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using ViewR.Core.Calibration;

namespace ViewR.Managers
{
    /// <summary>
    /// An alternative to Oculus' <see cref="OculusSampleFramework.HandsManager"/>.
    /// Primarily an accessor class.
    /// </summary>
    public class OvrReferenceManager : SingletonExtended<OvrReferenceManager>
    {
        #region OVR Hands

        [Header("OVR tracking references")]
        [FormerlySerializedAs("leftOVRHand")]
        [FormerlySerializedAs("leftHand")]
        [SerializeField]
        private OVRHand leftOvrHand = null;

        [FormerlySerializedAs("rightOVRHand")]
        [FormerlySerializedAs("rightHand")]
        [SerializeField]
        private OVRHand rightOvrHand = null;

        private readonly OVRHand[] _hand = new OVRHand[(int) OVRHand.Hand.HandRight + 1];
        
        public OVRHand RightOvrHand
        {
            get => _hand[(int) OVRHand.Hand.HandRight];
            private set => _hand[(int) OVRHand.Hand.HandRight] = value;
        }

        public OVRHand LeftOvrHand
        {
            get => _hand[(int) OVRHand.Hand.HandLeft];
            private set => _hand[(int) OVRHand.Hand.HandLeft] = value;
        }
        
        #endregion

        #region OVR Controller

        [SerializeField]
        private Transform leftControllerAnchor;
        [SerializeField]
        private Transform rightControllerAnchor;

        public Transform LeftControllerAnchor => leftControllerAnchor;
        public Transform RightControllerAnchor => rightControllerAnchor;

        #endregion

        #region Input Managers

        [Header("Input References")]
        [SerializeField, Interface(typeof(Hmd))]
        private MonoBehaviour hmd;
        public IHmd Hmd { get; private set; }

        [SerializeField, Interface(typeof(IHand))]
        private MonoBehaviour leftHandInput;
        public IHand LeftInputHand { get; private set; }


        [SerializeField, Interface(typeof(IHand))]
        private MonoBehaviour rightHandInput;
        public IHand RightInputHand { get; private set; }

        public HandVisual LeftHandVisual;
        public HandVisual RightHandVisual;
        
        [SerializeField, Interface(typeof(IController))]
        private MonoBehaviour leftControllerInput;
        public IController LeftControllerInput { get; private set; }


        [SerializeField, Interface(typeof(IController))]
        private MonoBehaviour rightControllerInput;
        public IController RightControllerInput { get; private set; }

        #region Synthetic hands

        [Header("Synthetic hands")]
        // ToDo: Still required?
        [SerializeField]
        private Transform lSyntheticHandManager;
        public Transform LSyntheticHandManager
        {
            get => lSyntheticHandManager;
            private set => lSyntheticHandManager = value;
        }
        // ToDo: Still required?
        [SerializeField]
        private Transform rSyntheticHandManager;
        public Transform RSyntheticHandManager
        {
            get => rSyntheticHandManager;
            private set => rSyntheticHandManager = value;
        }

        [SerializeField]
        private SyntheticHand lSyntheticHand;
        public SyntheticHand LSyntheticHand
        {
            get => lSyntheticHand;
            private set => lSyntheticHand = value;
        }
        [SerializeField]
        private SyntheticHand rSyntheticHand;
        public SyntheticHand RSyntheticHand
        {
            get => rSyntheticHand;
            private set => rSyntheticHand = value;
        }

        #endregion
        
        #endregion
        
        private bool _started;


        private void Awake()
        {
            Assert.IsNotNull(leftOvrHand);
            Assert.IsNotNull(rightOvrHand);

            LeftOvrHand = leftOvrHand;
            RightOvrHand = rightOvrHand;
            
            LeftInputHand = leftHandInput as IHand;
            RightInputHand = rightHandInput as IHand;
            Hmd = hmd as IHmd;
            LeftControllerInput = leftControllerInput as IController;
            RightControllerInput = rightControllerInput as IController;
            
            ConfigureSources();
        }
        
        private void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(leftHandInput);
            Assert.IsNotNull(rightHandInput);
            Assert.IsNotNull(leftControllerAnchor);
            Assert.IsNotNull(rightControllerAnchor);
            Assert.IsNotNull(hmd);
            Assert.IsNotNull(lSyntheticHandManager);
            Assert.IsNotNull(rSyntheticHandManager);
            Assert.IsNotNull(lSyntheticHand);
            Assert.IsNotNull(rSyntheticHand);
            Assert.IsNotNull(leftControllerInput);
            Assert.IsNotNull(rightControllerInput);
            this.EndStart(ref _started);
        }
        
        
        private void ConfigureSources()
        {

            // Controllers:
            var questModel = OVRPlugin.GetSystemHeadsetType();
            switch (questModel)
            {
                case OVRPlugin.SystemHeadset.Oculus_Quest:
                case OVRPlugin.SystemHeadset.Oculus_Link_Quest:
                    Debug.Log("Setting targets to Quest1.", this);
                    CalibrationManager.Instance.leftControllerCalibrationPoint = ReferenceManager.Instance.Quest1ControllerCenterLeft;
                    CalibrationManager.Instance.rightControllerCalibrationPoint = ReferenceManager.Instance.Quest1ControllerCenterRight;
                    break;
                case OVRPlugin.SystemHeadset.Oculus_Link_Quest_2:
                case OVRPlugin.SystemHeadset.Oculus_Quest_2:
                    Debug.Log("Setting targets to Quest2.", this);
                    CalibrationManager.Instance.leftControllerCalibrationPoint = ReferenceManager.Instance.Quest2ControllerCenterLeft;
                    CalibrationManager.Instance.rightControllerCalibrationPoint = ReferenceManager.Instance.Quest2ControllerCenterRight;
                    break;
                case OVRPlugin.SystemHeadset.Meta_Quest_Pro:
                case OVRPlugin.SystemHeadset.Meta_Link_Quest_Pro:
                    Debug.Log("Setting targets to Quest Pro.", this);
                    CalibrationManager.Instance.leftControllerCalibrationPoint = ReferenceManager.Instance.QuestProControllerCenterLeft;
                    CalibrationManager.Instance.rightControllerCalibrationPoint = ReferenceManager.Instance.QuestProControllerCenterRight;
                    break;
                case OVRPlugin.SystemHeadset.None:
                case OVRPlugin.SystemHeadset.Placeholder_11:
                case OVRPlugin.SystemHeadset.Placeholder_12:
                case OVRPlugin.SystemHeadset.Placeholder_13:
                case OVRPlugin.SystemHeadset.Placeholder_14:
                case OVRPlugin.SystemHeadset.Rift_DK1:
                case OVRPlugin.SystemHeadset.Rift_DK2:
                case OVRPlugin.SystemHeadset.Rift_CV1:
                case OVRPlugin.SystemHeadset.Rift_CB:
                case OVRPlugin.SystemHeadset.Rift_S:
                case OVRPlugin.SystemHeadset.PC_Placeholder_4104:
                case OVRPlugin.SystemHeadset.PC_Placeholder_4105:
                case OVRPlugin.SystemHeadset.PC_Placeholder_4106:
                case OVRPlugin.SystemHeadset.PC_Placeholder_4107:
                default:
                    Debug.LogWarning("The system is not configured for your device!" +
                                     "Setting targets to Quest2 anyway. Results may be inaccurate!", this);
                    CalibrationManager.Instance.leftControllerCalibrationPoint = ReferenceManager.Instance.Quest2ControllerCenterLeft;
                    CalibrationManager.Instance.rightControllerCalibrationPoint = ReferenceManager.Instance.Quest2ControllerCenterRight;
                    break;
            }
        }
        
    }
}