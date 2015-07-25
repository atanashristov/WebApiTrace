using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using WebApiTrace.TraceWriters;

namespace WebApiTraceExample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();


            // Route configurations

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                         name: "DefaultApi",
                         routeTemplate: "api/{controller}/{id}",
                         defaults: new { id = RouteParameter.Optional }
                     );


            // camelCase output formatter

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();



            // Trace set-up

            //config.EnableSystemDiagnosticsTracing();
            var traceWriter = new StringToDebuggerTraceWriter(new XmlToDebuggerTraceWriter());
            config.Services.Replace(typeof(System.Web.Http.Tracing.ITraceWriter), traceWriter);

            app.UseWebApi(config);



            // OWIN settings

            app.Run(context =>
            {
                if (context.Request.Path.Value == "/fail")
                {
                    throw new Exception("Request failed");
                }

                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello, world.");
            });
        }
    }
}
