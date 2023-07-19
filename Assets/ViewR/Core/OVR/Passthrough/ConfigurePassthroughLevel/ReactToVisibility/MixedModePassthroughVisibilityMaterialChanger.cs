using UnityEngine;
using ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.Visibility;

namespace ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.ReactToVisibility
{
    /// <summary>
    /// Sets the material separate from <see cref="EnvironmentPassthroughMaterialsSwapper"/>.
    ///
    /// If <see cref="MixedModePassthroughVisibility"/> gets set to true, it will still show the PT, and thus overwrite <see cref="EnvironmentPassthroughMaterialsSwapper"/>
    /// 
    /// Could be separated from this, but would require more references.
    /// </summary>
    [RequireComponent(typeof(EnvironmentPassthroughMaterialsSwapper))]
    public class MixedModePassthroughVisibilityMaterialChanger : MonoBehaviour
    {
        [SerializeField]
        private bool applyValueOnEnable = true;
        
        private EnvironmentPassthroughMaterialsSwapper _materialsChangerEnvironment;
        
        private void Awake()
        {
            // Get refs
            _materialsChangerEnvironment = GetComponent<EnvironmentPassthroughMaterialsSwapper>();
        }

        private void OnEnable()
        {
            MixedModePassthroughVisibility.MixedModePassthroughVisibilityDidChange += HandleVisibilityChanges;

            if (applyValueOnEnable)
                ApplyNewVisibleValue(MixedModePassthroughVisibility.Visible);
        }

        private void OnDisable()
        {
            MixedModePassthroughVisibility.MixedModePassthroughVisibilityDidChange -= HandleVisibilityChanges;
        }

        private void HandleVisibilityChanges(bool previousValue, bool newVisibleValue)
        {
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

        private void BecamePassthrough()
        {
            _materialsChangerEnvironment.BecamePassthrough();
        }

        private void BecameVirtual()
        {
            _materialsChangerEnvironment.BecameVirtual();
        }
    }
}