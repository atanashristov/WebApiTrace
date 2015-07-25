using WebApiTrace.TraceHelpers;

namespace WebApiTrace.TraceWriters
{
    /// <summary>
    /// Writes xml with trace data to the debugger.
    /// </summary>
    public class XmlToDebuggerTraceWriter : TraceWriterBase
    {
        public XmlToDebuggerTraceWriter(ITraceWriter next = null, TimeMachine time = null)
            : base(next, time)
        { }

        protected override void OnRequestFinishImpl(TraceData.TraceData traceData)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                string msg = Convertors.ToXml(traceData);
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }
    }
}
