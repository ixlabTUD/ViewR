namespace ViewR.StatusManagement.States
{
    /// <summary>
    /// Representation of the users Video Passthrough.
    /// </summary>
    public enum UserRepresentationType
    {
        HeadOnly,
        GeometricPrimitive,
        IK
    }

    public static class UserRepresentation
    {
        public static UserRepresentationType CurrentUserRepresentationType { get; private set; } = UserRepresentationType.GeometricPrimitive;

        #region Static Events

        public delegate void AvatarVisualChangedHandler(UserRepresentationType newUserRepresentationType);

        public static event AvatarVisualChangedHandler CurrentUserRepresentationDidChange;

        /// <summary>
        /// Use this static method to query a new visual representation of the avatar according to <see cref="userRepresentation"/>
        ///
        /// All avatars will subscribe to this event, thus calling it will invoke it on all avatars.
        /// </summary>
        public static void SetNewAvatarVisual(UserRepresentationType newUserRepresentationType)
        {
            if (CurrentUserRepresentationType == newUserRepresentationType)
                return;
            
            CurrentUserRepresentationType = newUserRepresentationType;
            CurrentUserRepresentationDidChange?.Invoke(newUserRepresentationType);
        }

        #endregion
    }
}