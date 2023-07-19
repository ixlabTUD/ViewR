using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.HelpersLib.Utils.Scaling
{
    /// <summary>
    /// Uniform scaling based on distance and animation curve
    /// </summary>
    public class ScaleToDistance : MonoBehaviour
    {
        [Help("Takes the initial scale on awake and then multiplies it by the animation curves result and the scaleFactor.")]
        [Header("References")]
        [SerializeField]
        private Transform objectToScale;
        
        [SerializeField]
        private Transform objectA;
        
        [SerializeField]
        private Transform objectB;
        
        [Header("Settings")]
        [SerializeField]
        private AnimationCurve animationCurve;

        [SerializeField]
        private float scaleFactor = 1;

        private Vector3 _initialScale;
        private bool _initialized;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize(bool overwriteInit = false)
        {
            if (_initialized && !overwriteInit)
                return;
            
            _initialScale = objectToScale.localScale;

            _initialized = true;
        }

        private void Update()
        {
            // Calculate distance
            var distance = Vector3.Distance(objectA.position, objectB.position);

            var evaluatedScaleFactor = animationCurve.Evaluate(distance);

            objectToScale.localScale = _initialScale * evaluatedScaleFactor * scaleFactor;
        }

    }
}