using UnityEngine;

namespace ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly
{
    /// <summary>
    /// Read Only attribute.
    /// Attribute to mark objects with a [ReadOnly] property - allowing us to only show them, but not expose them in the editor.
    /// Modified from: https://www.patrykgalach.com/2020/01/20/readonly-attribute-in-unity-editor/
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute { }
}