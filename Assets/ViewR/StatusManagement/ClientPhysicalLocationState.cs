namespace ViewR.StatusManagement
{
    /// <summary>
    /// Whether the client connects from remote or from on location. 
    /// </summary>
    public enum ClientPhysicalLocation
    {
        OnSite,
        Remote
    }

    /// <summary>
    /// Allows to keep track of our devices location / connection.
    /// Other methods can subscribe to changes.
    /// </summary>
    public static class ClientPhysicalLocationState
    {
        public static ClientPhysicalLocation CurrentClientPhysicalLocation { get; private set; } = ClientPhysicalLocation.OnSite;
        
        
        #region Events

        public delegate void PropertyChangedHandler(ClientPhysicalLocation clientPhysicalLocation);
        public static event PropertyChangedHandler ClientPhysicalLocationDidChange;

        #endregion

        /// <summary>
        /// Sets the current status and fires the respective event. <see cref="ClientPhysicalLocationDidChange"/>
        /// </summary>
        public static void SetStatus(ClientPhysicalLocation clientPhysicalLocation)
        {
            CurrentClientPhysicalLocation = clientPhysicalLocation;
            ClientPhysicalLocationDidChange?.Invoke(CurrentClientPhysicalLocation);
        }
    }
}