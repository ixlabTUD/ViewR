using Normal.Realtime;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore.Voice
{
    /// <summary>
    /// Obsolete.
    /// Used to debug muting and voice issues.
    /// Replaced by <see cref="ViewR.HelpersLib.NetworkHelpers.DebugInformationToWindow"/>.
    /// </summary>
    [RequireComponent(typeof(RealtimeAvatarVoice))]
    public class DebugRealtimeVoiceVolume : MonoBehaviour
    {

        [Header("ReadOnly:")]
        [ReadOnly,SerializeField]
        TMPro.TextMeshProUGUI _tmp;
        [ReadOnly, SerializeField]
        private bool muted;
        [ReadOnly, SerializeField]
        private bool logging;
        [ReadOnly, SerializeField]
        private float volume;

        private RealtimeAvatarVoice realtimeAvatarVoice;
        private bool firstTimeWasCalled;

        private void Awake()
        {
            // Get references
            if (!TryGetComponent(out realtimeAvatarVoice)) throw new MissingComponentException("This monobehaviour requires an \"RealtimeAvatarVoice\" component");

            // Get tmp
            Transform tmpHolder = NetworkManager.Instance.MainRealtimeInstance.gameObject.transform.FindChildRecursive("DebugFloatFieldTMP");
            if (tmpHolder != null)
            {
                if (!tmpHolder.TryGetComponent(out _tmp))
                    throw new MissingComponentException("Couldn't find the needed TMP.");
            }
            else
            {
                Destroy(this);
            }
        }

        //private void Start()
        //{
        //    Debug.Log($"DebugRealtimeVoiceVolume.Start: Listening to connection to room.");
        //    UberManager.Instance.realtime.didConnectToRoom += InvokeLogging;
        //    UberManager.Instance.realtime.didDisconnectFromRoom += Realtime_didDisconnectFromRoom;
        //}

        //private void Realtime_didDisconnectFromRoom(Realtime realtime)
        //{
        //    UberManager.Instance.realtime.didConnectToRoom -= InvokeLogging;
        //    UberManager.Instance.realtime.didDisconnectFromRoom -= Realtime_didDisconnectFromRoom;
        //}

        private void Update()
        {
            logging = NetworkManager.Instance.MainRealtimeInstance.connected;
            //if(logging && !firstTimeWasCalled)
            //{
            //    firstTimeWasCalled = true;
            //    InvokeLogging(UberManager.Instance.realtime);
            //}
            if (logging)
            {
                muted = realtimeAvatarVoice.mute;
                volume = realtimeAvatarVoice.voiceVolume;
                _tmp.text = volume.ToString();
            }
        }

        private void InvokeLogging(Realtime realtime)
        {
            logging = true;

            //    Debug.Log("DebugRealtimeVoiceVolume.InvokeLogging: Starting to log voice volume!");
            //    InvokeRepeating("LogVoiceVolume", 1f, 0.5f);
        }

        //private void LogVoiceVolume()
        //{
        //    Debug.Log($"DebugRealtimeVoiceVolume.LogVoiceVolume: Volume: {realtimeAvatarVoice.voiceVolume}.");
        //}

    }
}