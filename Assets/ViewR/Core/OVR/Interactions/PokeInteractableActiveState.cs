using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Assertions;

namespace ViewR.Core.OVR.Interactions
{
    public class PokeInteractableActiveState : MonoBehaviour, IActiveState
    {
        private bool _currentlyInAction;
        public bool Active
        {
            get { return _currentlyInAction;}
        }

        [SerializeField]
        private PokeInteractor pokeInteractor;

        protected bool _started = false;

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(pokeInteractor);
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                pokeInteractor.WhenInteractableSelected.Action += WhenInteractableSelected;
                pokeInteractor.WhenInteractableUnselected.Action += WhenInteractableUnselected;
            }
        }


        private void WhenInteractableSelected(PokeInteractable obj) => _currentlyInAction = true;
        private void WhenInteractableUnselected(PokeInteractable obj) => _currentlyInAction = false;

        protected virtual void OnDisable()
        {
            if (_started)
            {
                if (_currentlyInAction)
                    _currentlyInAction = false;

                pokeInteractor.WhenInteractableSelected.Action -= WhenInteractableSelected;
                pokeInteractor.WhenInteractableUnselected.Action -= WhenInteractableUnselected;
            }
        }
    }
}