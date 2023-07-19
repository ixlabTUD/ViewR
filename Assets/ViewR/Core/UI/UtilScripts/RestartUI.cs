using System.Collections;
using UnityEngine;

namespace ViewR.Core.UI.UtilScripts
{
    public class RestartUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameObjectToToggle;
    
    
        public void DoRestartUi()
        {
            StartCoroutine(ToggleGameObjectOnOff());
        }

        private IEnumerator ToggleGameObjectOnOff()
        {
            gameObjectToToggle.SetActive(false);
            yield return new WaitForSeconds(0.05f);
            gameObjectToToggle.SetActive(true);
        
        }
    }
}
