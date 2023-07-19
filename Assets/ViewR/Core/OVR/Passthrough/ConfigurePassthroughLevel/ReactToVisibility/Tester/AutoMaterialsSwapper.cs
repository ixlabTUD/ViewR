using UnityEngine;

namespace ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.ReactToVisibility.Tester
{
    /// <summary>
    /// Just a test class to test material assignment.
    /// </summary>
    public class AutoMaterialsSwapper : MonoBehaviour
    {
        [SerializeField]
        private Material[] materials1;
        [SerializeField]
        private Material[] materials2;
        [SerializeField]
        private Material[] materials3;
        [SerializeField]
        private float changeInterval = 0.33F;
        [SerializeField]
        private bool debugging;

        private Renderer _rend;

        private void OnValidate()
        {
            if (TryGetComponent(out _rend))
            {
                var length = _rend.sharedMaterials.Length;

                materials1 ??= new Material[length];

                materials2 ??= new Material[length];

                materials3 ??= new Material[length];
            }
        }

        private void Awake()
        {
            _rend = GetComponent<Renderer>();
        }

        private void Update()
        {
            var index = Mathf.FloorToInt(Time.time / changeInterval);

            index = index % 3;
            
            if (debugging)
                Debug.Log("Index:" + index);
            
            _rend.sharedMaterials = index switch
            {
                0 => materials1,
                1 => materials2,
                _ => materials3
            };
        }
    }
}