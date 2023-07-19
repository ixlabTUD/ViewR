using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;
using ViewR.Core.Experiences.ExperienceSync;
using ViewR.HelpersLib.Universals.Files;
using ViewR.Tools.CSVWriter.Mode;
using ViewR.Tools.CSVWriter.RequestedWriting;
using ViewR.Utils.Helpers;

namespace ViewR.Tools.CSVWriter
{
    public class CsvPoseDataWriter : MonoBehaviour
    {

        public HandBasedCalibrator HandBasedCalibrator;
        public ExperienceChooser ExperienceChooser;

        public void Start()

        {
            HandBasedCalibrator = GameObject.Find("CalibrationManager").GetComponentInChildren<HandBasedCalibrator>();
            
        }

        /// <summary>
        /// A class defining the configuration profile of <see cref="PoseDataWriter"/> to simplify passing on variables
        /// </summary>
        public class PoseDataWriterConfig
        {
            public FileTypeCheckConfig FileTypeCheckConfig = new FileTypeCheckConfig()
            {
                checkForFileType = true,
                fileEnding = ".csv"
            };
            
            public readonly IReadOnlyList<Transform> ObjectsToTrack;
            
            public readonly string FileNameInPersistentPath;
            public static readonly string ParentingFolder = "tracks";

            public PoseDataWriterConfig(string fileNameInPersistentPath, IReadOnlyList<Transform> objectsToTrack)
            {
                FileNameInPersistentPath = fileNameInPersistentPath;
                ObjectsToTrack = objectsToTrack;
            }

            public PoseDataWriterConfig(string fileNameInPersistentPath, IReadOnlyList<Transform> objectsToTrack, FileTypeCheckConfig fileTypeCheckConfig)
            {
                FileNameInPersistentPath = fileNameInPersistentPath;
                ObjectsToTrack = objectsToTrack;
                FileTypeCheckConfig = fileTypeCheckConfig;
            }
        }
        
        [Serializable]
        public class CsvWriterConfig
        {
            public bool WriteCount;
            public bool WriteGameObjectId;
            public bool WriteDeviceType = true;
            public bool WriteTrackingMode = false;
            public bool WriteTimeSinceStartup = true;
            public bool WritePosition = true;
            public bool WriteRotation = true;
            public bool WriteInputType;
            public bool WriteCalibrationState;
            public bool WriteCurrentCalibrationStation;
            public bool WriteExperienceID;
            public bool WritePerformanceMetrics;
        }

        [Serializable]
        public class PoseData
        {
            public string Id { get; set; }
            
            public Vector3 Position { get; set; }
            public Vector3 Forward { get; set; }
            public Quaternion Rotation { get; set; }
            
            public double TimeSinceStartup { get; set; }
        }

        [SerializeField] private CsvWriterConfig csvWriterConfig;

        private static string _deviceType;
        private int _count = 0;

        /// <summary>
        /// Actually Creates new entries and writes them to the file.
        /// </summary>
        public void GenerateNewEntry(PoseDataWriterConfig poseDataWriterConfig)
        {
            var allPoseData = new PoseData[poseDataWriterConfig.ObjectsToTrack.Count];

            for (var i = 0; i < poseDataWriterConfig.ObjectsToTrack.Count; i++)
            {
                var objectToTrack = poseDataWriterConfig.ObjectsToTrack[i];
                
                allPoseData[i] = new PoseData()
                {
                    Id = objectToTrack.gameObject.name,
                    // Forward = objectToTrack.forward,
                    Position = objectToTrack.position,
                    Rotation = objectToTrack.rotation,
                    // UnixEpoch = DateTime.Now.GetUnixEpoch()
                    TimeSinceStartup = Time.time
                };
            }
            
            AppendFile(poseDataWriterConfig, allPoseData);
        }
        
        /// <summary>
        /// Actively writing to the defined file.
        /// </summary>
        private void AppendFile(PoseDataWriterConfig poseDataWriterConfig, PoseData[] data)
        {
            var path = FileConfiguration.ConfigurePath(
                poseDataWriterConfig.FileNameInPersistentPath,
                poseDataWriterConfig.FileTypeCheckConfig,
                PoseDataWriterConfig.ParentingFolder);

            // Fetch device type if not yet set
            if (string.IsNullOrEmpty(_deviceType))
                _deviceType = OVRHelpers.FetchQuestModelID();

            // Cache value
            var fileExisted = File.Exists(path);

            // Append the file and Create if not existent yet.
            using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                // Write header if first time:
                if (!fileExisted)
                    writer.WriteLine(BuildCsvHeader);

                string trackingMode = null;
                if (csvWriterConfig.WriteTrackingMode)
                    trackingMode = ((int) TrackingModeManager.CurrentTrackingMode).ToString();
                
                // For each PoseData, write a line
                for (var i = 0; i < data.Length; i++)
                {
                    writer.WriteLine(BuildCsvRow(data[i], trackingMode));
                }
                
                // Up counter
                _count++;
            }

