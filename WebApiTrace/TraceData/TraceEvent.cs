using System;
using System.Xml.Serialization;
using WebApiTrace.TraceSettings;

namespace WebApiTrace.TraceData
{
    /// <summary>
    /// Contains track of one trace call.
    /// </summary>
    [XmlRoot("Event")]
    public class TraceEvent
    {
        /// <summary>
        /// Example values: "System.Web.Http.Request", "System.Web.Http.Controllers", ...
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Trace message. For example fully qualified controller class name, absolute uri, etc.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Example values: "", "SelectController", ...
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// Example values: "", "DefaultHttpControllerSelector", ...
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Contains string representation of System.Web.Http.Tracing.TraceKind. 
        /// Example values: "Begin", "End", ... 
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// Contains string representation of System.Web.Http.Tracing.TraceLevel.
        /// Example values: "Info".
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// The UTC date/time of the event
        /// </summary>
        public DateTime EventTime { get; set; }


        [XmlIgnore]
        public bool LocalEventTimeSpecified { get { return TraceSwitcher.Verbosity == TraceSwitcher.TraceVerbosity.Verbose; } }

        /// <summary>
        /// The local date/time of the event
        /// </summary>
        public DateTime LocalEventTime { get { return EventTime.ToLocalTime(); } set { } }

    }
}
