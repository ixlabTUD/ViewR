using UnityEngine;

namespace ViewR.StatusManagement
{
    /// <summary>
    /// The state of the passthrough setting.
    /// </summary>
    public enum PassthroughLevel
    {
        /// <summary>
        /// Video everywhere
        /// </summary>
        EntirelyPassthrough,
        /// <summary>
        /// Video everywhere except digital objects of relevance
        /// </summary>
        MostlyPassthrough,
        /// <summary>
        /// Passthrough level based on slider
        /// </summary>
        MixedMode,
        /// <summary>
        /// Video nowhere except for where other users are
        /// </summary>
        MostlyVirtual, 
        /// <summary>
        /// Video nowhere at all ( = remote mode, uses avatars)
        /// </summary>
        EntirelyVirtual
    }

    /// <summary>
    /// Allows to keep track of our passthrough level.
    /// Other methods can subscribe to changes.
    /// </summary>
    public static class ClientPassthroughLevel
    {
        private static PassthroughLevel _currentPassthroughLevel = PassthroughLevel.MostlyVirtual;
        /// <summary>
        /// Always returns "Entirely virtual" if the users <see cref="ClientPhysicalLocationState.CurrentClientPhysicalLocation"/> is <see cref="ClientPhysicalLocation.Remote"/>.
        /// </summary>
        public static PassthroughLevel CurrentPassthroughLevel
        {
            get
            {
                if(ClientPhysicalLocationState.CurrentClientPhysicalLocation == ClientPhysicalLocation.Remote)
                {
                    Debug.Log("ClientPassthroughLevel: Returning \"EntirelyVirtual\" because the client is joining remotely (Hint: Their ClientPhysicalLocationState.ClientPhysicalLocation is set to ClientPhysicalLocation.Remote).");
                    return PassthroughLevel.EntirelyVirtual;
                }
                else
                    return _currentPassthroughLevel;
            }
            private set
            {
                _currentPassthroughLevel = value;
                PassthroughLevelUpdated?.Invoke(CurrentPassthroughLevel);
            }
        }
        
        
        #region Events

        public delegate void PropertyChangedHandler(PassthroughLevel passthroughLevel);
        public static event PropertyChangedHandler PassthroughLevelUpdated;

        #endregion

        /// <summary>
        /// Sets the current status and fires the respective event. <see cref="PassthroughLevelUpdated"/>
        /// </summary>
        public static void SetStatus(PassthroughLevel passthroughLevel, bool forceOverwrite = false)
        {
            if (!forceOverwrite && CurrentPassthroughLevel == passthroughLevel)
                // Already there, bailing
                return;
            
            // Apply
            CurrentPassthroughLevel = passthroughLevel;
        }
        
    }
}