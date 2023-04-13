using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

namespace ViewR.HelpersLib.SurgeExtensions.StateMachineExt.Editor
{
    
    [CustomEditor(typeof(Button), true)]
    [CanEditMultipleObjects]
    public class TabButtonEditor : SelectableEditor
    {
        SerializedProperty _onClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _onClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_onClickProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}