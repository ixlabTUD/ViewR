using UnityEditor;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.HelpersLib.Utils.DestroyObjects
{
    public class DestroyGameObjectOnAwake : MonoBehaviour
    {
        [Help("This GameObject will be destroyed on awake!", MessageType.Warning)]
        [SerializeField]
        private bool execute = true;
        
        private void Awake()
        {
            if (execute)
                Destroy(this.gameObject);
        }
    }
}
