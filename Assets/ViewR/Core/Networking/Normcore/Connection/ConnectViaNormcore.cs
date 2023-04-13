using UnityEngine;
using UnityEngine.Serialization;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore.Connection
{
    /// <summary>
    /// Connects to the room configured in the Realtime component (see ReferenceManager), when <see cref="Connect"/> is called.
    /// </summary>
    public class ConnectViaNormcore : MonoBehaviour
    {

        [Help("Optional. Will autopopulate if not given")]
        [SerializeField]
        private Normal.Realtime.Realtime realtime;
        [FormerlySerializedAs("debugging")]
        [Help(
            "Connects to the room configured in the Realtime component (see ReferenceManager), when Connect is called.")]
        [Header("Debugging")]
        [SerializeField]
        private bool showGUIOverlay;
    
        public void Connect()
        {
            if(!realtime)
                realtime = NetworkManager.Instance.MainRealtimeInstance;
        
            realtime.Connect(realtime.roomToJoinOnStart);
        }

#if UNITY_EDITOR
        // Show button if we are in the editor && Debugging is true.
        private void OnGUI()
        {
            if(!showGUIOverlay) return;
            
            if (GUILayout.Button("Connect"))
            {
                Connect();
            }

        }
#endif
        
    }
}
