using System;
using WebApiTrace.TraceHelpers;
using WebApiTrace.TraceSettings;

namespace WebApiTrace.TraceWriters
{
    /// <summary>
    /// Writes xml with trace data to the debugger.
    /// </summary>
    public class StringToDebuggerTraceWriter : TraceWriterBase
    {
        public StringToDebuggerTraceWriter(ITraceWriter next = null, TimeMachine time = null)
            : base(next, time)
        { }


        protected override void OnRequestFinishImpl(TraceData.TraceData traceData)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                string msg;

                if (TraceSwitcher.Verbosity == TraceSwitcher.TraceVerbosity.Verbose)
                    msg = String.Format("[{0}/{1}, {2}ms] {3} {4}",
                        traceData.StartTime.ToString("o"),
                        traceData.LocalStartTime.ToString("o"),
                        traceData.TotalTime,
                        traceData.Method,
                        traceData.Url);
                else
                    msg = String.Format("[{0}, {1}ms] {2} {3}",
                        traceData.StartTime.ToString("o"),
                        traceData.TotalTime,
                        traceData.Method,
                        traceData.Url);

                System.Diagnostics.Debug.WriteLine(msg);
            }
        }
    }
}
