using UnityEditor;
using UnityEngine;

namespace ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly.Editor
{

    /// <summary>
    /// This class contain custom drawer for ReadOnly attribute.
    /// Modified from: https://www.patrykgalach.com/2020/01/20/readonly-attribute-in-unity-editor/
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Unity method for drawing GUI in Editor
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property.</param>
        /// <param name="label">Label.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Saving previous GUI enabled value
            var previousGUIState = GUI.enabled;
            // Disabling edit for property
            GUI.enabled = false;
            // Drawing Property
            EditorGUI.PropertyField(position, property, label);
            // Setting old GUI enabled value
            GUI.enabled = previousGUIState;
        }
    }

    #region how to use example
    /*
     * to be used like this:
        [space]
        [ReadOnly]
        [SerializeField]
        private string textNonEditable = "You can't edit this text.";
        [SerializeField]
        private string textEditable = "But you definitely can edit this one.";
        [space]
     *
     */
    #endregion
}