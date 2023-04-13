using System.Collections;
using Normal.Realtime;
using UnityEngine;
using ViewR.Core.UI.FloatingUI.ModalWindow;
using ViewR.HelpersLib.SurgeExtensions.Animators.UI;
using ViewR.HelpersLib.SurgeExtensions.StateMachineExt;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore.Connection
{
    public class DisplayNormcoreConnectionState : StateExtended
    {
        private Realtime _realtime;
        private ModalWindowPanel _modalWindowPanel;
        private Coroutine _routine;
        private UIFadeInOutGraphicCanvasGroup _uiFadeInOutGraphicCanvasGroup;

        
        private void Start() 
        {
            Setup();
        }

        private void OnEnable()
        {
            Setup();
            
            // Subscribe
            _realtime.didConnectToRoom += DidConnectToRoom;
        }
    
        private void OnDisable() 
        {
            // Unsubscribe
            _realtime.didConnectToRoom -= DidConnectToRoom;
        
            if(_routine != null)
                StopCoroutine(_routine);
        }

        private void Setup()
        {
            // Get refs
            _realtime = NetworkManager.Instance.MainRealtimeInstance;
            _modalWindowPanel = ModalWindowUIController.Instance.ModalWindowPanel;
            _uiFadeInOutGraphicCanvasGroup = _modalWindowPanel.fillText.GetComponent<UIFadeInOutGraphicCanvasGroup>();
            
            // Disable button
            _modalWindowPanel.confirmButton.interactable = false;

            _routine = StartCoroutine(UpdateTextDelayed());
        }

        private void DidConnectToRoom(Realtime realtime)
        {
            _modalWindowPanel.UpdateWindow("Successfully connected!");

            // // Enable button
            // _modalWindowPanel.confirmButton.interactable = true;

            DisplaySuccess(true);

            // Reset:
            _routine = null;
        }
        
        private void DisplaySuccess(bool spawned)
        {
            // Set button and text
            _modalWindowPanel.confirmButton.interactable = spawned;
            
            // Display it:
            _modalWindowPanel.fillHorizontalTransform.gameObject.SetActive(true);
            _modalWindowPanel.fillText.gameObject.SetActive(false);
            _modalWindowPanel.fillImage.gameObject.SetActive(true);
            
            // Appear CheckMark
            _modalWindowPanel.uiFillAnimation.Appear(spawned);
            // Appear Text
            _uiFadeInOutGraphicCanvasGroup.Appear(spawned);
            
            // Ensure its all displayed correctly:
            _modalWindowPanel.modalWindowUpdateContentFitterLayout.RecalculateLayouts();
        }
    
        /// <summary>
        /// Keeps the user informed, should the connection be slow
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateTextDelayed()
        {
            yield return new WaitForSeconds(2.5f);
            // Break if we are connected
            if(_realtime.connected) yield break;
            _modalWindowPanel.UpdateWindow("Still on it...");
        
            yield return new WaitForSeconds(4);
            // Break if we are connected
            if(_realtime.connected) yield break;
            _modalWindowPanel.UpdateWindow("It takes just a bit longer...");
        
            yield return new WaitForSeconds(4);
            // Break if we are connected
            if(_realtime.connected) yield break;
            _modalWindowPanel.UpdateWindow("Still trying...");
        }
    
    }
}