            RequestWritingEvents.InvokeDidWrite();
        }

        private string BuildCsvHeader
        {
            get
            {
                var output = "";
                if (csvWriterConfig.WriteCount)
                    output += "Count,";
                if (csvWriterConfig.WriteGameObjectId)
                    output += "ID,";
                if (csvWriterConfig.WriteDeviceType)
                    output += "Device,";
                if (csvWriterConfig.WriteTrackingMode)
                    output += "TrackingMode,";
                if (csvWriterConfig.WriteTimeSinceStartup)
                    output += "Time,";
                if (csvWriterConfig.WritePosition)
                    output += "PositionX,PositionY,PositionZ,";
                if (csvWriterConfig.WriteRotation)
                    output += "QuatW,QuatX,QuatY,QuatZ,";
                if (csvWriterConfig.WriteInputType)
                    output += "InputDevice,";
                if (csvWriterConfig.WriteCalibrationState)
                    output += "CalibrationState,UserInCalibrationZone,";
                if (csvWriterConfig.WriteCurrentCalibrationStation)
                    output += "CurrentCalibrationStation,";
                if (csvWriterConfig.WriteExperienceID)
                    output += "CurrentExperienceID";
                // if (csvWriterConfig.WritePerformanceMetrics)
                //     output += "CurrentCalibrationStation";
               

                return output;
            }
        }

        private string BuildCsvRow(PoseData data, string trackingMode = null)
        {
            // The default prints "(-9.5, 0.6, 7.5)", resulting in poor resolution
            // PseudoHead,17.676,0.70,0.56,3.06,0.91,0.00,-0.41

            var builder = new StringBuilder();
            if (csvWriterConfig.WriteCount)
            {
                builder.Append(_count);
                builder.Append(",");
            }
            if (csvWriterConfig.WriteGameObjectId)
            {
                builder.Append(data.Id);
                builder.Append(",");
            }   
            if (csvWriterConfig.WriteDeviceType)
            {
                builder.Append(_deviceType);
                builder.Append(",");
            }
            if (csvWriterConfig.WriteTrackingMode)
            {
                builder.Append(trackingMode);
                builder.Append(",");
            }
            if (csvWriterConfig.WriteTimeSinceStartup)
            {
                builder.Append(data.TimeSinceStartup.ToString(CsvWriter.TimeFormat, CsvWriter.CultInfo));
                builder.Append(",");
            }
            if (csvWriterConfig.WritePosition)
            {
                builder.Append(data.Position.x.ToString(CsvWriter.PositionFormat, CsvWriter.CultInfo));
                builder.Append(",");
                builder.Append(data.Position.y.ToString(CsvWriter.PositionFormat, CsvWriter.CultInfo));
                builder.Append(",");
                builder.Append(data.Position.z.ToString(CsvWriter.PositionFormat, CsvWriter.CultInfo));
                builder.Append(",");
            }
            if (csvWriterConfig.WriteRotation)
            {
                builder.Append(data.Rotation.w.ToString(CsvWriter.RotationFormat, CsvWriter.CultInfo));
                builder.Append(",");
                builder.Append(data.Rotation.x.ToString(CsvWriter.RotationFormat, CsvWriter.CultInfo));
                builder.Append(",");
                builder.Append(data.Rotation.y.ToString(CsvWriter.RotationFormat, CsvWriter.CultInfo));
                builder.Append(",");
                builder.Append(data.Rotation.z.ToString(CsvWriter.RotationFormat, CsvWriter.CultInfo));
            }
            if (csvWriterConfig.WriteInputType)
            {
                builder.Append(",");
                builder.Append((int)OVRInput.GetActiveController());
            }
            if (csvWriterConfig.WriteCalibrationState)
            {
                builder.Append(",");
                builder.Append(HandBasedCalibrator.UserInCalibrationMode ? 1 : 0);
                builder.Append(",");
                builder.Append(HandBasedCalibrator.UserStayInCalibrationZone ? 1 : 0);
            }
            if (csvWriterConfig.WriteCurrentCalibrationStation)
            {
                builder.Append(",");
                if(HandBasedCalibrator.currentCalibrationStation)
                    builder.Append(HandBasedCalibrator.currentCalibrationStation.name[19]);
                else
                {
                    builder.Append("0");
                }
            }
            if (csvWriterConfig.WriteExperienceID)
            {
                builder.Append(",");
                builder.Append(ExperienceChooser.CurrentExperienceID);
                
            }
            // if (csvWriterConfig.WriteCurrentCalibrationStation)
            // {
            //     builder.Append(",");
            //     builder.Append(HandBasedCalibrator.UserStayInCalibrationZone);
            // }

            return builder.ToString();
        }
    }
}