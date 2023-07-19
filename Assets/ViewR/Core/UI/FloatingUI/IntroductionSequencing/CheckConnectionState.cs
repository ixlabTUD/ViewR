using System.Collections;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.Managers;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing
{
    /// <summary>
    /// Changes the state to <see cref="startConnectingState"/> or <see cref="alreadyOnlineState"/> given whether we are already online or not.
    /// Shows the window for <see cref="showWindowDuration"/> before changing states.
    /// </summary>
    public class CheckConnectionState : TutorialTask
    {
        [Header("Setup")]
        [SerializeField]
        private GameObject startConnectingState;
        [SerializeField]
        private GameObject alreadyOnlineState;
        [SerializeField]
        private float showWindowDuration = 3.25f;
        
        [Help("Optional. Will auto-populate if not given")]
        [SerializeField]
        private Normal.Realtime.Realtime realtime;
        
        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        internal override void OnEnable()
        {
            base.OnEnable();
            
            if(!realtime)
                realtime = NetworkManager.Instance.MainRealtimeInstance;

            StartCoroutine(CheckForConnectionAndWaitABit());
        }

        /// <summary>
        /// Waits for a bit to not just flash open/close, and then checks for connection and changes the state.  
        /// </summary>
        private IEnumerator CheckForConnectionAndWaitABit()
        {
            yield return new WaitForSeconds(showWindowDuration);

            ChangeState(!realtime.connected ? startConnectingState : alreadyOnlineState);
        }
    }
}
