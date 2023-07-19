using Normal.Realtime;
using Pixelplacement;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ViewR.Managers
{
    /// <summary>
    /// A class to store references and basic calls to the main networking engine.
    /// </summary>
    public class NetworkManager : SingletonExtended<NetworkManager>
    {
        [Header("Networking")]
        [SerializeField]
        private Realtime mainRealtimeInstance; 
        public Realtime MainRealtimeInstance => mainRealtimeInstance;
        
        
        [SerializeField]
        private RealtimeAvatarManager realtimeAvatarManager; 
        public RealtimeAvatarManager RealtimeAvatarManager => realtimeAvatarManager;

        /// <summary>
        /// Gets the <see cref="Realtime.roomToJoinOnStart"/>
        /// </summary>
        public string GetRoomNameToJoin() => mainRealtimeInstance.roomToJoinOnStart;
    }
}