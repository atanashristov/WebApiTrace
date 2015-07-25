namespace WebApiTrace.TraceWriters
{
    /// <summary>
    /// Implement this interface to build your own trace writer.
    /// </summary>
    public interface ITraceWriter : System.Web.Http.Tracing.ITraceWriter
    {
        void OnRequestFinish(TraceData.TraceData traceData);
    }
}
