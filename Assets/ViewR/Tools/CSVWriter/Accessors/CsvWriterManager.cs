using Pixelplacement;
using ViewR.Tools.CSVWriter.RequestedWriting;

namespace ViewR.Tools.CSVWriter.Accessors
{
    public class CsvWriterManager : SingletonExtended<CsvWriterManager>
    {
        public CsvRecordPoseContinuously csvRecordPoseContinuously;
        public CsvRecordPoseOnRequest csvRecordPoseOnRequest;
    }
}
