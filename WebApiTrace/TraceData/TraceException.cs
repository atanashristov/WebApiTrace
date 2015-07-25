using System.Xml.Serialization;

namespace WebApiTrace.TraceData
{
    [XmlRoot("Exception")]
    public class TraceException
    {
        /// <summary>
        /// Contains the exception message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Contains the exception source name.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Contains the exception stack trace.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Contains inner exception if one exists.
        /// </summary>
        public TraceException InnerException { get; set; }
    }
}
