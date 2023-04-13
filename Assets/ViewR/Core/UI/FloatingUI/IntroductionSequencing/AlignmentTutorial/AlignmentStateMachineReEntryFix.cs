using System.Collections;
using UnityEngine;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing.AlignmentTutorial
{
    /// <summary>
    /// A quick fix to ensure we do display these states.
    /// </summary>
    public class AlignmentStateMachineReEntryFix : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefabLastState;
    
        private void OnEnable()
        {
            StartCoroutine(StartDelayed());
        }

        private IEnumerator StartDelayed()
        {
            yield return new WaitForSeconds(.2f);

            var firstChild = transform.GetChild(0).gameObject;
        
            yield return new WaitForSeconds(.05f);
            firstChild.SetActive(false);
        
            var lastChild = this.transform.GetChild(transform.childCount - 1).gameObject;
            // Instantiate if not yet present:
            if (!lastChild.name.Contains(prefabLastState.name))
            {
                var newLastChild = Instantiate(prefabLastState, transform);
                newLastChild.SetActive(false);
            }
        
            yield return new WaitForSeconds(.05f);
            firstChild.SetActive(true);
        }
    }
}
