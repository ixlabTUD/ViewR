using System;
using JetBrains.Annotations;
using Pixelplacement;
using UnityEngine;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Managers
{
    /// <summary>
    /// Saves user configs.
    /// </summary>
    /// ToDo: Potentially make this static?
    public class UserConfig : SingletonExtended<UserConfig>
    {
        // Cached value
        private OVRPlugin.Handedness? _handedness = null;
        /// <summary>
        /// Main handedness. 
        /// If handedness has not been set yet, get it from the playerprefs.
        /// If it hasn't been set there, log warning and set it to right as default.
        /// </summary>
        public OVRPlugin.Handedness Handedness
        {
            get
            {
                // If handedness has not been set yet, get it from the playerprefs.
                // If it hasn't been set there, log warning and set it to right as default.
                if (_handedness == null)
                {
                    if (PlayerPrefs.HasKey(PlayerPrefsAccessors.PREFS_HANDEDNESS))
                        return (OVRPlugin.Handedness) (_handedness =
                            (OVRPlugin.Handedness) PlayerPrefs.GetInt(PlayerPrefsAccessors.PREFS_HANDEDNESS));

                    Debug.LogWarning(
                        $"Please set the handedness first! Will assume right handedness for now.".StartWithFrom(
                            GetType()), this);
                    return (OVRPlugin.Handedness) (_handedness = OVRPlugin.Handedness.RightHanded);
                }
                // Otherwise, just use the stored value to avoid constant checking of playerprefs.
                else
                    return (OVRPlugin.Handedness) _handedness;
            }
            set
            {
                // Cache it.
                _handedness = value;
                // Store it.
                PlayerPrefs.SetInt(PlayerPrefsAccessors.PREFS_HANDEDNESS, (int)value);
                PlayerPrefs.Save();
            }
        }

        /// <summary>
        /// The main hand based on the users <see cref="handedness"/>
        /// </summary>
        public OVRPlugin.Hand MainHand
        {
            get
            {
                switch (Handedness)
                {
                    case OVRPlugin.Handedness.Unsupported:
                        Debug.LogError($"This should not be {OVRPlugin.Handedness.Unsupported}! Please set the handedness first!".StartWithFrom(GetType()), this);
                        return OVRPlugin.Hand.None;
                    case OVRPlugin.Handedness.LeftHanded:
                        return OVRPlugin.Hand.HandLeft;
                    case OVRPlugin.Handedness.RightHanded:
                        return OVRPlugin.Hand.HandRight;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool LeftHanded
        {
            get
            {
                if (MainHand == OVRPlugin.Hand.None)
                    Debug.LogError($"Please set the handedness first!".StartWithFrom(GetType()), this);

                return MainHand == OVRPlugin.Hand.HandLeft;
            }
        }

        public bool RightHanded
        {
            get
            {
                if (MainHand == OVRPlugin.Hand.None)
                    Debug.LogError($"Please set the handedness first!".StartWithFrom(GetType()), this);

                return MainHand == OVRPlugin.Hand.HandRight;
            }
        }

        /// <summary>
        /// Returns the user name stored in the PlayerPrefs.
        /// Note: Returns null if no user name is configured.
        /// </summary>
        [CanBeNull]
        public static string UserName =>
            PlayerPrefs.HasKey(PlayerPrefsAccessors.PREFS_USERNAME)
                ? PlayerPrefs.GetString(PlayerPrefsAccessors.PREFS_USERNAME)
                : null;
    }
}