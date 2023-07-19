using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewR.Managers;

namespace ViewR.Core.UI.MainUI
{
    /// <summary>
    /// Greets the user and allows them to configure their name.
    /// </summary>
    public class WelcomeUserByName : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField]
        private string notYouButtonText = "Not you? Change it here.";
        [SerializeField]
        private Color notYouButtonColor;
        [SerializeField]
        private string configureNameButtonText = "What's your name?";
        [SerializeField]
        private Color configureNameButtonColor;
        [Header("References")]
        [SerializeField]
        private TMP_Text welcomeTextBox;
        [SerializeField]
        private Button configureNameButton;
        
        
        private const string WelcomeText = "Welcome";

        private TMP_Text _buttonText;
        private Image _buttonImage;
    
        private void Awake()
        {
            // get refs
            _buttonText = configureNameButton.GetComponentInChildren<TMP_Text>();
            _buttonImage = configureNameButton.GetComponent<Image>();
        }

        public void UpdateWelcomeText(string newText)
        {
            welcomeTextBox.text = newText;
        }
    
        private void OnEnable()
        {
            if(PlayerPrefs.HasKey(PlayerPrefsAccessors.PREFS_USERNAME))
            {
                UpdateWelcomeText(WelcomeText + " " + PlayerPrefs.GetString(PlayerPrefsAccessors.PREFS_USERNAME) + "!");
                UpdateButton(true);
            }
            else
            {
                UpdateWelcomeText(WelcomeText + "!");
                UpdateButton(false);
            }
        }

        private void UpdateButton(bool knowUserName)
        {
            _buttonText.text = knowUserName ? notYouButtonText : configureNameButtonText;
            _buttonImage.color = knowUserName ? notYouButtonColor : configureNameButtonColor;
        }
    }
}
