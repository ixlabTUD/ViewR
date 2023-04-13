/************************************************************************************

Modified, originally from: https://stackoverflow.com/a/58446816, 
Originally by: ChoopTwisk
Modified by: Florian Schier
License: (CC BY-SA 4.0 - https://creativecommons.org/licenses/by-sa/4.0/)

************************************************************************************/


using System;
using UnityEngine;

namespace ViewR.HelpersLib.Extensions.EditorExtensions.ShowAttribute
{
    /// <summary>
    /// Attribute to only "show" or "allow to edit" a public or serialized field, given that a condition is true.
    /// Otherwise the field won't be drawn, or becomes un-editable.
    /// </summary>
    /// <remarks>
    /// Use cases:    
    ///     Using a field to hide/show another field:
    ///         public bool showHideList = false; 
    ///         [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.AND, nameof(showHideList))]
    ///         public string aField = "item 1";
    ///         
    ///     Using a field to enable/disable another field:
    ///         public bool enableDisableList = false;
    ///         [ShowIf(ActionOnConditionFail.DisableInspectorEditing, ConditionOperator.AND, nameof(enableDisableList))]
    ///         public string anotherField = "item 2";
    ///
    ///     Using a method to get a condition value:
    ///         [ShowIf(ActionOnConditionFail.DisableInspectorEditing, ConditionOperator.AND,nameof(CalculateIsEnabled))]
    ///         public string yetAnotherField = "one more";    public 
    ///         bool CalculateIsEnabled()    
    ///         {
    ///             return true;    
    ///         }
    ///     
    ///     Using multiple conditions on the same field:
    ///         public bool condition1;    
    ///         public bool condition2;    
    ///         [ShowIf(ActionOnConditionFail.DisableInspectorEditing, ConditionOperator.AND, nameof(condition1), nameof(condition2))]
    ///         public string oneLastField= "last field";
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public ActionOnConditionFail Action { get; private set; }
        public ConditionOperator Operator { get; private set; }
        public string[] Conditions { get; private set; }

        /// <summary>
        /// WARNING: Cannot be used on Serialized fields, as it only iterates through the classes line of inheritance.
        /// Attempts to make it more resilient failed, mainly due to the fact that there may be many serialized instances
        /// on one class and we wouldn't know which one to compare against. 
        /// 
        /// Attribute to only "show" or "allow to edit" a public or serialized field, given that certain condition(s) is/are true or false.
        /// </summary>
        /// <param name="action">Should the component be un-editable or completely hidden?</param>
        /// <param name="conditionOperator">Condition if multiple conditions are given.</param>
        /// <param name="conditions">Can be a field such as nameof(condition1), or a method such as nameof(CalculateIsEnabled)</param>
        public ShowIfAttribute(ActionOnConditionFail action, ConditionOperator conditionOperator = ConditionOperator.AND,
            params string[] conditions)
        {
            Action = action;
            Operator = conditionOperator;
            Conditions = conditions;
        }
    }

    public enum ConditionOperator
    {
        // A field is visible/enabled only if all conditions are true.
        AND,

        // A field is visible/enabled if at least ONE condition is true.
        OR
    }

    public enum ActionOnConditionFail
    {
        // If condition(s) are false, don't draw the field at all.
        DontDraw,

        // If condition(s) are false, don't let us edit the field.
        DisableInspectorEditing
    }
}