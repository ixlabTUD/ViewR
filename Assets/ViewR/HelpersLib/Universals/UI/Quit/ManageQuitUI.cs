using UnityEngine;

namespace ViewR.HelpersLib.Universals.UI.Quit
{
    /// <summary>
    /// Bit more specific, but may still come in handy!
    /// </summary>
    public class ManageQuitUI : MonoBehaviour
    {
        [SerializeField, Tooltip("Canvas that prompts confirmation for quitting.")] 
        private GameObject quitYesNo;
        [SerializeField, Tooltip("Blocks the main UI from being interacted with while this is active.")] 
        private GameObject mainFrameBlocker;

        public void PromptQuitConfirmation()
        {
            mainFrameBlocker.SetActive(true);
            quitYesNo.SetActive(true);
        }
        
        public void CancelQuitting()
        {
            mainFrameBlocker.SetActive(false);
            quitYesNo.SetActive(false);
        }
    }
}
