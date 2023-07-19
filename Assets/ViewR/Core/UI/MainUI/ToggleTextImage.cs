using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ViewR.Core.UI.MainUI
{
    public class ToggleTextImage : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField]
        private TMP_Text tmpTextField;
        [SerializeField]
        private Image micImage;

        public bool isSynced = true;
        
        [Header("References")]
        [SerializeField]
        private Sprite syncIcon;
        [SerializeField]
        private Sprite desyncIcon;
        [SerializeField]
        private string syncText = "Sync";
        [SerializeField]
        private string desyncText = "Desync";
        


        public void UpdateValues()
        {

            isSynced = !isSynced;

            //! Update local icons in UIs
            micImage.sprite = isSynced ? syncIcon : desyncIcon;
            tmpTextField.text = isSynced ? syncText : desyncText;
        }
    }
}
