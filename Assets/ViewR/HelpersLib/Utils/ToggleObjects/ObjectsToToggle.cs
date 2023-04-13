using UnityEngine;
using UnityEngine.Events;

namespace ViewR.HelpersLib.Utils.ToggleObjects
{
    [System.Serializable]
    public class ObjectsToToggle
    {
        public Renderer[] renderers;
        public Collider[] colliders;
        public MonoBehaviour[] monoBehaviours;
        public GameObject[] gameObjects;
        public UnityEvent<bool> wasToggled;
        public UnityEvent wasToggledOn;
        public UnityEvent wasToggledOff;

        /// <summary>
        /// Toggles all given components on
        /// </summary>
        public void ToggleOn()
        {
            Enable(true);
        }

        /// <summary>
        /// Toggles all given components off
        /// </summary>
        public void ToggleOff()
        {
            Enable(false);
        }

        /// <summary>
        /// Toggles all given components to the given <see cref="enable"/> value
        /// </summary>
        public void Enable(bool enable)
        {
            foreach (var renderer in renderers)
                renderer.enabled = enable;
            foreach (var collider in colliders)
                collider.enabled = enable;
            foreach (var monoBehaviour in monoBehaviours)
                monoBehaviour.enabled = enable;
            foreach (var o in gameObjects)
                o.SetActive(enable);
            
            //Invoke
            wasToggled?.Invoke(enable);
            if (enable)
                wasToggledOn?.Invoke();
            else
                wasToggledOff?.Invoke();
        }
    }
}