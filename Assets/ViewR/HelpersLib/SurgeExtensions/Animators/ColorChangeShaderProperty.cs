using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions.ScriptableObjects;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;

namespace ViewR.HelpersLib.SurgeExtensions.Animators
{
    /// <summary>
    /// Picks a random color from array and tweens towards it :)
    /// </summary>
    public class ColorChangeShaderProperty : MonoBehaviour
    {
        [SerializeField] private Color[] defaultColor;
        [SerializeField, Tooltip("Flashes to this and holds it for a short delay to allow for a visual effect.")] private Color interactionStartColor;
        [SerializeField] private Color[] interactionColor;
        [SerializeField] private Material material;
        [SerializeField] private string shaderProperty = "_Color";
        [Header("Tween")]
        [SerializeField] private float interactionStartDelay = 0.1f;
        [SerializeField] private TweenConfigShader tweenConfigShader;
        [SerializeField] private TweenConfigShader tweenConfigShaderInteractions;
        [Header("Debugging")]
        [SerializeField] private bool debugging;
        [SerializeField, ReadOnly] private Tween.TweenStatus tweenStatus ;

        private TweenBase _tweenBase ;

        private void OnEnable()
        {
            GlowNormal();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if(debugging && _tweenBase != null)
                tweenStatus = _tweenBase.Status;
        }
#endif

        private void OnDisable()
        {
            TryToStop();
        }

        public void StartInteraction()
        {
            if(debugging) Debug.Log("<b>MarkerColorChanger</b>: Started INTERACTION Color Change", this);
                    
            TryToStop();
            
            // GlowInteraction();
            _tweenBase = Tween.ShaderColor(material, shaderProperty, interactionStartColor, interactionColor[0], tweenConfigShader.Duration, interactionStartDelay, tweenConfigShader.AnimationCurve, tweenConfigShader.loopType, completeCallback: GlowInteraction);
        }

        private void GlowInteraction()
        {
            TryToStop();
            
            var chosenColor = Random.Range(0, interactionColor.Length);
            _tweenBase = Tween.ShaderColor(material, shaderProperty, interactionColor[chosenColor], tweenConfigShaderInteractions.Duration, tweenConfigShaderInteractions.Delay, tweenConfigShaderInteractions.AnimationCurve, tweenConfigShaderInteractions.loopType, completeCallback: GlowInteraction);
        }

        public void StopInteraction()
        {
            if(debugging) Debug.Log("<b>MarkerColorChanger</b>: Started NORMAL Color Change, Stopped interaction", this);

            TryToStop();

            _tweenBase = Tween.ShaderColor(material, shaderProperty, defaultColor[Random.Range(0, defaultColor.Length)], 1, tweenConfigShaderInteractions.Delay, tweenConfigShaderInteractions.AnimationCurve, tweenConfigShaderInteractions.loopType, completeCallback: GlowNormal);
        }
        
        private void GlowNormal()
        {
            if(debugging) Debug.Log($"<b>MarkerColorChanger</b>: Started NORMAL Color Change on {shaderProperty}", this);

            TryToStop();
            
            _tweenBase = Tween.ShaderColor(material, shaderProperty, defaultColor[Random.Range(0, defaultColor.Length)], tweenConfigShader.Duration, tweenConfigShader.Delay, tweenConfigShader.AnimationCurve, tweenConfigShader.loopType, completeCallback: GlowNormal);
        }


        private void TryToStop()
        {
            // Stop ongoing color changes
            Tween.Stop(this.GetInstanceID(), Tween.TweenType.ShaderColor);
            if(_tweenBase != null)
                _tweenBase.Stop();
        }
        
    }
}

