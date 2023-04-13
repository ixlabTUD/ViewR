using Normal.Realtime;
using UnityEngine;
using ViewR.Core.Networking.Normcore.RealtimeInstanceManagement;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Core.Networking.Normcore.Utils.ShowOnlineStatus
{
    public abstract class ShowOnlineStatus : RealtimeReferencer
    {
        [Header("Setup")]
        [SerializeField]
        internal Color onlineColor;
        [SerializeField]
        internal  Color offlineColor;

        [Header("Debugging")]
        [SerializeField]
        internal bool debugging;

        protected bool Initialized;

        
        private void Start()
        {
            InitialSetup();
        }

        private void InitialSetup()
        {
            if (RealtimeToUse == null)
            {
                Debug.LogWarning("No RealtimeToUse found.", this);
                return;
            }
            
            if(Initialized) return;
            
            RealtimeToUse.didConnectToRoom += ShowConnectedToRoom;
            RealtimeToUse.didDisconnectFromRoom += ShowDisconnectedFromRoom;

            // Catch if no realtime yet.
            if(RealtimeToUse == null)
            {
                Debug.LogWarning("No RealtimeToUse found.", this);
                ShowDisconnectedFromRoom(RealtimeToUse);
                return;
            }
            
            // Get current state:
            if(RealtimeToUse.connected)
                ShowConnectedToRoom(RealtimeToUse);
            else
                ShowDisconnectedFromRoom(RealtimeToUse);

            Initialized = true;
        }

        /// <summary>
        /// Ensure we show the right values.
        /// </summary>
        internal virtual void OnEnable()
        {
            if(!Initialized)
                InitialSetup();
                
            if(RealtimeToUse == null)
                // Catch
                ShowDisconnectedFromRoom(RealtimeToUse);
            else if(RealtimeToUse.connected)
                ShowConnectedToRoom(RealtimeToUse);
            else
                ShowDisconnectedFromRoom(RealtimeToUse);
        }

        internal virtual void OnDisable()
        {
            RealtimeToUse.didConnectToRoom -= ShowConnectedToRoom;
            RealtimeToUse.didDisconnectFromRoom -= ShowDisconnectedFromRoom;
            
            Initialized = false;
        }

        internal virtual void ShowConnectedToRoom(Realtime realtime)
        {
            if (debugging)
                Debug.Log("Connected!".Green().StartWithFrom(GetType()), this);
        }

        internal virtual void ShowDisconnectedFromRoom(Realtime realtime)
        {
            if (debugging)
                Debug.Log("Disconnected!".Green().StartWithFrom(GetType()), this);
        }
    }
}