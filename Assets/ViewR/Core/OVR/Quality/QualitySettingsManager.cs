using System;
using System.Linq;
using UnityEngine;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Core.OVR.Quality
{
    /// <summary>
    /// Simplified access point to modify quality settings.
    /// See Static members and/or <see cref="SetQualitySettings"/> to use this.
    /// </summary>
    public class QualitySettingsManager : MonoBehaviour
    {

        #region Staic properties

        private static bool _debugging;
        private static bool _initializedPerformance;
        
        private static int _defaultMsaaLevel = 4;
        private static int _defaultQualityLevel = 0;

        #endregion

        #region Main quality setters

        public static void SetQualityByName(
            int? msaaLevel = null,
            string qualityLevelName = null)
        {
            if (!string.IsNullOrEmpty(qualityLevelName))
            {
                // Check if it exists on our build
                if (!QualitySettings.names.Contains(qualityLevelName))
                    throw new Exception(
                        $"Quality setting \"{qualityLevelName}\" is not present. It may be not defined in the Quality Settings or it is not included in this build.");
                
                var positionInArray = QualitySettings.names.FindIndex(qualityLevelName);
                // Call int based method
                SetQuality(msaaLevel, positionInArray);
            }
            else
                // Call int based method
                SetQuality(msaaLevel, (int?) null);
        }
        
        /// <summary>
        /// Applies the <see cref="msaaLevel"/> and the <see cref="qualityLevel"/>, if they are != null.
        /// Note: This does apply a stepwise check for the <see cref="msaaLevel"/> to ensure it is set to valid values. 
        /// </summary>
        public static void SetQuality(
            int? msaaLevel = null,
            int? qualityLevel = null)
        {
            InitializeValues();

            // Check value
            if (qualityLevel != null)
            {
                // Check if it exists on our build
                if (QualitySettings.names.Length <= qualityLevel)
                    throw new Exception(
                        $"Quality setting \"{qualityLevel}\" is not present. It may be not defined in the Quality Settings or it is not included in this build.");

                // Only apply if different.
                if (QualitySettings.GetQualityLevel() != qualityLevel)
                    // Set value
                    QualitySettings.SetQualityLevel((int) qualityLevel, true);
            }
            
            // Check value
            if (msaaLevel != null)
            {
                // Only apply if different.
                if (msaaLevel != QualitySettings.antiAliasing)
                {
                    // Set value
                    if (msaaLevel >= 8)
                        QualitySettings.antiAliasing = 8;
                    else if (msaaLevel >= 4)
                        QualitySettings.antiAliasing = 4;
                    else if (msaaLevel >= 1)
                        QualitySettings.antiAliasing = 2;
                    else
                        QualitySettings.antiAliasing = 0;
                }
            }
        }

        #endregion

        #region Internal methods

        private static void InitializeValues(bool overwriteInit = false)
        {
            if ( !overwriteInit && _initializedPerformance) return;

            if (_debugging)
                Debug.Log("Quality Report:\n" +
                          "msaaLevel: " + QualitySettings.antiAliasing + "\n" +
                          "Quality Level: " + QualitySettings.GetQualityLevel() + ": " + QualitySettings.names[QualitySettings.GetQualityLevel()]);

            // Fetch default values.
            _defaultMsaaLevel = QualitySettings.antiAliasing;
            _defaultQualityLevel = QualitySettings.GetQualityLevel();

            
            _initializedPerformance = true;
        }

        #endregion


        #region External Methods

        public void ApplyQualitySettings(
            int? msaa = null,
            int? qualityLevel = null
            )
        {
            SetQuality(msaa, qualityLevel);
        }

        public void ApplyQualitySettings(
            int? msaa = null,
            string qualityLevelName = null
            )
        {
            SetQualityByName(msaa, qualityLevelName);
        }

        public static void RestoreDefaultMsaaLevel()
        {
            SetQuality(
                _defaultMsaaLevel,
                (int?) null
            );
        }
        public static void RestoreDefaultQualityLevel()
        {
            SetQuality(
                null,
                _defaultQualityLevel
            );
        }
        public static void RestoreDefaults()
        {
            SetQuality(
                _defaultMsaaLevel,
                _defaultQualityLevel
            );
        }

        public static void IncreaseQualityLevel()
        {
            QualitySettings.IncreaseLevel(true);
        }

        public static void DecreaseQualityLevel()
        {
            QualitySettings.DecreaseLevel(true);
        }

        #endregion
    }
}