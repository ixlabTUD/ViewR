using UnityEngine;
using ViewR.HelpersLib.Extensions.General;
using ViewR.HelpersLib.Universals.Files;
using ViewR.Utils.Helpers;

namespace ViewR.Tools.CSVWriter
{
    public static class FileConfiguration
    {
        private static string _randomSessionID;

        public static string RandomSessionID
        {
            get
            {
                if (_randomSessionID != null)
                    return _randomSessionID;

                return _randomSessionID = RandomExtensions.RandomAlphaNumericString(6);
            }
        }
    
        /// <summary>
        /// Takes the file path WITHOUT FILE ENDING and extends it by:
        /// - Device Type
        /// - DateTime
        /// - Random alphanumeric id to allow for batch device control without 
        /// </summary>
        public static string ExtendFileNameByDeviceDateSessionID(string fileNameWithoutEnding)
        {
            // Configure Path variable
#if OCULUSINTEGRATION_PRESENT
            // Extend by DeviceID
            fileNameWithoutEnding += "-DeviceID-" + OVRHelpers.FetchQuestModelID();
#endif
            // Extend by Date
            fileNameWithoutEnding = fileNameWithoutEnding.ExtendByDateTimeNow("yyyy-MM-dd--HH-mm-ss");
            
            // Extend by Random ID
            fileNameWithoutEnding += "-" + RandomSessionID;

            return fileNameWithoutEnding;
        }
        
        

        public static string ConfigurePath(string fileNameInPersistentPath, FileTypeCheckConfig fileTypeCheckConfig, string parentingFolder = "", bool debugging = false)
        {
            var path = PersistentAppPathAccessor.GetPersistentFilePath(
                fileNameInPersistentPath,
                fileTypeCheckConfig,
                parentingFolder);
            if (debugging)
                Debug.Log($"Writing to file at path: {path}");
            return path;
        }
    }
}