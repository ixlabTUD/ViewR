using Oculus.Interaction;
using UnityEngine;

namespace ViewR.Core.OVR.Quality
{
    /// <summary>
    /// A quick solution to update the toggles to the current value.
    /// </summary>
    public class MsaaToButtonToggle : MonoBehaviour
    {
        [SerializeField]
        private ToggleDeselect toggleTwo;
        [SerializeField]
        private ToggleDeselect toggleFour;
        
        private void OnEnable()
        {
            var currentMsaaLevel = QualitySettings.antiAliasing;

            if (currentMsaaLevel <= 2)
                toggleTwo.isOn = true;
            else
                toggleFour.isOn = true;
        }
    }
}