using UnityEngine;
using UnityEngine.UI;

namespace ViewR.HelpersLib.Universals.UI.Toggle
{
    /// <summary>
    /// A toggle group controller to process changes in streamlined and allow to disable interactability on toggles. 
    /// </summary>
    [RequireComponent(typeof(ToggleGroup))]
    public class ToggleGroupController : MonoBehaviour
    {
        private ToggleGroup _toggleGroup;
        private UnityEngine.UI.Toggle[] _toggles;

        private void Awake()
        {
            // Get references
            _toggleGroup = GetComponent<ToggleGroup>();
            _toggles = _toggleGroup.GetComponentsInChildren<UnityEngine.UI.Toggle>();
        }

        public void DeactivateInteractiveOfAllTogglesExcept(UnityEngine.UI.Toggle exceptThisToggle) => SetInteractiveOfAllToggles(false, exceptThisToggle);

        public void DeactivateInteractiveOfAllToggles() => SetInteractiveOfAllToggles(false);

        public void ActivateInteractiveOfAllToggles() => SetInteractiveOfAllToggles(true);

        public void SetInteractiveOfAllToggles(bool interactable, UnityEngine.UI.Toggle exceptThisToggle = null)
        {
            foreach (var toggle in _toggles)
            {
                if (toggle == exceptThisToggle)
                    continue;
                
                toggle.interactable = interactable;
            }
        }
    }
}
