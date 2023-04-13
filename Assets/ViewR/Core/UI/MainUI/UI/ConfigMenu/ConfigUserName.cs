#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewR.Managers;
using Random = UnityEngine.Random;

namespace ViewR.Core.UI.MainUI.UI.ConfigMenu
{
    /// <summary>
    /// Class that provides access to the main menu elements.
    /// It will update fields, assign functionality to the Main UI buttons
    /// </summary>
    public class ConfigUserName :
#if PHOTON_UNITY_NETWORKING
        MonoBehaviourPunCallbacks
#else
        MonoBehaviour
#endif
    {
        [Header("UI Refs")]
        [SerializeField]
        private TMP_InputField userNameTMP;

        [SerializeField]
        private Image emptyNameWarning;

        [SerializeField]
        private TextMeshProUGUI emptyNameWarningTMP;
#if PHOTON_UNITY_NETWORKING
        [SerializeField]
        private bool _usePhoton = false;
#endif
        
        public delegate void PropertyChangedHandler(string newValue);
        public static event PropertyChangedHandler UserNameDidChange;

        /// <summary>
        /// Essentially gets the user name and makes up a random one if it was none found.
        /// </summary>
#if PHOTON_UNITY_NETWORKING
        public override void OnEnable()
#else
        public void OnEnable()
#endif
        {
#if PHOTON_UNITY_NETWORKING
            base.OnEnable();
#endif

            // Add events
            userNameTMP.onValueChanged.AddListener(UpdateUserName);

            // Nickname: either load their previous user name, get one from Photon or generate a generic one.
            var nickname = "";
            // If it is a returning user:
            if (PlayerPrefs.HasKey(PlayerPrefsAccessors.PREFS_USERNAME))
                nickname = PlayerPrefs.GetString(PlayerPrefsAccessors.PREFS_USERNAME);

#if PHOTON_UNITY_NETWORKING
            // If the stored name was empty
            if (nickname.Equals(string.Empty))
                // either use online nickname or generate one!
                nickname = PhotonNetwork.LocalPlayer.NickName == string.Empty
                    ? "User" + Random.Range(0, 9999)
                    : PhotonNetwork.LocalPlayer.NickName;
#endif
            // If the stored name was empty
            if (nickname.Equals(string.Empty))
                nickname = "User" + Random.Range(0, 9999);

            // Use it
            userNameTMP.text = nickname;

#if PHOTON_UNITY_NETWORKING
            if (_usePhoton)
                PhotonNetwork.LocalPlayer.NickName = nickname;
#endif

            // Store it
            PlayerPrefs.SetString(PlayerPrefsAccessors.PREFS_USERNAME, nickname);
        }

        private void OnDisable()
        {
            // Unsubscribe
            userNameTMP.onValueChanged.RemoveAllListeners();
        }

        /// <summary>
        /// Whenever the value of the text box changes, this will update the users name.
        /// ! Only if the text is not empty.
        /// </summary>
        /// <param name="newPlayerName"></param>
        private void UpdateUserName(string newPlayerName)
        {
            // if field is empty, do not allow them to join
            if (newPlayerName == String.Empty)
            {
                ToggleWarning(true);
            }
            // allow them to join and save name
            else
            {
                ToggleWarning(false);

#if PHOTON_UNITY_NETWORKING
                // save the name
                if (_usePhoton)
                    PhotonNetwork.LocalPlayer.NickName = newPlayerName;
#endif

                PlayerPrefs.SetString(PlayerPrefsAccessors.PREFS_USERNAME, newPlayerName);

                // Fire away!
                InvokeUserNameDidChange(newPlayerName);
            }
        }

        private void ToggleWarning(bool displayWarning)
        {
            emptyNameWarningTMP.enabled = emptyNameWarning.enabled = displayWarning;
        }

        private static void InvokeUserNameDidChange(string newValue)
        {
            UserNameDidChange?.Invoke(newValue);
        }
    }
}