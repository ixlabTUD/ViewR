namespace ViewR.Core.OVR.Interactions.Input
{
    /// <summary>
    /// A class that provides easy access to the Input state of OVR.
    /// </summary>
    public static class OVRInputState
    {
        private static OVRInput.Controller CurrentController => OVRInput.GetActiveController();

        public static bool ControllersActive =>
            CurrentController.Equals(OVRInput.Controller.Touch) ||
            CurrentController.Equals(OVRInput.Controller.LTouch) ||
            CurrentController.Equals(OVRInput.Controller.RTouch);
        
        public static bool HandsActive =>
            CurrentController.Equals(OVRInput.Controller.Hands) ||
            CurrentController.Equals(OVRInput.Controller.LHand) ||
            CurrentController.Equals(OVRInput.Controller.RHand);
    }
}