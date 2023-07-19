using System;
using UnityEngine;

namespace ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.ReactToVisibility
{
    /// <summary>
    /// To ensure we can update the prefab of the scene geometry, we add the <see cref="EnvironmentPassthroughMaterialsSwapper"/> components through this script to all child objects that have a renderer. 
    /// </summary>
    public class AddPassthroughMaterialSwapperToChildren : MonoBehaviour
    {
        [SerializeField]
        private Material passthroughMaterial;

        private const bool ApplyValueOnEnable = true;


        /// <summary>
        /// Add <see cref="EnvironmentPassthroughMaterialsSwapper"/> to all children with renderers.
        /// </summary>
        public void AddComponentToAllChildrenWithRenderers()
        {
            var children = this.transform.GetComponentsInChildren<Renderer>(true);
            foreach (var child in children)
            {
                // Skip if the object already contains a swapper.
                if (child.TryGetComponent(out EnvironmentPassthroughMaterialsSwapper presentSwapper))
                    continue;
                
                var swapper = child.gameObject.AddComponent<EnvironmentPassthroughMaterialsSwapper>();
                
                swapper.InjectPassthroughMaterial(passthroughMaterial);
                swapper.applyValueOnEnable = ApplyValueOnEnable;
            }
        }
    }
}