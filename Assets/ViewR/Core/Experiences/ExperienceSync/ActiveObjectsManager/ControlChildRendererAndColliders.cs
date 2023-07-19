using UnityEngine;

namespace ViewR.Core.Experiences.ExperienceSync.ActiveObjectsManager
{
    /// <summary>
    /// A class to control child renderers and colliders active states.
    /// </summary>
    public class ControlChildRendererAndColliders : MonoBehaviour
    {
        [SerializeField]
        private bool deactivateOnAwake;
        [SerializeField]
        private bool activateOnAwake;
        [Header("Additional to components in children.")]
        [SerializeField]
        private Renderer[] additionalRenderers;
        [SerializeField]
        private Collider[] additionalColliders;
        
        private Renderer[] _renderers;
        private Collider[] _colliders;

        private void Awake()
        {
            // Get refs
            _renderers = GetComponentsInChildren<Renderer>(true);
            _colliders = GetComponentsInChildren<Collider>(true);

            // Hide on Awake
            if (deactivateOnAwake)
                ShowObjects(false);
            else if (activateOnAwake)
                ShowObjects(true);
        }


        /// <summary>
        /// Shows or hides the objects colliders and renderers, given <see cref="show"/>.
        /// </summary>
        public void ShowObjects(bool show)
        {
            foreach (var loopedRenderer in _renderers)
                loopedRenderer.enabled = show;
            foreach (var loopedCollider in _colliders)
                loopedCollider.enabled = show;
            foreach (var loopedRenderer in additionalRenderers)
                loopedRenderer.enabled = show;
            foreach (var loopedCollider in additionalColliders)
                loopedCollider.enabled = show;
        }

        public void DoShowObjects() => ShowObjects(true);
        public void DoHideObjects() => ShowObjects(false);

    }
}
