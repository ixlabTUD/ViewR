using UnityEngine;
using UnityEngine.EventSystems;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.Core.UI.FloatingUI.Loading
{
    /// <summary>
    /// This will always disable the event system, if another is present.
    /// Disable additional components if there is another event system present, by configuring them in the serialized fields.
    /// </summary>
    public class DisableEventSystemIfOtherSystemPresent : MonoBehaviour
    {
        [Help("This will always disable the event system, if another is present." +
              "\n" +
              "Disable additional components if there is another event system present, by configuring them below.")]
        [SerializeField]
        private bool active = true; 
        [SerializeField]
        private MonoBehaviour[] monoBehaviours; 
        [SerializeField]
        private Renderer[] renderers; 
        [SerializeField]
        private GameObject[] gameObjects; 
        
        private void OnEnable()
        {
            if (!active)
                return;
            
            // Search for event systems on active game objects.
            var eventSystems = FindObjectsOfType<EventSystem>();
        
            if(eventSystems.Length > 1)
            {
                this.gameObject.SetActive(false);
                
                foreach (var monoBehaviour in monoBehaviours)
                    monoBehaviour.enabled = false;
            
                foreach (var r in renderers)
                    r.enabled = false;
            
                foreach (var o in gameObjects)
                    o.SetActive(false);
            }

            // Clean up - if we are not in the editor. Allowing us to keep track easier in the editor.
#if !UNITY_EDITOR
            Destroy(this);
#endif
        }
    }
}
