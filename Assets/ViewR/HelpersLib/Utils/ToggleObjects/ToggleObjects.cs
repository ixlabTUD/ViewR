using UnityEngine;

namespace ViewR.HelpersLib.Utils.ToggleObjects
{
    public class ToggleObjects : MonoBehaviour
    {
        [SerializeField, Tooltip("Objects that will be enabled on ToggleOn, and disabled on ToggleOff")]
        private ObjectsToToggle objectsToToggleOnOnOn;
        [SerializeField, Tooltip("Objects that will be disabled on ToggleOn, and enabled on ToggleOff")]
        private ObjectsToToggle objectsToToggleOffOnOn;

        public void ToggleOn()
        {
            objectsToToggleOnOnOn.ToggleOn();
            objectsToToggleOffOnOn.ToggleOff();
        }
        
        public void ToggleOff()
        {
            objectsToToggleOnOnOn.ToggleOff();
            objectsToToggleOffOnOn.ToggleOn();
        }
        
        public void Toggle(bool toggleOn)
        {
            if (toggleOn)
                ToggleOn();
            else
                ToggleOff();
        }
    }
}
