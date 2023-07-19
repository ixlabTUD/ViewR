using UnityEngine;

namespace ViewR.HelpersLib.Utils
{
    public class DisableGameObjectOnAwake : MonoBehaviour
    {
        [SerializeField]
        private bool active = true;
        
        private void Awake()
        {
            if(active)
                this.gameObject.SetActive(false);
            else
                Destroy(this);
            
            // Clean up - if we are not in the editor. Allowing us to keep track easier in the editor.
#if !UNITY_EDITOR
            Destroy(this);
#endif
        }
    }
}
