using Normal.Realtime;
using UnityEngine;

namespace ViewR.Core.OVR.UX
{
    public class MuteOnStart : MonoBehaviour
    {
        [SerializeField]
        private bool muteOnStart;
        [SerializeField]
        private RealtimeAvatarVoice realtimeAvatarVoice;

        private void Start()
        {
            if(muteOnStart)
                realtimeAvatarVoice.mute = true;
        }
    }
}
