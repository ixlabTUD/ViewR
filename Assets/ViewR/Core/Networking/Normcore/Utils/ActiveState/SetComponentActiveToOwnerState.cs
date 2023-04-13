using Normal.Realtime;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.Utils.ToggleObjects;

namespace ViewR.Core.Networking.Normcore.Utils.ActiveState
{
    /// <summary>
    /// Sets the components "enabled" state to whether or not realtimeView is owned locally.
    /// </summary>
    public class SetComponentActiveToOwnerState : MonoBehaviour
    {
        [SerializeField, Tooltip("Old, you may use ObjectsToToggle instead."), Help("Old, you may use ObjectsToToggle instead.")]
        private MonoBehaviour[] componentsToEnable;
        [SerializeField]
        private ObjectsToToggle objectsToToggle;
        [SerializeField]
        private RealtimeView realtimeView;
        [SerializeField]
        private bool enableOnStart = true;
        [SerializeField, Tooltip("If true, it will activate the components if we are not the owner and it will deactivate them if we are the owner.")]
        private bool invert = false;

        private bool _done;

        private void Start()
        {
            if (enableOnStart)
                EnableComponents();
        }

        
        private void Update()
        {
            // In case the realtimeView is not found yet
            if (!_done && enableOnStart)
            {
                EnableComponents();
            }
        }

        private void EnableComponents()
        {
            if (componentsToEnable != null)
                foreach (var componentToEnable in componentsToEnable)
                    componentToEnable.enabled =
                        !invert ? realtimeView.isOwnedLocallySelf : !realtimeView.isOwnedLocallySelf;
            // Do it the new way too!
            objectsToToggle.Enable(!invert ? realtimeView.isOwnedLocallySelf : !realtimeView.isOwnedLocallySelf);

            _done = true;
            
            Destroy(this);
        }
    }
}