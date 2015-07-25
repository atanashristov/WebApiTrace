**Author**: Atanas Hristov  
**Last Update**:  7/25/2015 2:45pm  


# Web API Trace

Is a tracing component for the WEb API, build on top of `System.Web.Http.Tracing` and `Microsoft.AspNet.WebApi.Tracing`.


## Configuration

To attach the trace writer to your Web API web application, edit the `WebApiConfig` class modify method `Register()`.

You can easily create a chain of trace writers, as every trace writer can be injected
with instance of another one through the constructor.

This is how to chain the provided with the component built-in trace writers:


    ...
    public static void Register(HttpConfiguration config)
    {
        ...
        var traceWriter = new WebApiTrace.TraceWriters.StringToDebuggerTraceWriter(new WebApiTrace.TraceWriters.XmlToDebuggerTraceWriter());
        config.Services.Replace(typeof(System.Web.Http.Tracing.ITraceWriter), traceWriter);
    }




## Trace writer implementation



### Built-in trace writers

The component comes with two built-in trace writers:

- StringToDebuggerTraceWriter

Writes a line in the debugger window with simple trace like:

    [6/7/2015 1:39:27 PM, 1732.3821ms] GET http://localhost:9000/api/exception


- XmlToDebuggerTraceWriter

Writes XML in the debugger window that contains trace information. 

With default trace settings, this call to the `TraceExample` solution ExceptoinController:

    GET http://localhost:9000/api/exception HTTP/1.1
    Content-Type: application/json
    Host: localhost:9000
    Content-Length: 0

prints to the debugger:

```xml
  <TraceData>
    <Method>GET</Method>
    <Url>http://localhost:9000/api/exception</Url>
    <StartTime>2015-06-07T13:39:27.5068443-05:00</StartTime>
    <TotalTime>1732.3821</TotalTime>
    <Exception>
      <Message>Processing of the HTTP request resulted in an exception. Please see the HTTP response returned by the 'Response' property of this exception for details.</Message>
      <Source>WebApiTraceExample</Source>
      <StackTrace>   at WebApiTraceExample.Controllers.ExceptionController.Get() in F:\WebApiTrace\WebApiTraceExample\Controllers\ExceptionController.cs:line 17
     at lambda_method(Closure , Object , Object[] )
     at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.&lt;&gt;c__DisplayClassc.&lt;GetExecutor&gt;b__6(Object instance, Object[] methodParameters)
     at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.Execute(Object instance, Object[] arguments)
     at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ExecuteAsync(HttpControllerContext controllerContext, IDictionary`2 arguments, CancellationToken cancellationToken)
  --- End of stack trace from previous location where exception was thrown ---
     at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
     at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
     at System.Web.Http.Tracing.ITraceWriterExtensions.&lt;TraceBeginEndAsyncCore&gt;d__18`1.MoveNext()</StackTrace>
    </Exception>
  </TraceData>
```


### Own trace writers

To implement your own trace writer, inherit from:

    WebApiTrace.TraceWriters.TraceWriterBase


See the provided built-in trace writers as an example of implementing a trace writer.






## Trace Settings

### Level

The `level` controls for which AP calls to collect information and run trace.

We have the following levels:

    none: tracing is turned off
    error: (default) tracing only for errors and exceptions
    debug: tracing is for each and every request


### Verbosity

The `verbosity` controls how much data to show in the trace output.

We have the following verbosity options:

    minimal: show only summary for the AP call is shown
    general: show information for begin events of the WebAPI pipeline
    verbose: show information for all the events of the WebAPI pipeline




## Trace Settings API


Use the following URLs to set-up and check the WebAPI tracing:



- Get current trace settings

To get the current trace settings use the API Command:

    GET /api/system/trace/settings

Example:

    GET http://localhost:9000/api/system/trace/settings HTTP/1.1

    HTTP/1.1 200 OK
    Content-Length: 39
    Content-Type: application/json; charset=utf-8
    Server: Microsoft-HTTPAPI/2.0
    Date: Sun, 07 Jun 2015 18:26:04 GMT

    {"level":"debug","verbosity":"minimal"}




- Change the trace settings

To change the trace settings use the API command:

    PUT /api/system/trace/settings

Example:

    PUT http://localhost:9000/api/system/trace/settings HTTP/1.1
    Content-Type: application/json
    Host: localhost:9000
    Content-Length: 39

    {"level":"debug","verbosity":"minimal"}

    HTTP/1.1 204 No Content
    Content-Length: 0
    Server: Microsoft-HTTPAPI/2.0
    Date: Sun, 07 Jun 2015 18:27:40 GMT



- Reset trace settings to default

To reset the trace settings to default use the API command:

    PUT /api/system/trace/settings/reset

