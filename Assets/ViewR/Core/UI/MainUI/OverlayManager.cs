using Pixelplacement;
using UnityEngine;

namespace ViewR.Core.UI.MainUI
{
    public class OverlayManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainFrameBlocker;

        [SerializeField]
        private StateMachine stateMachine;
    
        public void ActivateOverlay(GameObject state)
        {
            throw new System.NotImplementedException();
        }

        private void DeactivateOverlay()
        {
            throw new System.NotImplementedException();
        }
    }
}
