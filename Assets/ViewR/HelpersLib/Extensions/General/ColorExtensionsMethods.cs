using UnityEngine;

namespace ViewR.HelpersLib.Extensions.General
{
    public static class ColorExtensionsMethods 
    {
        /// <summary>
        /// Overwrites r of a color
        /// </summary>
        public static Color SetR(this Color color, float r) => new Color(r, color.g, color.b, color.a);
        
        /// <summary>
        /// Overwrites g of a color
        /// </summary>
        public static Color SetG(this Color color, float g) => new Color(color.r, g, color.b, color.a);
        
        /// <summary>
        /// Overwrites b of a color
        /// </summary>
        public static Color SetB(this Color color, float b) => new Color(color.r, color.g, b, color.a);
        
        /// <summary>
        /// Overwrites alpha of a color
        /// </summary>
        public static Color SetA(this Color color, float a) => new Color(color.r, color.g, color.b, a);

    }
}