Example:

    PUT http://localhost:9000/api/system/trace/settings/reset HTTP/1.1
    Content-Type: application/json
    Host: localhost:9000
    Content-Length: 0

    HTTP/1.1 204 No Content
    Content-Length: 0
    Server: Microsoft-HTTPAPI/2.0
    Date: Sun, 07 Jun 2015 18:31:45 GMT

    ...

    GET http://localhost:9000/api/system/trace/settings HTTP/1.1
    Content-Type: application/json
    Host: localhost:9000
    Content-Length: 0

    HTTP/1.1 200 OK
    Content-Length: 39
    Content-Type: application/json; charset=utf-8
    Server: Microsoft-HTTPAPI/2.0
    Date: Sun, 07 Jun 2015 18:33:51 GMT

    {"level":"error","verbosity":"minimal"}




- Turn off trace completely

To completely turn off the trace, run the command:

    PUT /api/system/trace/settings/off

Example:

    PUT http://localhost:9000/api/system/trace/settings/off HTTP/1.1
    Content-Type: application/json
    Host: localhost:9000
    Content-Length: 0

    HTTP/1.1 204 No Content
    Content-Length: 0
    Server: Microsoft-HTTPAPI/2.0
    Date: Sun, 07 Jun 2015 18:33:57 GMT

    ...


    GET http://localhost:9000/api/system/trace/settings HTTP/1.1
    Content-Type: application/json
    Host: localhost:9000
    Content-Length: 0

    HTTP/1.1 200 OK
    Content-Length: 38
    Content-Type: application/json; charset=utf-8
    Server: Microsoft-HTTPAPI/2.0
    Date: Sun, 07 Jun 2015 18:34:22 GMT

    {"level":"none","verbosity":"minimal"}






## Trace Level Examples


### Trace level: none

When trace `level` is set to "none", no tracing will be produced from the API calls.

The `verbosity` setting is ignored.

All these settings have the same effect:

    PUT http://localhost:9000/api/diagnostics/trace/settings HTTP/1.1
    Content-Type: application/json

    {"level":"none","verbosity":"minimal"}


    PUT http://localhost:9000/api/diagnostics/trace/settings HTTP/1.1
    Content-Type: application/json

    {"level":"none","verbosity":"general"}


    PUT http://localhost:9000/api/diagnostics/trace/settings HTTP/1.1
    Content-Type: application/json

    {"level":"none","verbosity":"verbose"}


Even if the AP throws an exception:

    GET http://localhost:9000/api/exception HTTP/1.1

, no trace will be produced.



### Trace level: error (default)


When trace `level` is set to "error", only error from the AP calls will be written to the trace.

This is the default trace `level` (together with `verbosity` set to "minimal").

The `verbosity` setting is respected.


Following are examples with different verbosity.


- trace level "error", verbosity set to "minimal" (default)

The trace will only show exceptions/errors. The events on the request pipeline will not be shown:


    PUT http://localhost:9000/api/diagnostics/trace/settings HTTP/1.1
    Content-Type: application/json

    {"level":"error","verbosity":"minimal"}

  
    GET http://localhost:9000/api/exception HTTP/1.1
    ...

```xml
  <TraceData>
    <Method>GET</Method>
    <Url>http://localhost:9000/api/exception</Url>
    <StartTime>2015-06-07T12:00:05.2883387-05:00</StartTime>
    <TotalTime>2234.9178</TotalTime>
    <Exception>
      <Message>Processing of the HTTP request resulted in an exception. Please see the HTTP response returned by the 'Response' property of this exception for details.</Message>
      <Source>WebApiTraceExample</Source>
      <StackTrace>   at WebApiTraceExample.Controllers.ExceptionController.Get() in F:\WebApiTrace\WebApiTraceExample\Controllers\ExceptionController.cs:line 17
     at lambda_method(Closure , Object , Object[] )
     at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.&lt;&gt;c__DisplayClassc.&lt;GetExecutor&gt;b__6(Object instance, Object[] methodParameters)
     at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.Execute(Object instance, Object[] arguments)
     at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ExecuteAsync(HttpControllerContext controllerContext, IDictionary`2 arguments, CancellationToken cancellationToken)
  --- End of stack trace from previous location where exception was thrown ---
     at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
     at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
     at System.Web.Http.Tracing.ITraceWriterExtensions.&lt;TraceBeginEndAsyncCore&gt;d__18`1.MoveNext()</StackTrace>
    </Exception>
  </TraceData>
```

  

- trace level "error", verbosity set to "general"

The trace will only show exceptions/errors. Only begin events on the request pipeline will be shown:

    PUT http://localhost:9000/api/diagnostics/trace/settings HTTP/1.1
    Content-Type: application/json

    {"level":"error","verbosity":"general"}


    GET http://localhost:9000/api/exception HTTP/1.1
    ...

