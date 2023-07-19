using UnityEditor;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.HelpersLib.Utils.DestroyObjects
{
    /// <summary>
    /// Destroys this GameObject on start if not in the editor
    /// </summary>
    public class DestroyGameObjectInBuild : MonoBehaviour
    {
        [Help("This GameObject will be destroyed if not in the Editor!", MessageType.Warning)]
        [SerializeField]
        private bool execute = true;
        
#if !UNITY_EDITOR
        private void Start()
        {
            if(execute)
                Destroy(this.gameObject);
        }   
#endif
    }
}