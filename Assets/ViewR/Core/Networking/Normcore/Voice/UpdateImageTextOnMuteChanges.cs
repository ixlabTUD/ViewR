using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewR.Core.Networking.Normcore.Voice.Settings;

namespace ViewR.Core.Networking.Normcore.Voice
{
    /// <summary>
    /// Updates images and / or texts based on changes in mute settings.
    /// Relies on changes made to Mute through <see cref="MuteUnmuteMic"/>.
    ///
    ///  Also ensures these values are displayed currently on enable.
    /// </summary>
    public class UpdateImageTextOnMuteChanges : ReactToMuteChanges
    {
        [Header("Setup")]
        [SerializeField]
        private TMP_Text tmpTextField;
        [SerializeField]
        private Image micImage;

        
        [Header("References")]
        [SerializeField]
        private Sprite activeMicIcon;
        [SerializeField]
        private Sprite inactiveMicIcon;
        [SerializeField]
        private string activeMicText = "Mute";
        [SerializeField]
        private string inactiveMicText = "Unmute";
        
        [Header("Debugging")]
        [SerializeField]
        private bool debugging;


        protected override void OnEnableCallback(bool muted)
        {
            base.OnEnableCallback(muted);
            UpdateValues(muted);
        }

        protected override void HandleMuteChanges(object sender, MuteUnmuteMic.OnMuteChangedEventArgs eventArgs)
        {
            base.HandleMuteChanges(sender, eventArgs);
            UpdateValues(eventArgs.Muted);
        }

        private void UpdateValues(bool muted)
        {
            if (debugging)
                Debug.Log($"{nameof(UpdateImageTextOnMuteChanges)}.{nameof(HandleMuteChanges)}: Changing to muted: {muted}", this);

            //! Update local icons in UIs
            micImage.sprite = !muted ? activeMicIcon : inactiveMicIcon;
            tmpTextField.text = !muted ? activeMicText : inactiveMicText;
        }
    }
}