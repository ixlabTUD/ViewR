using UnityEngine;
using UnityEngine.UI;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.Core.UI.FloatingUI.ModalWindow
{
    [RequireComponent(typeof(AspectRatioFitter), typeof(Image))]
    public class SetAspectRatioFitterToImage : MonoBehaviour
    {
        [Header("References")]
        [Help("If this is assigned, it will recalculate the ratio in case the image was changed when configuring the window. If none is assigned, you may call the Fit() method whenever required.\nAlso runs in the Editor onValidate if debug is toggled.")]
        [SerializeField]
        private ModalWindowPanel modalWindowPanel;
        [SerializeField]
        private bool fitOnEnable = true;

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;
        
        private AspectRatioFitter _aspectRatioFitter;
        private Image _image;

        private void Awake()
        {
            // Get refs
            _aspectRatioFitter = GetComponent<AspectRatioFitter>();
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            if (fitOnEnable)
                Fit();
            
            if(modalWindowPanel)
                modalWindowPanel.newWindowWasSetUp.AddListener(NewWindowWasConfigured);
        }

        private void OnDisable()
        {
            if(modalWindowPanel)
                modalWindowPanel.newWindowWasSetUp.RemoveListener(NewWindowWasConfigured);
        }

        private void NewWindowWasConfigured()
        {
            Fit();
        }

        public void Fit()
        {
            _aspectRatioFitter.aspectRatio = _image.sprite.rect.width / _image.sprite.rect.height;
        }
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            if(debugging)
            {
                Awake();
                Fit();
            }
        }
#endif
    }
}
