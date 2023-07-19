namespace ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.Visibility
{
    /// <summary>
    /// Keeps track of the visibility of object appearance.
    /// 
    /// To subscribe to events, subscribe to <see cref="ObjectVisibilityDidChange"/>
    /// </summary>
    public static class ObjectVisibility
    {
        // Should default to true.
        private static bool _visible = true;

        /// <summary>
        /// Should Objects currently be <see cref="Visible"/>?
        /// To subscribe to events, subscribe to <see cref="ObjectVisibilityDidChange"/>
        /// </summary>
        public static bool Visible
        {
            get => _visible;
            set
            {
                // Don't do anything, if that value is the same.
                if (_visible == value) return;

                // Invoke event
                FireObjectVisibilityDidChange(_visible, value);
                // Save value
                _visible = value;
            }
        }
        
        /// <summary>
        /// Changes in the visibility of the workspace
        /// </summary>
        public static event VisibilityEvents.VisibilityChanged ObjectVisibilityDidChange;

        private static void FireObjectVisibilityDidChange(bool previousValue, bool newVisibleValue)
        {
            ObjectVisibilityDidChange?.Invoke(previousValue, newVisibleValue);
        }
    }
}