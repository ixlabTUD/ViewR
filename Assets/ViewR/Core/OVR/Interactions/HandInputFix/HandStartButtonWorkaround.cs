using System;
using UnityEngine;
using UnityEngine.Events;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.Managers;

namespace ViewR.Core.OVR.Interactions.HandInputFix
{
    /// <summary>
    /// --- OBSOLETE --- Oculus fixed their buggy part, this is no longer needed. 
    /// 
    /// As "OVRInput.Get[GetDown/GetUp](OVRInput.RawButton.Start, OVRInput.Controller.LHand);" seems currently broken, this is a quick work around.
    ///
    /// We check for the system gesture and whether the index finger is pinching.
    /// We then fire our event shortly before the OVR Start button triggers (or should trigger, at least), because this sets the systemGesture to be inactive for a frame or two.
    /// We furthermore provide both, a Unity Event for editor references, and a static event 
    /// </summary>
    /// <remarks>
    /// - There should be only one of these existing, as the static event will allow others to subscribe.
    /// - Only fires Unity event if called from this instance.
    /// </remarks>
    [DisallowMultipleComponent, Obsolete("This workaround is no longer needed, as Oculus has fixed their bug.")]
    public class HandStartButtonWorkaround : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent systemGestureCalled;
        
        public delegate void PropertyChangedHandler(bool buttonDown, HandStartButtonWorkaround handStartButtonWorkaround);
        public static event PropertyChangedHandler SystemGestureCalled;

        private const float FadeTime = 1.2f;

        [Header("Debugging")]
        [Help("Workaround for OVRInput.Get[GetDown/GetUp](OVRInput.Button.Start).\nSimply subscribe to its static event.")]
        [SerializeField]
        private bool debugging;

        private float _timer = 0.0f;
        private OVRHand _ovrHand;

        private void Update()
        {
            if (!_ovrHand)
            {
                _ovrHand = OvrReferenceManager.Instance.LeftOvrHand;
                if (!_ovrHand)
                    return;
            }

            // Testing:
            // var state = OVRPlugin.GetControllerState4((uint) OVRInput.Controller.Hands);
            // var menuGesture = (state.Buttons & (uint) OVRInput.RawButton.Start) > 0;
            // var testInput01 = OVRInput.Get(OVRInput.RawButton.Start, OVRInput.Controller.LHand);
            // var testInput02 = OVRInput.GetDown(OVRInput.RawButton.Start, OVRInput.Controller.LHand);

            var systemGesture = _ovrHand.IsSystemGestureInProgress;
            var indexPinching = _ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            
            if(!(systemGesture && indexPinching))
            {
                // Reset if needed
                if (_timer > 0) Reset();

                // Bail!
                return;
            }
            
            // Upp timer!
            if (_timer <= FadeTime)
                _timer += Time.deltaTime;
            // Or invoke and reset.
            else
            {
                FireSystemGestureCalled(true);
                Reset();
            }
        }

        private void Reset()
        {
            _timer = 0;
        }

        private void FireSystemGestureCalled(bool buttonDown)
        {
            SystemGestureCalled?.Invoke(buttonDown, this);
        }

        private void OnEnable()
        {
            SystemGestureCalled += SystemGestureWasCalled;
        }

        private void OnDisable()
        {
            SystemGestureCalled -= SystemGestureWasCalled;
        }

        /// <summary>
        /// Only fires Unity event if called from this instance.
        /// </summary>
        private void SystemGestureWasCalled(bool buttonDown, HandStartButtonWorkaround source)
        {
            if(source != this)
                return;
            
            if(debugging) Debug.Log($"SystemGestureTest: was called: {buttonDown}");
            systemGestureCalled?.Invoke();
        }
    }
}