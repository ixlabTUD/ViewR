using UnityEditor;
using UnityEngine;

namespace ViewR.Core.Calibration
{
    public class CalibrationStation : MonoBehaviour
    {
        [SerializeField] private Transform targetL;

        [SerializeField] private Transform targetR;

        public CalibrationStationType calibrationStationType;
        public HandCalibrationConfiguration calibrationConfiguration;
        public float distanceBetweenCalibrationTargets;


        public CalibrationSocketOrientation socketOrientation;

        [Header("Debugging")] [SerializeField] private bool debugging;

        public Transform TargetL => targetL;
        public Transform TargetR => targetR;

        private void OnEnable()
        {
            if (debugging)
                Debug.Log($"Registering {nameof(CalibrationStation)}, called from {this}.", this);

            distanceBetweenCalibrationTargets = Vector3.Distance(targetL.position, targetR.position);
            CalibrationManager.Instance.RegisterStation(this);
        }

        private void OnDisable()
        {
            if (debugging)
                Debug.Log($"Unregistering {nameof(CalibrationStation)}, called from {this}.", this);

            
            //CalibrationManager.Instance.UnregisterStation(this);
        }
    }
}