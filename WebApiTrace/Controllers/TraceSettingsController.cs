using WebApiTrace.TraceSettings;
using System.Web.Http;

namespace WebApiTrace.Controllers
{
    [RoutePrefix("api/system/trace")]
    public class TraceSettingsController : ApiController
    {
        /// <summary>
        /// Get current trace settings
        /// </summary>
        /// 
        /// <example>
        /// 
        /// Request:
        /// <code>
        /// <![CDATA[
        ///     GET http://localhost:9000/api/system/trace/settings HTTP/1.1
        ///     User-Agent: Fiddler
        ///     Host: localhost:9000
        /// ]]>
        /// </code>
        /// 
        /// Response:
        /// <code>
        /// <![CDATA[
        ///    HTTP/1.1 200 OK
        ///    Content-Length: 39
        ///    Content-Type: application/json; charset=utf-8
        ///    Server: Microsoft-HTTPAPI/2.0
        ///    Date: Sun, 03 May 2015 18:27:28 GMT
        ///    
        ///    {"level":"error","verbosity":"general"}
        /// ]]>
        /// </code>
        /// 
        /// </example>
        /// 
        /// <returns>Trace diagnostic settings.</returns>
        /// 
        [Route("settings")]
        [HttpGet]
        public GetTraceSettingsResult GetTraceSettings()
        {
            return new GetTraceSettingsResult
            {
                Level = TraceSwitcher.Level.ToString().ToLower(),
                Verbosity = TraceSwitcher.Verbosity.ToString().ToLower(),
            };
        }


        /// <summary>
        /// Change trace settings.
        /// </summary>
        /// 
        /// <example>
        /// 
        /// Request:
        /// <code>
        /// <![CDATA[
        ///    PUT http://localhost:9000/api/system/trace/settings HTTP/1.1
        ///    User-Agent: Fiddler
        ///    Host: localhost:9000
        ///    Content-Type: application/json
        ///    Content-Length: 38
        ///
        ///    {"level":"none","verbosity":"minimal"}
        /// ]]>
        /// </code>
        ///
        /// Response:
        /// <code>
        /// <![CDATA[
        ///    HTTP/1.1 204 No Content
        ///    Content-Length: 0
        ///    Server: Microsoft-HTTPAPI/2.0
        ///    Date: Sun, 03 May 2015 18:39:36 GMT
        /// ]]>
        /// </code>
        ///  
        /// </example>
        /// 
        /// <param name="request">new settings</param>
        /// 
        [Route("settings")]
        [HttpPut]
        public void SetTraceSettings(SetTraceSettingsRequest request)
        {
            TraceSwitcher.Level = request.Level;
            TraceSwitcher.Verbosity = request.Verbosity;
        }


        /// <summary>
        /// Reset trace settings to default
        /// </summary>
        /// <example>
        /// 
        /// Request:
        /// <code>
        /// <![CDATA[
        ///     PUT http://localhost:9000/api/system/trace/settings/reset HTTP/1.1
        ///     User-Agent: Fiddler
        ///     Host: localhost:9000
        ///     Content-Type: application/json
        ///     Content-Length: 0
        /// ]]>
        /// </code>
        /// 
        /// Response:
        /// <code>
        /// <![CDATA[
        ///     HTTP/1.1 204 No Content
        ///     Content-Length: 0
        ///     Server: Microsoft-HTTPAPI/2.0
        ///     Date: Sun, 03 May 2015 18:53:08 GMT
        /// ]]>
        /// </code>
        /// 
        /// </example>
        /// 
        [Route("settings/reset")]
        [HttpPut]
        public void ResetTraceSettings()
        {
            TraceSwitcher.ResetSettings();
        }


        /// <summary>
        /// Turn off trace completely
        /// </summary>
        /// <example>
        /// 
        /// Request:
        /// <code>
        /// <![CDATA[
        ///     PUT http://localhost:9000/api/system/trace/settings/off HTTP/1.1
        ///     User-Agent: Fiddler
        ///     Host: localhost:9000
        ///     Content-Type: application/json
        ///     Content-Length: 0
        /// ]]>
        /// </code>
        /// 
        /// Response:
        /// <code>
        /// <![CDATA[
        ///     HTTP/1.1 204 No Content
        ///     Content-Length: 0
        ///     Server: Microsoft-HTTPAPI/2.0
        ///     Date: Sun, 03 May 2015 18:58:38 GMT
        /// ]]>
        /// </code>
        /// 
        /// </example>
        /// 
        [Route("settings/off")]
        [HttpPut]
        public void TurnOffTraceSettings()
        {
            TraceSwitcher.TurnOff();
        }

        /// <summary>
        /// DTO to get the trace diagnostics settings
        /// </summary>
        public struct GetTraceSettingsResult
        {
            public string Level { get; set; }
            public string Verbosity { get; set; }
        }

        /// <summary>
        /// DTO to change the trace diagnostics settings
        /// </summary>
        public struct SetTraceSettingsRequest
        {
            public TraceSwitcher.TraceLevel Level { get; set; }
            public TraceSwitcher.TraceVerbosity Verbosity { get; set; }
        }

    }
}
