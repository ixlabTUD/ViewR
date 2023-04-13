/************************************************************************************

Modified, originally from: https://stackoverflow.com/a/58446816, 
Originally by: ChoopTwisk
Modified by: Florian Schier
License: (CC BY-SA 4.0 - https://creativecommons.org/licenses/by-sa/4.0/)

************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ViewR.HelpersLib.Extensions.EditorExtensions.ShowAttribute.Editor
{
    /// <summary>
    /// Drawer for the <see cref="ShowIfAttribute"/>.
    /// The drawer will be able to change a variables visibility and whether its editable
    /// </summary>
    [CustomPropertyDrawer(typeof(ShowIfAttribute), true)]
    public class ShowIfAttributeDrawer : PropertyDrawer
    {
        #region Reflection helpers.

        private static MethodInfo GetMethod(object target, string methodName)
        {
            return GetAllMethods(target, m => m.Name.Equals(methodName,
                StringComparison.InvariantCulture)).FirstOrDefault();
        }

        private static FieldInfo GetField(object target, string fieldName)
        {
            return GetAllFields(target, f => f.Name.Equals(fieldName,
                StringComparison.InvariantCulture)).FirstOrDefault();
        }

        private static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo,
            bool> predicate)
        {
            var types = new List<Type>()
            {
                target.GetType()
            };

            while (types.Last().BaseType != null)
            {
                types.Add(types.Last().BaseType);
            }

            for (var i = types.Count - 1; i >= 0; i--)
            {
                var fieldInfos = types[i]
                    .GetFields(BindingFlags.Instance | BindingFlags.Static |
                               BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(predicate);

                foreach (var fieldInfo in fieldInfos)
                {
                    yield return fieldInfo;
                }
            }
        }

        private static IEnumerable<MethodInfo> GetAllMethods(object target,
            Func<MethodInfo, bool> predicate)
        {
            var methodInfos = target.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static |
                            BindingFlags.NonPublic | BindingFlags.Public)
                .Where(predicate);

            return methodInfos;
        }

        #endregion

        /// <summary>
        /// Evaluation.
        /// Executes field and method based evaluation and returns a bool based on the <see cref="ConditionOperator"/>.
        /// </summary>
        /// <returns></returns>
        private bool MeetsConditions(SerializedProperty property)
        {
            var showIfAttribute = this.attribute as ShowIfAttribute;
            var target = property.serializedObject.targetObject;
            var conditionValues = new List<bool>();

            // Iterate over all conditions
            foreach (var condition in showIfAttribute.Conditions)
            {
                // Evaluate Field based evaluation
                var conditionField = GetField(target, condition);
                if (conditionField != null &&
                    conditionField.FieldType == typeof(bool))
                {
                    conditionValues.Add((bool) conditionField.GetValue(target));
                }

                // Evaluate Method based evaluation
                var conditionMethod = GetMethod(target, condition);
                if (conditionMethod != null &&
                    conditionMethod.ReturnType == typeof(bool) &&
                    conditionMethod.GetParameters().Length == 0)
                {
                    conditionValues.Add((bool) conditionMethod.Invoke(target, null));
                }
            }

            if (conditionValues.Count > 0)
            {
                var met = showIfAttribute.Operator == ConditionOperator.AND
                    ? conditionValues.Aggregate(true, (current, value) => current && value)
                    : conditionValues.Aggregate(false, (current, value) => current || value);
                return met;
            }
            else
            {
                Debug.LogError("Invalid boolean condition fields or methods used!");
                return true;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent
            label)
        {
            // Calculate the property height, if we don't meet the condition and the draw mode is DontDraw, then height will be 0.
            var meetsCondition = MeetsConditions(property);
            var showIfAttribute = this.attribute as ShowIfAttribute;

            if (!meetsCondition && showIfAttribute?.Action == ActionOnConditionFail.DontDraw)
                return 0;
            return base.GetPropertyHeight(property, label);
        }

        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent
            label)
        {
            var meetsCondition = MeetsConditions(property);
            // Early out, if conditions met, draw and go.
            if (meetsCondition)
            {
                // Draw it!
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            var showIfAttribute = this.attribute as ShowIfAttribute;
            if (showIfAttribute?.Action == ActionOnConditionFail.DontDraw)
            {
                // Simply don't draw it at all.
                return;
            }
            else if (showIfAttribute?.Action == ActionOnConditionFail.DisableInspectorEditing)
            {
                // Saving previous GUI enabled value
                var previousGUIState = GUI.enabled;
                // Disabling edit for property
                GUI.enabled = false;
                // Drawing Property
                EditorGUI.PropertyField(position, property, label, true);
                // Setting old GUI enabled value
                GUI.enabled = previousGUIState;

                // // Draw it without it being interactable
                // EditorGUI.BeginDisabledGroup(true);
                // EditorGUI.PropertyField(position, property, label, true);
                // EditorGUI.EndDisabledGroup();
            }
            else if(showIfAttribute == null)
            {
                throw new NullReferenceException("The showIfAttribute should not be null...");
            }
        }
    }
}