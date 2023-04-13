using Normal.Realtime;
using UnityEngine;
using UnityEngine.UI;
using ViewR.Core.Networking.Normcore.RealtimeInstanceManagement;

namespace ViewR.Core.Networking.Normcore.Connection
{
    public class ToggleComponentsOnConnectionStateChange : RealtimeReferencer
    {
        [SerializeField]
        private Button[] buttonsToEnableOnConnected;
        [SerializeField]
        private Button[] buttonsToDisableOnConnected;
        [SerializeField]
        private GameObject[] gameObjectsToEnableOnConnected;
        [SerializeField]
        private GameObject[] gameObjectsToDisableOnConnected;
        [SerializeField]
        private MonoBehaviour[] monoBehavioursToEnableOnConnected;
        [SerializeField]
        private MonoBehaviour[] monoBehavioursToDisableOnConnected;
        [SerializeField]
        private Renderer[] renderersToEnableOnConnected;
        [SerializeField]
        private Renderer[] renderersToDisableOnConnected;

        private void OnEnable()
        {
            RealtimeToUse.didConnectToRoom += HandleDidConnect;
            RealtimeToUse.didDisconnectFromRoom += HandleDidDisconnect;
        }

        private void OnDisable()
        {
            RealtimeToUse.didConnectToRoom -= HandleDidConnect;
            RealtimeToUse.didDisconnectFromRoom -= HandleDidDisconnect;
        }
        
        private void HandleDidConnect(Realtime realtime)
        {
            ToggleElements(true);
        }

        private void HandleDidDisconnect(Realtime realtime)
        {
            ToggleElements(false);
        }

        private void ToggleElements(bool doEnable)
        {
            // Enable
            foreach (var button in buttonsToEnableOnConnected)
                button.interactable = doEnable;
            foreach (var monoBehaviour in monoBehavioursToEnableOnConnected)
                monoBehaviour.enabled = doEnable;
            foreach (var r in renderersToEnableOnConnected)
                r.enabled = doEnable;
            foreach (var o in gameObjectsToEnableOnConnected)
                o.SetActive(doEnable);

            // Disable
            foreach (var button in buttonsToDisableOnConnected)
                button.interactable = !doEnable;
            foreach (var monoBehaviour in monoBehavioursToDisableOnConnected)
                monoBehaviour.enabled = !doEnable;
            foreach (var r in renderersToDisableOnConnected)
                r.enabled = !doEnable;
            foreach (var o in gameObjectsToDisableOnConnected)
                o.SetActive(!doEnable);
        }
    }
}
