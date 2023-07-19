using System.Collections;
using UnityEngine;
using ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.Visibility;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;
using ViewR.StatusManagement.States;

namespace ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.ReactToVisibility
{
    /// <summary>
    /// Reacts to changes in <see cref="VirtualEnvironmentVisibility"/>s <see cref="VirtualEnvironmentVisibility.VirtualEnvironmentVisibilityDidChange"/> event.
    /// Ensures objects have their materials when needed and that they get the <see cref="passthroughMaterial"/> on all their shared materials whenever required.
    ///
    /// You can use <see cref="AddPassthroughMaterialSwapperToChildren"/> to use this dynamically.
    /// </summary>
    [RequireComponent(typeof(Renderer)), DisallowMultipleComponent]
    public class EnvironmentPassthroughMaterialsSwapper : MonoBehaviour
    {
        [SerializeField]
        private Material passthroughMaterial;

        public bool applyValueOnEnable = true;

        private Material[] _defaultMaterials;
        private Material[] _passthroughMaterials;
        private Renderer _renderer;
        private bool _initialized;
        
        [ReadOnly]
        public VirtualEnvironmentRepresentation currentVirtualEnvironmentRepresentation;
        
        [SerializeField, ReadOnly]
        private VirtualEnvironmentRepresentation previousVirtualEnvironmentRepresentation; 
        public VirtualEnvironmentRepresentation PreviousVirtualEnvironmentRepresentation => previousVirtualEnvironmentRepresentation;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            VirtualEnvironmentVisibility.VirtualEnvironmentVisibilityDidChange += HandleVisibilityChanges;
        }

        private void OnEnable()
        {
            if (applyValueOnEnable)
                ApplyNewVisibleValue(VirtualEnvironmentVisibility.Visible);
        }

        private void Initialize(bool overwrite = false)
        {
            // Bail if already initialized
            if (_initialized && !overwrite)
                return;
            
            // Get refs
            _renderer = GetComponent<Renderer>();
            
            // Fetch values
            _defaultMaterials = _renderer.sharedMaterials;
            
            // Create new array
            _passthroughMaterials = new Material[_defaultMaterials.Length];
            for (var i = 0; i < _passthroughMaterials.Length; i++)
                _passthroughMaterials[i] = passthroughMaterial;

            _initialized = true;
        }

        private void OnDestroy()
        {
            VirtualEnvironmentVisibility.VirtualEnvironmentVisibilityDidChange -= HandleVisibilityChanges;
        }

        private void HandleVisibilityChanges(bool previousValue, bool newVisibleValue)
        {
            Initialize();

            ApplyNewVisibleValue(newVisibleValue);
        }

        private void ApplyNewVisibleValue(bool newVisibleValue)
        {
            switch (newVisibleValue)
            {
                case true:
                    BecameVirtual();
                    break;
                case false:
                    BecamePassthrough();
                    break;
            }
        }

        public void BecamePassthrough()
        {
            if (!_renderer)
                Initialize();
            
            if (_passthroughMaterials[0] == null)
            {
                StartCoroutine(WaitForMaterial());
                return;
            }

            _renderer.sharedMaterials = _passthroughMaterials;
        }

        public void BecameVirtual()
        {
            if (!_renderer)
                Initialize();

            _renderer.sharedMaterials = _defaultMaterials;
        }

        public void InjectPassthroughMaterial(Material newPassthroughMaterial)
        {
            passthroughMaterial = newPassthroughMaterial;
            if (_initialized)
                for (var i = 0; i < _passthroughMaterials.Length; i++)
                    _passthroughMaterials[i] = newPassthroughMaterial;
            else
                Initialize();
        }
        
        IEnumerator WaitForMaterial()
        {

            yield return new WaitForSeconds(.1f);
            BecamePassthrough();

        }
    }
}