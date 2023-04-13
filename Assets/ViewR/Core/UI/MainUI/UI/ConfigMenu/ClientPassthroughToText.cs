using TMPro;
using UnityEngine;
using ViewR.StatusManagement;
using ViewR.StatusManagement.Listeners;

namespace ViewR.Core.UI.MainUI.UI.ConfigMenu
{
    /// <summary>
    /// Displays the current Passthrough level.
    /// </summary>
    public class ClientPassthroughToText : ClientPassthroughLevelListener
    {
        [SerializeField]
        private TMP_Text textField;
        
        protected override void OnEnable()
        {
            base.OnEnable();

            UpdateVisuals();
        }

        protected override void HandlePassthroughLevelUpdate(PassthroughLevel newPassthroughLevel)
        {
            base.HandlePassthroughLevelUpdate(newPassthroughLevel);
            
            UpdateVisuals(newPassthroughLevel);
        }

        private void UpdateVisuals(PassthroughLevel? newPassthroughLevel = null)
        {
            // Catch if null:
            newPassthroughLevel ??= ClientPassthroughLevel.CurrentPassthroughLevel;

            textField.text = newPassthroughLevel.ToString();
        }
    }
}