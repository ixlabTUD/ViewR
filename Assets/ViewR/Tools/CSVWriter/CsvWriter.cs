using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using ViewR.HelpersLib.Universals.Files;
using ViewR.Utils.Helpers;

namespace ViewR.Tools.CSVWriter
{
    public class CsvWriter : MonoBehaviour
    {
        [FormerlySerializedAs("filePathInPersistentPath")] 
        [SerializeField] private string fileNameInPersistentPath;
        [SerializeField] private FileTypeCheckConfig fileTypeCheckConfig;
        [SerializeField] private string parentingFolder = "csvs";

        public string[] csvHeaders;

        private string _deviceType;
        private bool _initialized;

        #region Data Format Configuration

        public static readonly CultureInfo CultInfo = new CultureInfo("en-US");
        protected string Path;
        public const string TimeFormat = "0.####";
        public const string PositionFormat = "0.#######";
        public const string RotationFormat = "0.#######";

        #endregion
        
        private void Start()
        {
            Initialize();
        }

        #region Initialization

        private void Initialize(bool forceOverwrite = false)
        {
            if (_initialized && !forceOverwrite)
                return;
            fileNameInPersistentPath = FileConfiguration.ExtendFileNameByDeviceDateSessionID(fileNameInPersistentPath);
            Path = FileConfiguration.ConfigurePath(fileNameInPersistentPath, fileTypeCheckConfig, parentingFolder, true);
            _initialized = true;
        }

        #endregion

        public void WriteToFile(IReadOnlyList<string> data, bool writeLine)
        {
            // Init if needed
            Initialize();
            
            // Fetch device type if not yet set
            if (string.IsNullOrEmpty(_deviceType))
                _deviceType = OVRHelpers.FetchQuestModelID();

            // Cache value
            var fileExisted = File.Exists(Path);

            // Append the file and Create if not existent yet.
            // Write header if first time:
            if (!fileExisted)
                DoWrite(csvHeaders, Path, true);

            // Write data
            DoWrite(data, Path, writeLine);
        }

        private static void DoWrite(IReadOnlyList<string> data, string path, bool writeLine)
        {
            using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                for (var i = 0; i < data.Count; i++)
                {
                    // if last one: skip last ","
                    if (i != data.Count - 1 || (data.Count == 1 && !writeLine))
                        writer.Write(data[i] + ",");
                    else
                        writer.WriteLine(data[i]);
                }
            }
        }
    }
}