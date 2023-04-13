using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using ViewR.Core.UI.FloatingUI.IntroductionSequencing.FineTunedAlignment;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Tools.CSVWriter
{
    public class CsvRecordPoseContinuously : CsvPoseRecorderBase
    {
        [Header("Setup")]
        [SerializeField]
        private float recordWaitTime = 0.05f;
        [SerializeField]
        private bool everyFrame;
        [SerializeField]
        private bool startUponInitialAlignment;
        
        [Header("Unity Events")]
        [SerializeField]
        private UnityEvent startedTracking;
        [SerializeField]
        private UnityEvent stoppedTracking;
        
        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        private Coroutine _repeatedInvoke;

        protected override void Start()
        {
            base.Start();

            if (startUponInitialAlignment)
                AlignmentEvents.AlignmentCompleted += StartWriting;
        }

        #region Public Methods

        [ContextMenu(nameof(StartWriting))]
        public void StartWriting()
        {
            if (debugging)
                Debug.Log("Starting Logging.".Bold().Green());
            
            // Stop if already running
            if (_repeatedInvoke != null)
                StopWriting();

            // Start
            _repeatedInvoke = StartCoroutine(InvokeWriteRepetitively());
            
            // Invoke
            startedTracking?.Invoke();

            if (startUponInitialAlignment)
                AlignmentEvents.AlignmentCompleted -= StartWriting;
        }

        [ContextMenu(nameof(StopWriting))]
        public void StopWriting()
        {
            if (debugging)
                Debug.Log("Stopping Logging.".Bold().Green());

            // Stop
            StopCoroutine(_repeatedInvoke);
            
            // Invoke
            stoppedTracking?.Invoke();
        }

        #endregion

        /// <summary>
        /// Writes the NewEntry every <see cref="recordWaitTime"/> seconds.
        /// Spins infinitely until canceled.
        /// </summary>
        private IEnumerator InvokeWriteRepetitively()
        {
            // Spin
            while (true)
            {
                Initialize();
                
                // Log
                if (debugging)
                    Debug.Log("Logging...".Teal());
                
                // Write
                CsvPoseDataWriter.GenerateNewEntry(PoseDataWriterConfig);
                
                // Wait
                if (everyFrame)
                    yield return new WaitForEndOfFrame();
                else
                    yield return new WaitForSeconds(recordWaitTime);
            }
        }
    }
}
