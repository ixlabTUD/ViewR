using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using ViewR.HelpersLib.OVRExtensions;

namespace ViewR.Core.OVR.Interactions
{
    /// <summary>
    /// Allows us to have static methods for the <see cref="InteractableUnityEventWrapper"/> events.
    /// </summary>
    public class InteractableUnityEventWrapperStaticEvents : MonoBehaviour
    {
        [SerializeField]
        private InteractableUnityEventWrapper interactableUnityEventWrapper;
        [SerializeField]
        private SyntheticHandFader syntheticHandFader;
        
        public static UnityEvent WhenHover;
        public static UnityEvent WhenUnhover;
        public static UnityEvent WhenSelect;
        public static UnityEvent WhenUnselect;
        public static UnityEvent WhenInteractorsCountUpdated;
        public static UnityEvent WhenSelectingInteractorsCountUpdated;
        
        
        protected bool _started = false;

        #region Invokers of static UnityEvents

        private void OnWhenHover() => WhenHover?.Invoke();
        private void OnWhenUnhover() => WhenUnhover?.Invoke();
        private void OnWhenSelect() => WhenSelect?.Invoke();
        private void OnWhenUnselect() => WhenUnselect?.Invoke();
        private void OnWhenInteractorsCountUpdated() => WhenInteractorsCountUpdated?.Invoke();
        private void OnWhenSelectingInteractorsCountUpdated() => WhenSelectingInteractorsCountUpdated?.Invoke();

        #endregion

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            Assert.IsNotNull(interactableUnityEventWrapper);
            this.EndStart(ref _started);
            
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                interactableUnityEventWrapper.WhenHover.AddListener(OnWhenHover);
                interactableUnityEventWrapper.WhenUnhover.AddListener(OnWhenUnhover);
                interactableUnityEventWrapper.WhenSelect.AddListener(OnWhenSelect);
                interactableUnityEventWrapper.WhenUnselect.AddListener(OnWhenUnselect);
                interactableUnityEventWrapper.WhenInteractorViewAdded.AddListener(OnWhenInteractorsCountUpdated);
                interactableUnityEventWrapper.WhenInteractorViewRemoved.AddListener(OnWhenInteractorsCountUpdated);
                interactableUnityEventWrapper.WhenSelectingInteractorViewAdded.AddListener(OnWhenSelectingInteractorsCountUpdated);
                interactableUnityEventWrapper.WhenSelectingInteractorViewRemoved.AddListener(OnWhenSelectingInteractorsCountUpdated);
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                interactableUnityEventWrapper.WhenHover.RemoveListener(OnWhenHover);
                interactableUnityEventWrapper.WhenUnhover.RemoveListener(OnWhenUnhover);
                interactableUnityEventWrapper.WhenSelect.RemoveListener(OnWhenSelect);
                interactableUnityEventWrapper.WhenUnselect.RemoveListener(OnWhenUnselect);
                interactableUnityEventWrapper.WhenInteractorViewAdded.RemoveListener(OnWhenInteractorsCountUpdated);
                interactableUnityEventWrapper.WhenInteractorViewRemoved.RemoveListener(OnWhenInteractorsCountUpdated);
                interactableUnityEventWrapper.WhenSelectingInteractorViewAdded.RemoveListener(OnWhenSelectingInteractorsCountUpdated);
                interactableUnityEventWrapper.WhenSelectingInteractorViewRemoved.RemoveListener(OnWhenSelectingInteractorsCountUpdated);
            }
        }

    }
}