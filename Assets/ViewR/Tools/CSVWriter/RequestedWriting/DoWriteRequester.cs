using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;

namespace ViewR.Tools.CSVWriter.RequestedWriting
{
    /// <summary>
    /// A simple accessor to trigger <see cref="CsvRecordPoseOnRequest"/> to write data to disk. 
    /// </summary>
    public class DoWriteRequester : MonoBehaviour
    {
        [ExposeMethodInEditor]
        public void RequestDoWrite()
        {
            RequestWritingEvents.InvokeDoWrite();
        }
    }
}