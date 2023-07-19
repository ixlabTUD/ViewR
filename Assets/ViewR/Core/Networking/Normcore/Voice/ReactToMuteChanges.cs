using UnityEngine;
using ViewR.Core.Networking.Normcore.Voice.Settings;

namespace ViewR.Core.Networking.Normcore.Voice
{
    /// <summary>
    /// A basic class to react to mute changes
    /// Relies on changes made to Mute through <see cref="MuteUnmuteMic"/>.
    /// </summary>
    public class ReactToMuteChanges : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            MuteUnmuteMic.OnMuteChanged += HandleMuteChanges;
            
            // Ensure we show correctly:
            OnEnableCallback(MuteUnmuteMic.CurrentlyMuted);
        }
        protected virtual void OnDisable()
        {
            MuteUnmuteMic.OnMuteChanged -= HandleMuteChanges;
        }

        protected virtual void HandleMuteChanges(object sender, MuteUnmuteMic.OnMuteChangedEventArgs eventArgs)
        {
        }

        /// <summary>
        /// Method that is guaranteed to be called on <see cref="OnEnable"/> with the current <see cref="muted"/>. value.
        /// </summary>
        protected virtual void OnEnableCallback(bool muted)
        {
        }
    }
}