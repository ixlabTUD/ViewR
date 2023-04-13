using UnityEngine;

namespace ViewR.HelpersLib.Extensions.General
{
    /// <summary>
    /// Extension methods for numerical values.
    /// 
    /// Modified, from Andrew Perry @ https://gist.github.com/omgwtfgames/f917ca28581761b8100f/564dddacb77d63d0e8084f7cdc0cd16f86e82f9c - Thanks!
    /// </summary>
    
    public static class NumericalExtensionMethods
    {

        public static float LinearRemap(this float value,
            float valueRangeMin, float valueRangeMax,
            float newRangeMin, float newRangeMax)
        {
            return (value - valueRangeMin) / (valueRangeMax - valueRangeMin) * (newRangeMax - newRangeMin) + newRangeMin;
        }

        public static int WithRandomSign(this int value, float negativeProbability = 0.5f)
        {
            return Random.value < negativeProbability ? -value : value;
        }

    }
}