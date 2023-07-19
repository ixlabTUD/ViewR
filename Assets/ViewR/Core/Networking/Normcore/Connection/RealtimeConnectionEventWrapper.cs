using Normal.Realtime;
using UnityEngine;
using UnityEngine.Events;
using ViewR.Core.Networking.Normcore.RealtimeInstanceManagement;

namespace ViewR.Core.Networking.Normcore.Connection
{
    /// <summary>
    /// A wrapper class to simplify events on connect.
    /// Provides both, inspector events and public events for easy access from both realms ;) 
    /// </summary>
    public class RealtimeConnectionEventWrapper : RealtimeReferencer
    {
        [SerializeField]
        private UnityEvent didConnect;

        [SerializeField]
        private UnityEvent didDisconnect;

        [SerializeField]
        private UnityEvent noRealtimeFound;

        [SerializeField, Tooltip("Should invoke \"didConnect\" events on enable if we are currently connected?")]
        private bool runOnEnableIfConnected;

        [SerializeField, Tooltip("Should invoke \"didDisconnect\" events on enable if we are currently not connected?")]
        private bool runOnEnableIfNotConnected;

        public delegate void ConnectionChangedHandler();

        public event ConnectionChangedHandler DidConnectRealtime;
        public event ConnectionChangedHandler DidDisconnectRealtime;
        public event ConnectionChangedHandler NoRealtimeFound;

        private void OnEnable()
        {
            if (RealtimeToUse != null)
            {
                RealtimeToUse.didConnectToRoom += InvokeDidConnectEvents;
                RealtimeToUse.didDisconnectFromRoom += InvokeDidDisconnectEvents;

                if (runOnEnableIfConnected && RealtimeToUse.connected)
                    InvokeDidConnectEvents(RealtimeToUse);
                if (runOnEnableIfNotConnected && !RealtimeToUse.connected)
                    InvokeDidDisconnectEvents(RealtimeToUse);
            }
            else
            {
                InvokeNoRealtimeFound();
            }
        }

        private void OnDisable()
        {
            RealtimeToUse.didConnectToRoom -= InvokeDidConnectEvents;
            RealtimeToUse.didDisconnectFromRoom -= InvokeDidDisconnectEvents;
        }

        private void InvokeDidConnectEvents(Realtime realtime1)
        {
            didConnect?.Invoke();
            DidConnectRealtime?.Invoke();
        }

        private void InvokeDidDisconnectEvents(Realtime realtime1)
        {
            didDisconnect?.Invoke();
            DidDisconnectRealtime?.Invoke();
        }

        protected virtual void InvokeNoRealtimeFound()
        {
            noRealtimeFound?.Invoke();
            NoRealtimeFound?.Invoke();
        }
    }
}