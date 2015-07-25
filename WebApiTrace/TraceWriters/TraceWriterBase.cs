using WebApiTrace.TraceHelpers;
using WebApiTrace.TraceSettings;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Tracing;

namespace WebApiTrace.TraceWriters
{
    /// <summary>
    /// Base class the trace writers inherit from.
    /// </summary>
    public abstract class TraceWriterBase : ITraceWriter
    {
        private const string PropKey = "WebApiTraceData";

        private readonly ITraceWriter _next;
        private readonly TimeMachine _time;

        protected ITraceWriter Next { get { return _next; } }

        protected TraceWriterBase(ITraceWriter next, TimeMachine time)
        {
            _next = next;
            _time = time ?? new TimeMachine();
        }

        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (TraceSwitcher.Level == TraceSwitcher.TraceLevel.None)
                return;

            if (request == null)
                return;

            var rec = new TraceRecord(request, category, level);
            traceAction(rec);
            AddTraceData(request, rec);

            if (!IsRequestFinishing(rec))
                return;

            var traceData = GetTraceData(request);
            if (traceData != null)
            {
                ComputeSummaryData(traceData);

                if (TraceSwitcher.Level == TraceSwitcher.TraceLevel.Error
                    && traceData.Exception == null)
                {
                    return;
                }

                OnRequestFinish(traceData);
            }
        }

        public void OnRequestFinish(TraceData.TraceData traceData)
        {
            OnRequestFinishImpl(traceData);
            if (Next != null)
            {
                Next.OnRequestFinish(traceData);
            }
        }

        protected abstract void OnRequestFinishImpl(TraceData.TraceData traceData);


        private static bool IsRequestFinishing(TraceRecord rec)
        {
            if (!AreEqual(rec.Operation, "Dispose"))
                return false;

            if (!AreEqual(rec.Category, "System.Web.Http.Controllers"))
                return false;

            if (rec.Kind != TraceKind.End)
                return false;

            return true;
        }

        private static bool AreEqual(string propValue, string value)
        {
            return (String.Equals((propValue ?? string.Empty).Trim(), (value ?? string.Empty).Trim(), StringComparison.CurrentCultureIgnoreCase));
        }


        private TraceData.TraceData GetTraceData(HttpRequestMessage request)
        {
            object obj;
            if (!request.Properties.TryGetValue(PropKey, out obj))
                return null;

            return obj as TraceData.TraceData;
        }

        private void AddTraceData(HttpRequestMessage request, TraceRecord rec)
        {
            AddTraceMainData(request, rec);

            AddTraceEventData(request, rec);

            AddTraceExceptionData(request, rec);
        }

        private void AddTraceMainData(HttpRequestMessage request, TraceRecord rec)
        {
            if (request.Properties.ContainsKey(PropKey))
                return;

            request.Properties[PropKey] = Convertors.ConvertToTraceData(rec, _time);
        }

        private void AddTraceEventData(HttpRequestMessage request, TraceRecord rec)
        {
            var traceData = GetTraceData(request);
            if (traceData == null)
                return;

            if ((TraceSwitcher.Verbosity == TraceSwitcher.TraceVerbosity.Verbose) ||
                (TraceSwitcher.Verbosity == TraceSwitcher.TraceVerbosity.General && rec.Kind == TraceKind.Begin))
                traceData.Events.Add(Convertors.ConvertToTraceEvent(rec, _time));

            request.Properties[PropKey] = traceData;
        }

        private void AddTraceExceptionData(HttpRequestMessage request, TraceRecord rec)
        {
            if (rec.Exception == null)
                return;

            var traceData = GetTraceData(request);
            if (traceData == null)
                return;

            if (traceData.Exception != null)
                return;

            traceData.Exception = Convertors.ConvertToTraceException(rec, _time);
            request.Properties[PropKey] = traceData;
        }

        private void ComputeSummaryData(TraceData.TraceData traceData)
        {
            ComputeTotalTime(traceData);
            ComputeControllerName(traceData);
            ComputeActionName(traceData);
        }

        private void ComputeTotalTime(TraceData.TraceData traceData)
        {
            traceData.EndTime = _time.UtcNow;

            TimeSpan ts = traceData.EndTime.GetValueOrDefault() - traceData.StartTime;
            traceData.TotalTime = ts.TotalMilliseconds;
        }

        private void ComputeControllerName(TraceData.TraceData traceData)
        {
            var evt = traceData.Events.FirstOrDefault(e =>
                AreEqual(e.Category, "System.Web.Http.Controllers") &&
                AreEqual(e.Operation, "Create") &&
                AreEqual(e.Kind, "End"));

            if (evt == null)
                return;

            traceData.ControllerName = evt.Message;
        }

        private void ComputeActionName(TraceData.TraceData traceData)
        {
            var evt = traceData.Events.FirstOrDefault(e =>
                AreEqual(e.Category, "System.Web.Http.Action") &&
                AreEqual(e.Operation, "ExecuteAsync") &&
                AreEqual(e.Kind, "Begin"));

            if (evt == null)
                return;

            traceData.ActionName = evt.Message;
        }


    }
}
