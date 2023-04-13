using Normal.Realtime;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.Utils.ToggleObjects;

namespace ViewR.Core.Networking.Normcore.Utils.Ownership
{
    /// <summary>
    /// Disables components if we are owner of the realtime view
    /// </summary>
    public class DisableComponentsIfOwner : MonoBehaviour
    {
        [Header("References - OLD")]
        [SerializeField, Tooltip("Old, you may use ObjectsToToggle instead."),
         Help("Old, you may use ObjectsToToggle instead.")]
        private MonoBehaviour[] componentsToEnable;
        [SerializeField, Tooltip("Old, you may use ObjectsToToggle instead."),
         Help("Old, you may use ObjectsToToggle instead.")]
        private Renderer[] renderers; 

        [Header("References - NEW")]
        [SerializeField]
        private ObjectsToToggle objectsToToggle; 
        
        [Header("Setup")]
        [SerializeField]
        private RealtimeView realtimeView;
        [SerializeField]
        private bool enableOnStart = true;

        private void Start()
        {
            // Bail if not ours
            if(!realtimeView.isOwnedLocallySelf || !enableOnStart)
                return;

            EnableComponents(false);
        }

        private void EnableComponents(bool setEnabled)
        {
            foreach (var componentToEnable in componentsToEnable)
                componentToEnable.enabled = setEnabled;
            
            foreach (var r in renderers)
                r.enabled = setEnabled;
            
            objectsToToggle.Enable(setEnabled);
        }
    }
}