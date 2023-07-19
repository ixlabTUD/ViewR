namespace ViewR.Core.Calibration.Aligner.Scripts
{
    /// <summary>
    /// Further Calibration Events can be found in <see cref="Aligner"/>.
    /// </summary>
    public static class CalibrationEvents
    {
        public delegate void SuccessfulCalibration(bool firstCalibration);

        /// <summary>
        /// Fires every time an calibration is performed.
        /// </summary>
        public static event SuccessfulCalibration CalibrationPerformed;
        /// <summary>
        /// Fires only upon the first successful calibration.
        /// </summary>
        public static event SuccessfulCalibration FirstCalibrationPerformed;

        /// <summary>
        /// Keeps track of first-time calibration. 
        /// </summary>
        private static bool _firstCalibrationSucceeded;

        static CalibrationEvents()
        {
            // Subscribe
            Aligner.CalibrationPerformed += AlignerOnCalibrationPerformed;
        }

        private static void AlignerOnCalibrationPerformed(float distanceBetween, float angleBetween,
            float endDistanceKabsch, float endAngleKabsch, float endDistanceTwoPoint, float endAngleTwoPoint,
            string calibrationMethod)
        {
            AlignerOnCalibrationPerformed();
        }
        
        private static void AlignerOnCalibrationPerformed()
        {
            // Fires first-time event
            if (!_firstCalibrationSucceeded)
            {
                FirstCalibrationPerformed?.Invoke(true);
                _firstCalibrationSucceeded = true;
            }
            
            // Always fires an event.
            CalibrationPerformed?.Invoke(_firstCalibrationSucceeded);
        }

        public static void InvokeAlignmentCompleted()
        {
            AlignerOnCalibrationPerformed();
        }
    }
}
