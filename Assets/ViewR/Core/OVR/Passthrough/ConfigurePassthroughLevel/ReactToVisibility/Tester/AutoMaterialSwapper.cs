// Mostly from http://answers.unity.com/answers/1375406/view.html

using UnityEngine;

namespace ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.ReactToVisibility.Tester
{
    /// <summary>
    /// Just a test class to test material assignment.
    /// </summary>
    public class AutoMaterialSwapper : MonoBehaviour
    {
        public Material[] materials;
        public float changeInterval = 0.33F;
        public Renderer rend;

        private void Start()
        {
            if(!rend)
                rend = GetComponent<Renderer>();
            rend.enabled = true;
        }

        private void Update()
        {
            if (materials.Length == 0)
                return;
 
            var index = Mathf.FloorToInt(Time.time / changeInterval);
 
            index = index % materials.Length;
 
            rend.sharedMaterial = materials[index];
        }
    }
}