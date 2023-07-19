using UnityEngine;

namespace ViewR.HelpersLib.Utils
{
    /// <summary>
    /// Gets MonoBehaviours and Renderers, as they do not inherit from a common parent that also has the .enabled functionality.
    /// </summary>
    public class DisableComponentOnAwake : MonoBehaviour
    {
        [SerializeField]
        private MonoBehaviour[] monoBehaviours; 
        [SerializeField]
        private Renderer[] renderers; 
        [SerializeField]
        private bool disableOnAwake = true;
        
        
        private void Awake()
        {
            if (!disableOnAwake) return;
            
            foreach (var monoBehaviour in monoBehaviours)
                monoBehaviour.enabled = false;
            
            foreach (var r in renderers)
                r.enabled = false;
        
            // Clean up - if we are not in the editor. Allowing us to keep track easier in the editor.
#if !UNITY_EDITOR
            Destroy(this);
#endif
        }
    }
}