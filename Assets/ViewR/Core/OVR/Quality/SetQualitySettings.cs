using UnityEngine;

namespace ViewR.Core.OVR.Quality
{
    /// <summary>
    /// Interface for <see cref="QualitySettingsManager"/>.
    /// </summary>
    public class SetQualitySettings : MonoBehaviour
    {
        public const int MSAA_LEVEL_LOW = 2;
        public const int MSAA_LEVEL_HIGH = 4;
        
        
        public void SetMsaaDefault() => QualitySettingsManager.RestoreDefaultMsaaLevel();
        public void SetMsaa(int msaaLevel) => QualitySettingsManager.SetQuality(msaaLevel);
        public void SetMsaaPerformanceLow() => QualitySettingsManager.SetQuality(msaaLevel: MSAA_LEVEL_LOW);
        public void SetMsaaPerformanceHigh() => QualitySettingsManager.SetQuality(msaaLevel: MSAA_LEVEL_HIGH);

        public void SetQualityLevelDefault() => QualitySettingsManager.RestoreDefaultQualityLevel();
        public void SetQualityLevel(int qualityLevel) => QualitySettingsManager.SetQuality(null, qualityLevel);
        public void SetQualityLevel(string qualityLevelName) => QualitySettingsManager.SetQualityByName(null, qualityLevelName);
        public void IncreaseQualityLevel() => QualitySettingsManager.IncreaseQualityLevel();
        public void DecreaseQualityLevel() => QualitySettingsManager.DecreaseQualityLevel();
        
        public void SetQuality(int msaaLevel, int qualityLevel) => QualitySettingsManager.SetQuality(msaaLevel, qualityLevel);
        public void SetQuality(int msaaLevel, string qualityLevelName) => QualitySettingsManager.SetQualityByName(msaaLevel, qualityLevelName);
    }
}