using UnityEditor;
using UnityEngine;

namespace ViewR.HelpersLib.Extensions.General
{
    #if UNITY_EDITOR
    /// <summary>
    /// Extensions to Inspector windows
    /// </summary>
    public class EditorWindowsExtensions : MonoBehaviour
    {
        /// <summary>
        /// Draws a horizontal line in an editor window.
        /// Optional color (default: gray).
        /// </summary>
        /// <param name="color"></param>
        /// <param name="thickness"></param>
        /// <param name="padding"></param>
        ///
        public static void UIDrawHorizontalLine(Color? color = null, int thickness = 2, int padding = 10)
        {
            color ??= Color.gray;
            var rect = EditorGUILayout.GetControlRect(GUILayout.Height(thickness + padding));
            rect.height = thickness;
            rect.width += 6;
            rect.x -= 2;
            rect.y += padding / (float) 2;
            EditorGUI.DrawRect(rect, (Color)color);
        }
    }
    #endif
}