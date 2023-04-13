namespace ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.Visibility
{
    /// <summary>
    /// Keeps track of the visibility of virtual environment appearance.
    ///
    /// To subscribe to events, listen to <see cref="VirtualEnvironmentVisibilityDidChange"/>
    /// </summary>
    public static class VirtualEnvironmentVisibility
    {
        // Should default to true.
        private static bool _visible = true;

        /// <summary>
        /// Should VirtualEnvironment elements currently be <see cref="Visible"/>?
        /// To subscribe to events, subscribe to <see cref="VirtualEnvironmentVisibilityDidChange"/>
        /// </summary>
        public static bool Visible
        {
            get => _visible;
            set
            {
                // Don't do anything, if that value is the same.
                if (_visible == value) return;

                // Invoke event
                FireVirtualEnvironmentVisibilityDidChange(_visible, value);
                // Save value
                _visible = value;
            }
        }

        /// <summary>
        /// Changes in the visibility of the virtual environment
        /// </summary>
        public static event VisibilityEvents.VisibilityChanged VirtualEnvironmentVisibilityDidChange;

        private static void FireVirtualEnvironmentVisibilityDidChange(bool previousValue, bool newVisibleValue)
        {
            VirtualEnvironmentVisibilityDidChange?.Invoke(previousValue, newVisibleValue);
        }
    }
}