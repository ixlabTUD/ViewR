using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ViewR.Core.UI.FloatingUI.Loading.Visuals
{
    /// <summary>
    /// Method that allows us to set the value of the Circle-Progress bar by script efficiently via get/set.
    /// </summary>
    [ExecuteInEditMode]
    public class CircleProgressBar : MonoBehaviour
    {
        [SerializeField] private RectTransform fxHolder;
        [SerializeField] private Image circleFill;
        [SerializeField] private TMP_Text progressText;

        [SerializeField] private float progress = 0f;

        public float Progress
        {
            get => progress;
            set
            {
                progress = value;

                UpdateVisuals(value);
            }
        }

        private void UpdateVisuals(float value)
        {
            circleFill.fillAmount = value;
            progressText.text = Mathf.FloorToInt(value * 100).ToString();
            fxHolder.rotation = Quaternion.Euler(new Vector3(0, 0, -value * 360));
        }
    }
}