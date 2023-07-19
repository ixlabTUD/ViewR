namespace ViewR.Managers
{
    /// <summary>
    /// A place to store all references to PlayerPrefs to avoid searching and losing them.  
    /// </summary>
    /// <remarks>
    /// Example usage:
    ///      public static bool SomeBool
    ///      {
    ///          get
    ///         {
    ///              return PlayerPrefs.GetInt(PlayerPrefsAccessors.PREFS_WASACTIVE, 1) == 1;
    ///          }
    ///          set
    ///         {
    ///              PlayerPrefs.SetInt(PlayerPrefsAccessors.PREFS_WASACTIVE, value ? 1 : 0);
    ///          }
    ///      }
    /// </remarks>
    public static class PlayerPrefsAccessors
    {
        public const string PREFS_USERNAME = "playerName";
        public const string PREFS_HANDEDNESS = "Handedness";
        public const string PREFS_LAST_SEEN_UNIX = "LastSeenUnix";

        public static string PREFS_QUIT_TIME_UNIX => PREFS_LAST_SEEN_UNIX;
    }
}