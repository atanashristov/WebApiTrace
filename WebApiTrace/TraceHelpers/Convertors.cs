using System;
using System.IO;
using System.Text;
using System.Web.Http.Tracing;
using System.Xml;
using System.Xml.Serialization;
using WebApiTrace.TraceData;

namespace WebApiTrace.TraceHelpers
{
    public static class Convertors
    {
        public static string ToXml<T>(T obj)
        {
            var sb = new StringBuilder();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
            };

            using (TextWriter strWriter = new StringWriter(sb))
            using (XmlWriter xmlWriter = XmlWriter.Create(strWriter, xmlWriterSettings))
            {
                var serializer = new XmlSerializer(typeof(T));

                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                serializer.Serialize(xmlWriter, obj, ns);
            }
            return sb.ToString();
        }

        public static TraceData.TraceData ConvertToTraceData(TraceRecord rec, TimeMachine time)
        {
            var res = new TraceData.TraceData();

            res.Method = rec.Request.Method.Method;
            res.Url = rec.Request.RequestUri.AbsoluteUri;
            res.StartTime = time.UtcNow;

            return res;
        }

        public static TraceEvent ConvertToTraceEvent(TraceRecord rec, TimeMachine time)
        {
            var res = new TraceEvent();

            res.Category = rec.Category;
            res.Message = rec.Message;
            res.Operation = rec.Operation;
            res.Operator = rec.Operator;
            res.Kind = rec.Kind.ToString();
            res.Level = rec.Level.ToString();
            res.EventTime = time.UtcNow;

            return res;
        }

        public static TraceException ConvertToTraceException(TraceRecord rec, TimeMachine time)
        {
            return (rec.Exception == null) ? null : GetExceptionInformation(rec.Exception);
        }

        private static TraceException GetExceptionInformation(Exception ex)
        {
            var res = new TraceException();

            res.Message = ex.Message;
            res.Source = ex.Source;
            res.StackTrace = ex.StackTrace;

            if (ex.InnerException != null)
                res.InnerException = GetExceptionInformation(ex.InnerException);

            return res;
        }
    }
}
