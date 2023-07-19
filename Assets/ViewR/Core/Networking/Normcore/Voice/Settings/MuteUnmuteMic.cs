using Normal.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewR.Core.Networking.Normcore.Avatar;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore.Voice.Settings
{
    /// <summary>
    /// This class Mutes/Unmutes the avatar.
    /// It also sends this state to the local avatars <see cref="RealtimeAvatarMuteSync"/> to sync this value.
    ///
    /// <see cref="ReactToMuteChanges"/>, <see cref="UpdateImageTextOnMuteChanges"/> and <see cref="MuteMicSetter"/> are just some examples using this class.
    /// </summary>
    public class MuteUnmuteMic : MonoBehaviour
    {
        #region Serialized Fields and Properties

        [SerializeField, Help("Can be auto-populated.")]
        private RealtimeAvatarManager realtimeAvatarManager;

        [Space]
        [SerializeField]
        private Sprite activeMicIcon;

        [SerializeField]
        private Sprite inactiveMicIcon;

        [Header("Optional:")]
        [SerializeField, Tooltip("An GameObject that gets activated when the user is muted. Can also be done via UpdateImageTextOnMuteChanges.")]
        private GameObject mutedInformationBar = null;

        [SerializeField, Tooltip("Images to update on mute changes.")]
        private Image[] micImagesToUpdate;

        [SerializeField, Tooltip("Text fields to update on mute changes. Can also be done via UpdateImageTextOnMuteChanges.")]
        private TMP_Text[] micTextFields;

        #endregion

        #region Private Members

        private Realtime _realtime;
        private RealtimeAvatarVoice _realtimeAvatarVoice;

        #endregion

        #region GetSet

        private bool _muted = false;

        /// <summary>
        /// Setting this value will Mute/Unmute the respective User
        /// </summary>
        public bool Mute
        {
            get => _muted;
            set
            {
                CurrentlyMuted = _muted = value ? DoMute() : DoUnmute();
                OnMuteChanged?.Invoke(this, new OnMuteChangedEventArgs {Muted = value});
            }
        }

        public static bool CurrentlyMuted { get; private set; }

        #endregion

        #region Public Members

        public static event System.EventHandler<OnMuteChangedEventArgs> OnMuteChanged;

        public class OnMuteChangedEventArgs : System.EventArgs
        {
            public bool Muted;
        }

        #endregion


        #region Unity specific methods

        private void Start()
        {
            // Ensure refs
            if (!_realtime)
            {
                _realtime = NetworkManager.Instance.MainRealtimeInstance;
                if (!_realtime)
                    throw new MissingReferenceException($"No {nameof(_realtime)} present.");
            }

            if (!realtimeAvatarManager)
            {
                realtimeAvatarManager = NetworkManager.Instance.RealtimeAvatarManager;
                if (!realtimeAvatarManager)
                    throw new MissingReferenceException($"No {nameof(realtimeAvatarManager)} present.");
            }

            Debug.Log($"MuteUnmuteMic.Start: Listening to connection to room.", this);

            _realtime.didConnectToRoom += Initialize;
            if (_realtime.connected)
                Initialize(_realtime);
        }

        private void OnDestroy()
        {
            _realtime.didConnectToRoom -= Initialize;
        }

        #endregion

        #region public methods

        public void ToggleMute()
        {
            // Call muting
            Mute = !Mute;
        }

        #endregion

        #region private methods

        private void Initialize(Realtime realtime)
        {
            Debug.Log($"{nameof(MuteUnmuteMic)}.{nameof(Initialize)}: Connected to room. Initializing");

            // Get references
            var localRealtimeAvatar = realtimeAvatarManager.localAvatar;

            // Refs: Voice
            _realtimeAvatarVoice = localRealtimeAvatar.GetComponent<AvatarAccessHelper>().RealtimeAvatarVoice;
            if (!_realtimeAvatarVoice)
                throw new MissingReferenceException(
                    $"Could not find {nameof(RealtimeAvatarVoice)} on {localRealtimeAvatar}.");

            // Initialize local muted value.
            Debug.Log($"{nameof(MuteUnmuteMic)}.{nameof(Initialize)}: Initializing muted to {_realtimeAvatarVoice.mute}.");

            _muted = _realtimeAvatarVoice.mute;
        }

        private bool DoMute()
        {
            Debug.Log("Muting...");
            // Do magic if we are online.
            if (_realtime.connected)
                realtimeAvatarManager.localAvatar.head.GetComponent<RealtimeAvatarVoice>().mute = true;
            // update settings image
            UpdateIcons(true);

            // Activate information
            if (mutedInformationBar)
                mutedInformationBar.SetActive(true);

            return true;
        }

        private bool DoUnmute()
        {
            Debug.Log("Unmuting...");
            // Do magic if we are online.
            if (_realtime.connected)
                realtimeAvatarManager.localAvatar.head.GetComponent<RealtimeAvatarVoice>().mute = false;
            // update settings image
            UpdateIcons(false);

            // Deactivate information
            if (mutedInformationBar)
                mutedInformationBar.SetActive(false);

            return false;
        }

        private void UpdateIcons(bool muting)
        {
            //! Update local icons in UIs
            // Update all mute buttons
            foreach (var micImage in micImagesToUpdate)
            {
                // Change image shown:
                // If not muting: offer to mute and show active mic icon
                // If muting: offer to unmute and show inactive mic icon
                micImage.sprite = !muting ? activeMicIcon : inactiveMicIcon;
            }

            foreach (var micTextField in micTextFields)
            {
                // Change image shown:
                micTextField.text = !muting ? "Mute" : "Unmute";
            }
        }

        #endregion
    }
}