```xml
  <TraceData>
    <Method>GET</Method>
    <Url>http://localhost:9000/api/exception</Url>
    <StartTime>2015-06-07T12:11:10.4735405-05:00</StartTime>
    <TotalTime>1998.6536</TotalTime>
    <ActionName>Invoking action 'Get()'</ActionName>
    <Exception>
      <Message>Processing of the HTTP request resulted in an exception. Please see the HTTP response returned by the 'Response' property of this exception for details.</Message>
      <Source>WebApiTraceExample</Source>
      <StackTrace>   at WebApiTraceExample.Controllers.ExceptionController.Get() in F:\WebApiTrace\WebApiTraceExample\Controllers\ExceptionController.cs:line 17
     at lambda_method(Closure , Object , Object[] )
     at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.&lt;&gt;c__DisplayClassc.&lt;GetExecutor&gt;b__6(Object instance, Object[] methodParameters)
     at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.Execute(Object instance, Object[] arguments)
     at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ExecuteAsync(HttpControllerContext controllerContext, IDictionary`2 arguments, CancellationToken cancellationToken)
  --- End of stack trace from previous location where exception was thrown ---
     at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
     at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
     at System.Web.Http.Tracing.ITraceWriterExtensions.&lt;TraceBeginEndAsyncCore&gt;d__18`1.MoveNext()</StackTrace>
    </Exception>
    <Events>
      <TraceEvent>
        <Category>System.Web.Http.Request</Category>
        <Message>http://localhost:9000/api/exception</Message>
        <Operation />
        <Operator />
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:11:10.4735405-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Message>Route='controller:exception'</Message>
        <Operation>SelectController</Operation>
        <Operator>DefaultHttpControllerSelector</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:11:10.4735405-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Operation>CreateController</Operation>
        <Operator>HttpControllerDescriptor</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:11:10.4735405-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Operation>Create</Operation>
        <Operator>DefaultHttpControllerActivator</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:11:10.4735405-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Operation>ExecuteAsync</Operation>
        <Operator>ExceptionController</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:11:10.4735405-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Action</Category>
        <Operation>SelectAction</Operation>
        <Operator>ApiControllerActionSelector</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:11:10.4735405-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.ModelBinding</Category>
        <Operation>ExecuteBindingAsync</Operation>
        <Operator>HttpActionBinding</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:11:10.4735405-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Action</Category>
        <Message>Action='Get()'</Message>
        <Operation>InvokeActionAsync</Operation>
        <Operator>ApiControllerActionInvoker</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:11:10.4735405-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Action</Category>
        <Message>Invoking action 'Get()'</Message>
        <Operation>ExecuteAsync</Operation>
        <Operator>ReflectedHttpActionDescriptor</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:11:10.4735405-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Operation>Dispose</Operation>
        <Operator>ExceptionController</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:11:12.4721941-05:00</EventTime>
      </TraceEvent>
    </Events>
  </TraceData>
```



- trace level "error", verbosity set to "verbose"

The trace will only show exceptions/errors. All events on the request pipeline will be shown, as well as end-time of the request:

    PUT http://localhost:9000/api/diagnostics/trace/settings HTTP/1.1
    Content-Type: application/json

    {"level":"error","verbosity":"verbose"}


    GET http://localhost:9000/api/exception HTTP/1.1
    ...

