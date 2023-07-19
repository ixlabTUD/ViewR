using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;

namespace ViewR.HelpersLib.Utils.DrawOnTop
{
    /// <summary>
    /// A modified version of http://answers.unity.com/answers/1428061/view.html
    /// </summary>
    [ExecuteInEditMode]
    public class SetDepthComparisonOnUIMaterial : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.Rendering.CompareFunction desiredUIComparison = UnityEngine.Rendering.CompareFunction.Always;

        [SerializeField, Tooltip("Set to blank to automatically populate from the child UI elements")]
        private Graphic[] uiElementsToApplyTo;

        //Allows us to reuse materials
        private readonly Dictionary<Material, Material> _materialMappings = new Dictionary<Material, Material>();
        
        private const string ShaderTestMode = "unity_GUIZTestMode";
        private static readonly int UnityGuiZTestMode = Shader.PropertyToID(ShaderTestMode);

        protected virtual void Start()
        {
            Execute();
        }

        [ExposeMethodInEditor]
        private void Execute()
        {
            if (uiElementsToApplyTo == null || uiElementsToApplyTo.Length == 0)
                uiElementsToApplyTo = gameObject.GetComponentsInChildren<Graphic>(true);

            foreach (var graphic in uiElementsToApplyTo)
            {
                var material = graphic.materialForRendering;
                if (material == null)
                {
                    Debug.LogError(
                        $"{nameof(SetDepthComparisonOnUIMaterial)}: skipping target without material {graphic.name}.{graphic.GetType().Name}");
                    continue;
                }

                if (!_materialMappings.TryGetValue(material, out var materialCopy))
                {
                    materialCopy = new Material(material);
                    _materialMappings.Add(material, materialCopy);
                }

                materialCopy.SetInt(UnityGuiZTestMode, (int) desiredUIComparison);
                graphic.material = materialCopy;
            }
        }
    }
}