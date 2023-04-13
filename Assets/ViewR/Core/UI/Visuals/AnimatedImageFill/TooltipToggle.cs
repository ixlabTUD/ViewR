using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ViewR.Core.UI.Visuals.AnimatedImageFill
{
    /// <summary>
    /// Inheriting from <see cref="Tooltip"/>, this class only updates the <see cref="Tooltip.objectsToToggle"/> <see cref="OnPointerExit"/> if the toggle is not on!
    /// Additionally, if the parent game object contains a <see cref="ToggleGroup"/>, it won't react to <see cref="OnPointerEnter"/>, if <see cref="ToggleGroup.AnyTogglesOn()"/> is true.  
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class TooltipToggle : Tooltip
    {
        private Toggle _toggle;
        private ToggleGroup _toggleGroup;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggleGroup = GetComponentInParent<ToggleGroup>();
        }


        public override void OnPointerEnter(PointerEventData eventData)
        {
            // Don't fade in, if another toggle is already active.
            if (_toggleGroup != null && _toggleGroup.AnyTogglesOn())
                return;

            objectsToToggle.ToggleOn();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            // Don't fade out if turned on.
            if (_toggle.isOn || (_toggleGroup != null && _toggleGroup.AnyTogglesOn()))
                return;

            objectsToToggle.ToggleOff();
        }
    }
}