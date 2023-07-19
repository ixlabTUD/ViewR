using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ViewR.Core.Calibration
{
    /// <summary>
    ///     Checks if a button is pressed on both controllers and executes logic based on them.
    /// </summary>
    public class ControllerBasedCalibrator : MonoBehaviour
    {
        public InputAction leftControllerPrimaryButton;
        public InputAction rightControllerPrimaryButton;

        [HideInInspector] public bool Running;

        [Header("Local use")] [SerializeField] private CalibrationStation localCalibrationStation;

        private Transform _sourceHandL;
        private Transform _sourceHandR;

        private Transform _transformToMove;


        private ProgressBar progressBar;

        private readonly List<Vector3> sourcesDirectionL = new();
        private readonly List<Vector3> sourcesDirectionR = new();
        private readonly List<Vector3> sourcesL = new();
        private readonly List<Vector3> sourcesR = new();


        private void Awake()
        {
            progressBar = GetComponent<ProgressBar>();
            _transformToMove = CalibrationManager.Instance.transformToMove;
            _sourceHandL = CalibrationManager.Instance.leftControllerCalibrationPoint;
            _sourceHandR = CalibrationManager.Instance.rightControllerCalibrationPoint;

            leftControllerPrimaryButton.Enable();
            rightControllerPrimaryButton.Enable();
        }


        private void Update()
        {
            if (!Running &&
                ((leftControllerPrimaryButton.IsPressed() && rightControllerPrimaryButton.WasPressedThisFrame()) ||
                 (rightControllerPrimaryButton.IsPressed() && leftControllerPrimaryButton.WasPerformedThisFrame())))
            {
                // Set flag

                Running = true;
                ButtonPressed();
            }

            if (Running && leftControllerPrimaryButton.IsPressed() && rightControllerPrimaryButton.IsPressed())
                ProcessBothButtonsHeldDown();

            if (Running && (!leftControllerPrimaryButton.IsPressed() || !rightControllerPrimaryButton.IsPressed()))
            {
                ProcessBothButtonsReleased();
                // Set flag
                Running = false;
            }
        }

        private void OnEnable()
        {
            progressBar.chargeSuccessEvent += RequestCalibration;
        }

        private void OnDisable()
        {
            progressBar.chargeSuccessEvent -= RequestCalibration;
        }

        private void RequestCalibration()
        {
            CalibrationManager.Instance.RequestCalibrationCharged(sourcesL, sourcesR, sourcesDirectionL,
                sourcesDirectionR, localCalibrationStation);
        }

        private void ButtonPressed()
        {
            progressBar.EnableProgressBar();
        }

        /// <summary>
        ///     Populate Data
        /// </summary>
        private void ProcessBothButtonsHeldDown()
        {
            progressBar.IncreaseProgressBar();


            sourcesL.Add(_sourceHandL.position);
            sourcesR.Add(_sourceHandR.position);

            sourcesDirectionL.Add(_sourceHandL.forward);
            sourcesDirectionR.Add(_sourceHandR.forward);
        }

        private void ProcessBothButtonsReleased()
        {
            progressBar.DisableProgressBar();

            sourcesL.Clear();
            sourcesR.Clear();

            sourcesDirectionL.Clear();
            sourcesDirectionR.Clear();
        }
    }
}