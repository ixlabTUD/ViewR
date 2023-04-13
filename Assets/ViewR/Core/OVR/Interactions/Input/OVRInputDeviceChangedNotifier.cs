using System;
using UnityEngine;

namespace ViewR.Core.OVR.Interactions.Input
{
    /// <summary>
    /// A basic class that allows us to get simple events for switching of input modalities! 
    /// 
    /// Update loop that efficiently should primarily compare if our CurrentController is =! our previous one.
    /// Only runs logic if changed, and even than only runs two switches and overwrites the previous value.
    /// </summary>
    public class OVRInputDeviceChangedNotifier : MonoBehaviour
    {
        private OVRInput.Controller _previousController = OVRInput.Controller.None;
        private static OVRInput.Controller CurrentController => OVRInput.GetActiveController();
        
        public static event Action ControllersActivated;
        public static event Action ControllersDeactivated;
        public static event Action HandsActivated;
        public static event Action HandsDeactivated;

        // Update loop that efficiently should primarily compare if our CurrentController is =! our previous one.
        // Only runs logic if changed, and even than only runs two switches and overwrites the previous value. 
        private void Update()
        {
            // Bail if no changes
            if(CurrentController == _previousController)
                // Dont do anything
                return;
            
            switch (CurrentController)
            {
                case OVRInput.Controller.None:
                case OVRInput.Controller.Remote:
                case OVRInput.Controller.Gamepad:
                case OVRInput.Controller.Active:
                case OVRInput.Controller.All:
                    // Deactivate whatever was previously active
                    InvokeDeactivated(_previousController);
                    break;
                case OVRInput.Controller.LTouch:
                case OVRInput.Controller.RTouch:
                case OVRInput.Controller.Touch:
                    switch (_previousController)
                    {
                        case OVRInput.Controller.None:
                        case OVRInput.Controller.All:
                        case OVRInput.Controller.Active:
                        case OVRInput.Controller.Remote:
                        case OVRInput.Controller.Gamepad:
                            InvokeControllersActivated();
                            break;
                        case OVRInput.Controller.Hands:
                        case OVRInput.Controller.LHand:
                        case OVRInput.Controller.RHand:
                            InvokeHandsDeactivated();
                            InvokeControllersActivated();
                            break;
                        case OVRInput.Controller.LTouch:
                        case OVRInput.Controller.RTouch:
                        case OVRInput.Controller.Touch:
                            // No significant changes
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case OVRInput.Controller.Hands:
                case OVRInput.Controller.LHand:
                case OVRInput.Controller.RHand:
                    switch (_previousController)
                    {
                        case OVRInput.Controller.None:
                        case OVRInput.Controller.All:
                        case OVRInput.Controller.Active:
                        case OVRInput.Controller.Remote:
                        case OVRInput.Controller.Gamepad:
                            InvokeHandsActivated();
                            break;
                        case OVRInput.Controller.Hands:
                        case OVRInput.Controller.LHand:
                        case OVRInput.Controller.RHand:
                            // No significant changes
                            break;
                        case OVRInput.Controller.LTouch:
                        case OVRInput.Controller.RTouch:
                        case OVRInput.Controller.Touch:
                            InvokeControllersDeactivated();
                            InvokeHandsActivated();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _previousController = CurrentController;
        }

        public static bool ControllerIsController(OVRInput.Controller controller)
        {
            return 
                controller.Equals(OVRInput.Controller.Touch) ||
                controller.Equals(OVRInput.Controller.LTouch) ||
                controller.Equals(OVRInput.Controller.RTouch);
        }

        public static bool ControllerIsHands(OVRInput.Controller controller)
        {
            return 
                controller.Equals(OVRInput.Controller.Hands) ||
                controller.Equals(OVRInput.Controller.LHand) ||
                controller.Equals(OVRInput.Controller.RHand);
        }

        #region Invokers

        private void InvokeDeactivated(OVRInput.Controller previousController)
        {
            if (ControllerIsHands(_previousController))
                InvokeHandsDeactivated();
            else if (ControllerIsController(_previousController))
                InvokeControllersDeactivated();
        }

        private static void InvokeControllersActivated()
        {
            ControllersActivated?.Invoke();
        }

        private static void InvokeControllersDeactivated()
        {
            ControllersDeactivated?.Invoke();
        }

        private static void InvokeHandsActivated()
        {
            HandsActivated?.Invoke();
        }

        private static void InvokeHandsDeactivated()
        {
            HandsDeactivated?.Invoke();
        }

        #endregion
    }
}