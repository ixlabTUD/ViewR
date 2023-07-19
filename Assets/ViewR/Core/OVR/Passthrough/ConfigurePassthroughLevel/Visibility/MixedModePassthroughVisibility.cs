namespace ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.Visibility
{
    public static class MixedModePassthroughVisibility
    {
        // Should default to true.
        private static bool _visible = true;

        /// <summary>
        /// Should Objects currently be <see cref="Visible"/>?
        /// To subscribe to events, subscribe to <see cref="MixedModePassthroughVisibilityDidChange"/>
        /// </summary>
        public static bool Visible
        {
            get => _visible;
            set
            {
                // Don't do anything, if that value is the same.
                if (_visible == value) return;

                // Invoke event
                FireMixedModePassthroughVisibilityDidChange(_visible, value);
                // Save value
                _visible = value;
            }
        }
        
        /// <summary>
        /// Changes in the visibility of the workspace
        /// </summary>
        public static event VisibilityEvents.VisibilityChanged MixedModePassthroughVisibilityDidChange;

        private static void FireMixedModePassthroughVisibilityDidChange(bool previousValue, bool newVisibleValue)
        {
            MixedModePassthroughVisibilityDidChange?.Invoke(previousValue, newVisibleValue);
        }
    }
}