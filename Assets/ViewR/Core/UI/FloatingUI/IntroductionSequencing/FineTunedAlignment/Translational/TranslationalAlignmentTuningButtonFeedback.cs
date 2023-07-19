using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing.FineTunedAlignment.Translational
{
    public class TranslationalAlignmentTuningButtonFeedback : MonoBehaviour
    {
        [SerializeField]
        private Sprite activateSprite;

        [SerializeField]
        private Sprite deactivateSprite;

        [SerializeField]
        private string activateText = "Start Fine Tuning";

        [SerializeField]
        private string deactivateText = " End Fine Tuning ";

        [SerializeField]
        private TMP_Text tmpTextField;

        [SerializeField]
        private Image image;

        private void Start()
        {
            TranslationalAlignmentTuningManager.AlignmentTuningManagerStarted += HandleTuningStarted;
            TranslationalAlignmentTuningManager.AlignmentTuningManagerEnded += HandleTuningEnded;
        }

        private void OnDestroy()
        {
            TranslationalAlignmentTuningManager.AlignmentTuningManagerStarted -= HandleTuningStarted;
            TranslationalAlignmentTuningManager.AlignmentTuningManagerEnded -= HandleTuningEnded;
        }

        private void OnEnable()
        {
            Show(TranslationalAlignmentTuningManager.Active);
        }

        private void HandleTuningStarted(TranslationalAlignmentTuningManager translationalAlignmentTuningManager)
        {
            Show(true);
        }

        private void HandleTuningEnded(TranslationalAlignmentTuningManager translationalAlignmentTuningManager)
        {
            Show(false);
        }

        private void Show(bool active)
        {
            if (image)
                image.sprite = active ? deactivateSprite : activateSprite;
            if (tmpTextField)
                tmpTextField.text = active ? deactivateText : activateText;
        }
    }
}