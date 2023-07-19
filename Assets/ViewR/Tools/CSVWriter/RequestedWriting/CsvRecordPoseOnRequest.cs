using UnityEngine;
using UnityEngine.Events;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Tools.CSVWriter.RequestedWriting
{
    /// <summary>
    /// Writes PoseData from <see cref="CsvPoseRecorderBase.objectsToTrack"/> upon request.
    /// </summary>
    public class CsvRecordPoseOnRequest : CsvPoseRecorderBase
    {
        [Header("Unity Events")]
        [SerializeField]
        private UnityEvent capturedMoment;
        
        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        #region Unity Methods

        private void OnEnable()
        {
            // Subscribe
            RequestWritingEvents.DoWrite += DoWriteRequestThroughEvent;
        }

        private void OnDisable()
        {
            // Unsubscribe
            RequestWritingEvents.DoWrite -= DoWriteRequestThroughEvent;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Writes upon request through event <see cref="RequestWritingEvents.DoWrite"/>.
        /// </summary>
        private void DoWriteRequestThroughEvent()
        {
            if (debugging)
                Debug.Log("DoWriteRequestThroughEvent.".Green());

            WriteOnce();
        }

        #endregion

        
        #region Public Methods

        [ExposeMethodInEditor]
        public void WriteOnce()
        {
            if (debugging)
                Debug.Log("Logging.".Bold().Blue());

            // Do Write
            CsvPoseDataWriter.GenerateNewEntry(PoseDataWriterConfig);
            
            // Invoke
            capturedMoment?.Invoke();
        }

        #endregion
    }
}
