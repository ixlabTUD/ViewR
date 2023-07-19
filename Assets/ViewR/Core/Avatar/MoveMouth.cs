using Normal.Realtime;
using UnityEngine;
#if PHOTON_UNITY_NETWORKING
using Photon.Voice.PUN;
#endif

namespace ViewR.Core.Avatar
{
    public class MoveMouth : MonoBehaviour
    {
        public Transform mouth;

        [SerializeField]
        private bool useNormcore;

        private RealtimeAvatarVoice _voiceNormcore;
#if PHOTON_UNITY_NETWORKING
        private PhotonVoiceView _voicePun;
#endif
        private float _mouthSize;

        private void Awake()
        {
            // Get a reference to the RealtimeAvatarVoice component
            _voiceNormcore = GetComponent<RealtimeAvatarVoice>();
#if PHOTON_UNITY_NETWORKING
            _voicePun = GetComponent<PhotonVoiceView>();
            if (!_voiceNormcore && _voicePun) useNormcore = false;
#endif
        }

        private void Update()
        {
            // Use the current voice volume (a value between 0 - 1) to calculate the target mouth size (between 0.1 and 1.0)
#if PHOTON_UNITY_NETWORKING
            var targetMouthSize = useNormcore
                ? Mathf.Lerp(0.1f, 1.0f, _voiceNormcore.voiceVolume)
                : Mathf.Lerp(0.1f, 1.0f, _voicePun.RecorderInUse.LevelMeter.CurrentAvgAmp);
#else
            var targetMouthSize = Mathf.Lerp(0.1f, 1.0f, _voiceNormcore.voiceVolume);
#endif

            // Animate the mouth size towards the target mouth size to keep the open / close animation smooth
            _mouthSize = Mathf.Lerp(_mouthSize, targetMouthSize, 30.0f * Time.deltaTime);

            // Apply the mouth size to the scale of the mouth geometry
            Vector3 localScale = mouth.localScale;
            localScale.y = _mouthSize;
            mouth.localScale = localScale;
        }
    }
}
