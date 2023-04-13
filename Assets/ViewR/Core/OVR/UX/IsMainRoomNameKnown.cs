using UnityEngine;
using UnityEngine.Events;
using ViewR.HelpersLib.Utils.ToggleObjects;
using ViewR.Managers;

namespace ViewR.Core.OVR.UX
{
    public class IsMainRoomNameKnown : MonoBehaviour
    {
        [SerializeField]
        private ObjectsToToggle objectsToActivateOnceTrueOnEnable;
        [SerializeField]
        private ObjectsToToggle objectsToDeactivateOnceTrueOnEnable;
        [SerializeField]
        private UnityEvent isValid;
        [SerializeField]
        private UnityEvent isInvalid;
        
        private void OnEnable()
        {
            // Call methods.
            if (Evaluate())
            {
                objectsToActivateOnceTrueOnEnable.Enable(true);
                objectsToDeactivateOnceTrueOnEnable.Enable(false);
                isValid?.Invoke();
            }
            else
            {
                isInvalid?.Invoke();
            }
        }

        private bool Evaluate()
        {
            if (NetworkManager.IsInstanceRegistered)
                return NetworkManager.Instance.GetRoomNameToJoin() != string.Empty;
            else
                return false;
        }
    }
}