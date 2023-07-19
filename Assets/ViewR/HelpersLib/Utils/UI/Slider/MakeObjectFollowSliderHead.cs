using UnityEngine;

namespace ViewR.HelpersLib.Utils.UI.Slider
{
    public class MakeObjectFollowSliderHead : MonoBehaviour
    {
        [SerializeField]
        private Transform[] transformsToReposition;
        [SerializeField]
        private Transform sliderHead;

        private void OnEnable()
        {
            UpdateObjectPosition(0f);
        }

        public void UpdateObjectPosition(float _)
        {
            foreach (var transformToReposition in transformsToReposition)
                transformToReposition.position = sliderHead.position;
        }
    }
}
