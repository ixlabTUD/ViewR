using UnityEngine;

namespace ViewR.HelpersLib.Utils.DestroyObjects
{
    public class DestroyGameObjectOnEnable : MonoBehaviour
    {
        [SerializeField]
        private bool execute = true;
        
        private void OnEnable()
        {
            if (execute)
                Destroy(this.gameObject);
        }
    }
}
