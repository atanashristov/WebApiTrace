using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using WebApiTrace.TraceSettings;

namespace WebApiTrace.TraceData
{
    /// <summary>
    /// The root element of the trace data. Contains all the trace data for one request.
    /// </summary>
    [Serializable]
    public class TraceData
    {

        /// <summary>
        /// HTTP Request method name.
        /// Example values: "GET", "POST", ...
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// HTTP Request URL (absolute URI).
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The UTC start time the request.
        /// </summary>
        public DateTime StartTime { get; set; }


        [XmlIgnore]
        public bool LocalStartTimeSpecified { get { return TraceSwitcher.Verbosity == TraceSwitcher.TraceVerbosity.Verbose; } }

        /// <summary>
        /// The local start time the request.
        /// </summary>
        public DateTime LocalStartTime { get { return StartTime.ToLocalTime(); } set { } }


        [XmlIgnore]
        public bool EndTimeSpecified { get { return EndTime.HasValue && TraceSwitcher.Verbosity == TraceSwitcher.TraceVerbosity.Verbose; } }

        /// <summary>
        /// The end time the request.
        /// </summary>
        public DateTime? EndTime { get; set; }


        [XmlIgnore]
        public bool LocalEndTimeSpecified { get { return EndTime.HasValue && TraceSwitcher.Verbosity == TraceSwitcher.TraceVerbosity.Verbose; } }

        /// <summary>
        /// The end time the request.
        /// </summary>
        public DateTime? LocalEndTime { get { return EndTime.GetValueOrDefault().ToLocalTime(); } set { } }


        /// <summary>
        /// Total time in milliseconds.
        /// </summary>
        public double TotalTime { get; set; }


        /// <summary>
        /// The name of the controller that was executed.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// The name of the controller's action that was executed.
        /// </summary>
        public string ActionName { get; set; }


        /// <summary>
        /// Exception information.
        /// </summary>
        public TraceException Exception { get; set; }


        [XmlIgnore]
        public bool EventsSpecified { get { return Events != null && Events.Count > 0; } }

        /// <summary>
        /// List of the events during the request.
        /// </summary>
        public List<TraceEvent> Events { get; set; }


        public TraceData()
        {
            Events = new List<TraceEvent>();
        }

    }
}
