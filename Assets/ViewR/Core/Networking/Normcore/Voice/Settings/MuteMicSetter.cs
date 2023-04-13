using UnityEngine;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore.Voice.Settings
{
    public class MuteMicSetter : MonoBehaviour
    {
        public void Mute()
        {
            ReferenceManager.Instance.MuteUnmuteMic.Mute = true;
        }
        
        public void Unmute()
        {
            ReferenceManager.Instance.MuteUnmuteMic.Mute = false;
        }
        
        public void ToggleMute()
        {
            ReferenceManager.Instance.MuteUnmuteMic.ToggleMute();
        }
    }
}