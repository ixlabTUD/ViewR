using UnityEngine;
using ViewR.Core.Calibration.Aligner.Scripts;
using ViewR.Core.Networking.Normcore.RealtimeInstanceManagement;
using ViewR.HelpersLib.Utils.ToggleObjects;

namespace ViewR.Core.Setup
{
    public class SetupOnceAligned : MonoBehaviour
    {
        [SerializeField] private ObjectsToToggle objectsToToggleOnceAligned;
        
        private void Start()
        { 
            // Subscribe
            CalibrationEvents.FirstCalibrationPerformed += CalibrationEventsOnFirstCalibrationPerformed;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe
            CalibrationEvents.FirstCalibrationPerformed -= CalibrationEventsOnFirstCalibrationPerformed;
        }

        private void CalibrationEventsOnFirstCalibrationPerformed(bool firstCalibration)
        {
            // Show Space
            objectsToToggleOnceAligned.ToggleOn();
            
            // Go online.
            RealtimeReferencer.RealtimeToUse.Connect(RealtimeReferencer.RealtimeToUse.roomToJoinOnStart);
        }
    }
}