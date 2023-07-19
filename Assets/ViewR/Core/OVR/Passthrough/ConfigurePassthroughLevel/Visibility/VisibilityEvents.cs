namespace ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.Visibility
{
    /// <summary>
    /// Event delegate to manage the visibility of elements within the environment
    /// </summary>
    public static class VisibilityEvents
    {
        public delegate void VisibilityChanged(bool previousValue, bool newVisibleValue);
    }
}