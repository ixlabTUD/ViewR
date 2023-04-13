using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.Core.UI.FloatingUI.ModalWindow
{
    /// <summary>
    /// A quick class to change button color on the modal window.
    /// Can be overwritten by assigning an local modal window panel
    /// </summary>
    public class TweenButtonColorModalWindow : MonoBehaviour
    {
        [Header("Setup")]
        public Color targetButtonColorSuccess;
        
        [Header("Optional")]
        [Help("To use locally, assign a value here. Else will address the global modal window.")]
        [SerializeField]
        private ModalWindowPanel localModalWindowPanel;


        #region Button Color changes

        public void ChangeConfirmButtonColor()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenConfirmButtonColor(targetButtonColorSuccess);
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenConfirmButtonColor(targetButtonColorSuccess);
        }

        public void ChangeDeclineButtonColor()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenDeclineButtonColor(targetButtonColorSuccess);
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenDeclineButtonColor(targetButtonColorSuccess);
        }

        public void ChangeAlternateButton1Color()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenAlternateButton1Color(targetButtonColorSuccess);
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenAlternateButton1Color(targetButtonColorSuccess);
        }

        public void ChangeAlternateButton2Color()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenAlternateButton2Color(targetButtonColorSuccess);
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenAlternateButton2Color(targetButtonColorSuccess);
        }

        public void ChangeAlternateButton3Color()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenAlternateButton3Color(targetButtonColorSuccess);
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenAlternateButton3Color(targetButtonColorSuccess);
        }
        
        public void ChangeConfirmButtonColorAndResetOthers()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenConfirmButtonColor(targetButtonColorSuccess);
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenConfirmButtonColor(targetButtonColorSuccess);
            
            // Reset others
            // ChangeConfirmButtonColorToInitialColor();
            ChangeDeclineButtonColorToInitialColor();
            ChangeAlternateButton1ColorToInitialColor();
            ChangeAlternateButton2ColorToInitialColor();
            ChangeAlternateButton3ColorToInitialColor();
        }

        public void ChangeDeclineButtonColorAndResetOthers()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenDeclineButtonColor(targetButtonColorSuccess);
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenDeclineButtonColor(targetButtonColorSuccess);
            
            // Reset others
            ChangeConfirmButtonColorToInitialColor();
            // ChangeDeclineButtonColorToInitialColor();
            ChangeAlternateButton1ColorToInitialColor();
            ChangeAlternateButton2ColorToInitialColor();
            ChangeAlternateButton3ColorToInitialColor();
        }

        public void ChangeAlternateButton1ColorAndResetOthers()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenAlternateButton1Color(targetButtonColorSuccess);
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenAlternateButton1Color(targetButtonColorSuccess);
            
            // Reset others
            ChangeConfirmButtonColorToInitialColor();
            ChangeDeclineButtonColorToInitialColor();
            // ChangeAlternateButton1ColorToInitialColor();
            ChangeAlternateButton2ColorToInitialColor();
            ChangeAlternateButton3ColorToInitialColor();
        }

        public void ChangeAlternateButton2ColorAndResetOthers()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenAlternateButton2Color(targetButtonColorSuccess);
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenAlternateButton2Color(targetButtonColorSuccess);
            
            // Reset others
            ChangeConfirmButtonColorToInitialColor();
            ChangeDeclineButtonColorToInitialColor();
            ChangeAlternateButton1ColorToInitialColor();
            // ChangeAlternateButton2ColorToInitialColor();
            ChangeAlternateButton3ColorToInitialColor();
        }

        public void ChangeAlternateButton3ColorAndResetOthers()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenAlternateButton3Color(targetButtonColorSuccess);
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenAlternateButton3Color(targetButtonColorSuccess);
            
            // Reset others
            ChangeConfirmButtonColorToInitialColor();
            ChangeDeclineButtonColorToInitialColor();
            ChangeAlternateButton1ColorToInitialColor();
            ChangeAlternateButton2ColorToInitialColor();
            // ChangeAlternateButton3ColorToInitialColor();
        }

        public void ChangeConfirmButtonColorToInitialColor()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenConfirmButtonColorToInitialColor();
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenConfirmButtonColorToInitialColor();
        }

        public void ChangeDeclineButtonColorToInitialColor()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenDeclineButtonColorToInitialColor();
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenDeclineButtonColorToInitialColor();
        }

        public void ChangeAlternateButton1ColorToInitialColor()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenAlternateButton1ColorToInitialColor();
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenAlternateButton1ColorToInitialColor();
        }

        public void ChangeAlternateButton2ColorToInitialColor()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenAlternateButton2ColorToInitialColor();
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenAlternateButton2ColorToInitialColor();
        }

        public void ChangeAlternateButton3ColorToInitialColor()
        {
            if (localModalWindowPanel != null)
                localModalWindowPanel.TweenAlternateButton3ColorToInitialColor();
            else
                ModalWindowUIController.Instance.ModalWindowPanel.TweenAlternateButton3ColorToInitialColor();
        }

        #endregion
    }
}
