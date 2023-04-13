using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.Tools.CSVWriter.RequestedWriting;

namespace ViewR.Tools.CSVWriter.Accessors
{
    public class CsvWriterRequester : MonoBehaviour
    {
        [Header("Local use")]
        [Help("This will overwrite the use of the CsvWriterManager, if assigned. Leave empty otherwise.")]
        [SerializeField] 
        private CsvRecordPoseContinuously csvRecordPoseContinuously;
        [SerializeField] 
        private CsvRecordPoseOnRequest csvRecordPoseOnRequest;

        public void StartContinuousWriter()
        {
            if (csvRecordPoseContinuously != null)
                csvRecordPoseContinuously.StartWriting();
            else
            {
                if (!CsvWriterManager.IsInstanceRegistered)
                {
                    Debug.LogError("No CsvWriterManager registered!", this);
                    return;
                }
                
                CsvWriterManager.Instance.csvRecordPoseContinuously.StartWriting();
            }
        }
        
        public void StopContinuousWriter()
        {
            if (csvRecordPoseContinuously != null)
                csvRecordPoseContinuously.StopWriting();
            else
            {
                if (!CsvWriterManager.IsInstanceRegistered)
                {
                    Debug.LogError("No CsvWriterManager registered!", this);
                    return;
                }
                
                CsvWriterManager.Instance.csvRecordPoseContinuously.StopWriting();
            }
        }

        public void RequestToWriteOnce()
        {
            {
                if (csvRecordPoseOnRequest != null)
                    csvRecordPoseOnRequest.WriteOnce();
                else
                {
                    if (!CsvWriterManager.IsInstanceRegistered)
                    {
                        Debug.LogError("No CsvWriterManager registered!", this);
                        return;
                    }
                
                    CsvWriterManager.Instance.csvRecordPoseOnRequest.WriteOnce();
                }
            }
        }
    }
}