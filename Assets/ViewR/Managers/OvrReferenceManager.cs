using System;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

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
        
    }
}