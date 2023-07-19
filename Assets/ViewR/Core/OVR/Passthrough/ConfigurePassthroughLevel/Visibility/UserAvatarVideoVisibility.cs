namespace ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.Visibility
{
    /// <summary>
    /// Keeps track of the visibility of user appearance.
    ///
    /// To subscribe to events, listen to <see cref="UserVideoVisibilityDidChange"/>
    /// </summary>
    public static class UserAvatarVideoVisibility
    {
        // Should default to true.
        private static bool _visible = true;

        /// <summary>
        /// Should passthrough be <see cref="Visible"/> where users currently are?
        /// To subscribe to events, listen to <see cref="UserVideoVisibilityDidChange"/>
        /// </summary>
        public static bool Visible
        {
            get => _visible;
            set
            {
                // Don't do anything, if that value is the same.
                if (_visible == value) return;

                // Invoke event
                FireUserVideoVisibilityDidChange(_visible, value);
                // Save value
                _visible = value;
            }
        }

        /// <summary>
        /// Changes in the visibility of the UserVideo passthrough where the users are.
        /// </summary>
        public static event VisibilityEvents.VisibilityChanged UserVideoVisibilityDidChange;

        private static void FireUserVideoVisibilityDidChange(bool previousValue, bool newVisibleValue)
        {
            UserVideoVisibilityDidChange?.Invoke(previousValue, newVisibleValue);
        }

    }
}