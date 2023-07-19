using System;
using Oculus.Interaction;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;

namespace ViewR.Utils.Debugging.Oculus
{
    /// <summary>
    /// A simple testing class for <see cref="InteractableUnityEventWrapper"/> to do it without the hassle of starting everything in the Quest :)
    /// </summary>
    public class SimulateInteractableInput : MonoBehaviour
    {
        [SerializeField]
        private InteractableUnityEventWrapper interactableUnityEventWrapper;

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void InvokeWhenHover() => interactableUnityEventWrapper.WhenHover?.Invoke();

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void InvokeWhenUnhover() => interactableUnityEventWrapper.WhenUnhover?.Invoke();

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void InvokeWhenSelect() => interactableUnityEventWrapper.WhenSelect?.Invoke();

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void InvokeWhenUnselect() => interactableUnityEventWrapper.WhenUnselect?.Invoke();

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void InvokeWhenInteractorsCountUpdated() =>
            interactableUnityEventWrapper.WhenInteractorViewAdded?.Invoke();


#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void InvokeWhenSelectingInteractorsCountUpdated() =>
            interactableUnityEventWrapper.WhenSelectingInteractorViewAdded?.Invoke();

        
        /// <summary>
        /// Convenience feature.
        /// </summary>
        private void OnValidate()
        {
            if (!interactableUnityEventWrapper)
                TryGetComponent(out interactableUnityEventWrapper);
        }
    }
}