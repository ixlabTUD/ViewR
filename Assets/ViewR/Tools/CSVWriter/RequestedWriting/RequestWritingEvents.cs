namespace ViewR.Tools.CSVWriter.RequestedWriting
{
    /// <summary>
    /// A class holding events to simplify our lives.
    /// </summary>
    public static class RequestWritingEvents
    {
        // Events
        public delegate void CsvWritingEvent();

        public static CsvWritingEvent DoWrite;

        public static CsvWritingEvent DidWrite;

        public static void InvokeDoWrite() => DoWrite?.Invoke();
        public static void InvokeDidWrite() => DidWrite?.Invoke();

    }
}