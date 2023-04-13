using UnityEngine;

namespace ViewR.Tools.CSVWriter.Mode
{
    public class TrackingModeSetter : MonoBehaviour
    {
        public void SetTrackingMode(TrackingMode newTrackingMode)
        {
            TrackingModeManager.CurrentTrackingMode = newTrackingMode;
        }
        
        public void SetTrackingMode(int newTrackingMode)
        {
            TrackingModeManager.CurrentTrackingMode = (TrackingMode) newTrackingMode;
        }
    }
}