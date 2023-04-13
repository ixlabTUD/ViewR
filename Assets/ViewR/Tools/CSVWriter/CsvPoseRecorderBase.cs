using UnityEngine;
using UnityEngine.Serialization;

namespace ViewR.Tools.CSVWriter
{
    [RequireComponent(typeof(CsvPoseDataWriter))]
    public class CsvPoseRecorderBase : MonoBehaviour
    {
        [FormerlySerializedAs("filePathInPersistentPath")]
        [Header("Setup")]
        [SerializeField, Tooltip("Note: This will be stored in PersistentPath/tracks/<name>")]
        internal string fileNameInPersistentPath;
        [SerializeField]
        internal Transform[] objectsToTrack;

        internal CsvPoseDataWriter CsvPoseDataWriter;
        internal CsvPoseDataWriter.PoseDataWriterConfig PoseDataWriterConfig;
        
        private bool _initialized = false;


        protected virtual void Start()
        {
            Initialize();
        }

        #region Private Methods

        /// <summary>
        /// Initializes the class by configuring the writer config and the path.
        /// </summary>
        /// <param name="forceOverwrite"></param>
        internal void Initialize(bool forceOverwrite = false)
        {
            // Bail if already done and not overwriting
            if (_initialized && ! forceOverwrite)
                return;

            if (!CsvPoseDataWriter)
                CsvPoseDataWriter = GetComponent<CsvPoseDataWriter>();
            
            ConfigureWriterConfig();
            FileConfiguration.ConfigurePath(
                PoseDataWriterConfig.FileNameInPersistentPath,
                PoseDataWriterConfig.FileTypeCheckConfig,
                CsvPoseDataWriter.PoseDataWriterConfig.ParentingFolder,
                true);

            _initialized = true;
        }
        
        private void ConfigureWriterConfig()
        {
            fileNameInPersistentPath = FileConfiguration.ExtendFileNameByDeviceDateSessionID(fileNameInPersistentPath);

            // Init 
            PoseDataWriterConfig = new CsvPoseDataWriter.PoseDataWriterConfig(
                objectsToTrack: objectsToTrack,
                fileNameInPersistentPath: fileNameInPersistentPath);
        }

        #endregion

        /// <summary>
        /// Overwrites the <see cref="objectsToTrack"/> and Re-Initializes this class.
        /// We will thus write to a new file.
        /// </summary>
        public void SetObjectsToTrack(Transform[] newObjectsToTrack)
        {
            objectsToTrack = newObjectsToTrack;
            
            Initialize(true);
        }

        public void ConfigureNewSetup(string newFileName, Transform[] newObjectsToTrack = null)
        {
            fileNameInPersistentPath = newFileName;
            if (newObjectsToTrack != null)
                objectsToTrack = newObjectsToTrack;

            Initialize(true);
        }
    }
}