```xml
  <TraceData>
    <Method>GET</Method>
    <Url>http://localhost:9000/api/exception</Url>
    <StartTime>2015-06-07T12:15:36.2903057-05:00</StartTime>
    <EndTime>2015-06-07T12:15:39.3655731-05:00</EndTime>
    <TotalTime>3075.2674</TotalTime>
    <ControllerName>WebApiTraceExample.Controllers.ExceptionController</ControllerName>
    <ActionName>Invoking action 'Get()'</ActionName>
    <Exception>
      <Message>Processing of the HTTP request resulted in an exception. Please see the HTTP response returned by the 'Response' property of this exception for details.</Message>
      <Source>WebApiTraceExample</Source>
      <StackTrace>   at WebApiTraceExample.Controllers.ExceptionController.Get() in F:\WebApiTrace\WebApiTraceExample\Controllers\ExceptionController.cs:line 17
     at lambda_method(Closure , Object , Object[] )
     at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.&lt;&gt;c__DisplayClassc.&lt;GetExecutor&gt;b__6(Object instance, Object[] methodParameters)
     at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.Execute(Object instance, Object[] arguments)
     at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ExecuteAsync(HttpControllerContext controllerContext, IDictionary`2 arguments, CancellationToken cancellationToken)
  --- End of stack trace from previous location where exception was thrown ---
     at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
     at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
     at System.Web.Http.Tracing.ITraceWriterExtensions.&lt;TraceBeginEndAsyncCore&gt;d__18`1.MoveNext()</StackTrace>
    </Exception>
    <Events>
      <TraceEvent>
        <Category>System.Web.Http.Request</Category>
        <Message>http://localhost:9000/api/exception</Message>
        <Operation />
        <Operator />
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2903057-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Message>Route='controller:exception'</Message>
        <Operation>SelectController</Operation>
        <Operator>DefaultHttpControllerSelector</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2903057-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Message>Exception</Message>
        <Operation>SelectController</Operation>
        <Operator>DefaultHttpControllerSelector</Operator>
        <Kind>End</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2903057-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Operation>CreateController</Operation>
        <Operator>HttpControllerDescriptor</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2903057-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Operation>Create</Operation>
        <Operator>DefaultHttpControllerActivator</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2903057-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Message>WebApiTraceExample.Controllers.ExceptionController</Message>
        <Operation>Create</Operation>
        <Operator>DefaultHttpControllerActivator</Operator>
        <Kind>End</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2903057-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Message>WebApiTraceExample.Controllers.ExceptionController</Message>
        <Operation>CreateController</Operation>
        <Operator>HttpControllerDescriptor</Operator>
        <Kind>End</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2903057-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Operation>ExecuteAsync</Operation>
        <Operator>ExceptionController</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2903057-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Action</Category>
        <Operation>SelectAction</Operation>
        <Operator>ApiControllerActionSelector</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2903057-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Action</Category>
        <Message>Selected action 'Get()'</Message>
        <Operation>SelectAction</Operation>
        <Operator>ApiControllerActionSelector</Operator>
        <Kind>End</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2913066-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.ModelBinding</Category>
        <Operation>ExecuteBindingAsync</Operation>
        <Operator>HttpActionBinding</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2913066-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.ModelBinding</Category>
        <Operation>ExecuteBindingAsync</Operation>
        <Operator>HttpActionBinding</Operator>
        <Kind>End</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2913066-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Action</Category>
        <Message>Action='Get()'</Message>
        <Operation>InvokeActionAsync</Operation>
        <Operator>ApiControllerActionInvoker</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2913066-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Action</Category>
        <Message>Invoking action 'Get()'</Message>
        <Operation>ExecuteAsync</Operation>
        <Operator>ReflectedHttpActionDescriptor</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:36.2913066-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Action</Category>
        <Operation>ExecuteAsync</Operation>
        <Operator>ReflectedHttpActionDescriptor</Operator>
        <Kind>End</Kind>
        <Level>Error</Level>
        <EventTime>2015-06-07T12:15:39.3645717-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Action</Category>
        <Operation>InvokeActionAsync</Operation>
        <Operator>ApiControllerActionInvoker</Operator>
        <Kind>End</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:39.3655731-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Operation>ExecuteAsync</Operation>
        <Operator>ExceptionController</Operator>
        <Kind>End</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:39.3655731-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Request</Category>
        <Message>Content-type='none', content-length=unknown</Message>
        <Operation />
        <Operator />
        <Kind>End</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:39.3655731-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Operation>Dispose</Operation>
        <Operator>ExceptionController</Operator>
        <Kind>Begin</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:39.3655731-05:00</EventTime>
      </TraceEvent>
      <TraceEvent>
        <Category>System.Web.Http.Controllers</Category>
        <Operation>Dispose</Operation>
        <Operator>ExceptionController</Operator>
        <Kind>End</Kind>
        <Level>Info</Level>
        <EventTime>2015-06-07T12:15:39.3655731-05:00</EventTime>
      </TraceEvent>
    </Events>
  </TraceData>
```







### Trace level: debug


When trace `level` is set to "debug", every single call to the API will produce trace.

The `verbosity` setting is respected the same way as with trace level of "error".







## TODOs


This is a list of the desired changes we would like to include in future releases:

- Filter with regular expressions

Provide filtering using regular expressions, so that only specific API URLs get traced in debug mode. Note: all exceptions and errors still should get traced


- Query for the last (N) traces

Provide memory resident list of the last (N) traces and give ability to query this list


- Summary numbers API

Be able to get summary numbers since the API is up and running. Provide a way to query the summaries: 
which API commands raise most errors,
which API commands are the slowest,
which AP commands are mostly used, etc.


- Complementary trace writers

Additional project with more trace writers, like 
send errors to email, 
write to log file,
post to URL, etc.


- Global versus per writer trace settings

Provide a way for the individual trace writers so that they can have their 
own trace level and verbosity settings which overrides the global one.

- Bug: changing settings (especially verbosity), affects the currently running tracers.
They should stay consistent and not change. It is nice to have as the effect is minimal, 
usually trace for the request that changes the settings may be affected.

