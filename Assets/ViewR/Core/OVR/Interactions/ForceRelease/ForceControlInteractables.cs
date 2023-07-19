using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Core.OVR.Interactions.ForceRelease
{
    /// <summary>
    /// Force-controls interactables.
    /// </summary>
    public class ForceControlInteractables : MonoBehaviour
    {
        [Header("Debugging")]
        [SerializeField] 
        private bool debugging;
        
        [SerializeField, Interface(typeof(IInteractable))]
        private List<MonoBehaviour> _interactables;
        private List<IInteractable> Interactables;

        private void Awake()
        {
            // Set Interactables
            Interactables = _interactables.ConvertAll(mono => mono as IInteractable);
        }

        public IEnumerable<IInteractable> GetSelectedInteractables()
        {
            return Interactables.Where(interactable => interactable.State == InteractableState.Select);
        }

        [ExposeMethodInEditor]
        public void ForceRelease()
        {
            // Fetch
            var selectedInteractables = GetSelectedInteractables();

            var selectingInteractors = new List<IInteractor>();
            
            // Fetch selecting interactors
            if (selectedInteractables != null)
            {
                foreach (var selectedInteractable in selectedInteractables)
                {
                    var res = selectedInteractable.SelectingInteractorViews as IEnumerable<IInteractor>;
                    foreach (var interactor in res)
                    {
                        selectingInteractors.Add(interactor);
                    }
                }
            }
            
            // Bail if none active
            if (!(selectingInteractors.Count > 0))
            {
                if (debugging)
                    Debug.Log("No selecting interactors. Bailing!".StartWithFrom(GetType()), this);
                return;
            }
            
            foreach (var selectingInteractor in selectingInteractors)
            {
                // selectingInteractor.Unselect();
                
                // Dispble and reenable deleayed
                StartCoroutine(ToggleInteractor(selectingInteractor));
                
                // Interactor.Process()
            }

            return;
        }

        private IEnumerator ToggleInteractor(IInteractor selectingInteractor)
        {
            // Disable
            selectingInteractor.Disable();
            
            // Wait for next frame
            yield return null;
            
            // Enable
            selectingInteractor.Enable();
        }
        
        
    }
}