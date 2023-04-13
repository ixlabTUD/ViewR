using UnityEngine;
using ViewR.Core.Networking.Normcore.RealtimeInstanceManagement;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;

namespace ViewR.Core.Networking.Normcore.Connection
{
    /// <summary>
    /// An extended version of <see cref="ConnectViaNormcore"/> to allow us to access any instance we'd like!
    /// </summary>
    public class ConnectViaNormcoreInstance : RealtimeReferencer
    {
        [Header("Debugging")]
        [SerializeField]
        private bool showGUIOverlay;

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void ConnectToDefaultRoom()
        {
            RealtimeToUse.Connect(RealtimeToUse.roomToJoinOnStart);
        }
        
        public void Connect(string roomName)
        {
            RealtimeToUse.Connect(roomName);
        }
        

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void Disconnect()
        {
            RealtimeToUse.Disconnect();
        }
    }
}
