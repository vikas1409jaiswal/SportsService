// Copyright (c) Medidata Solutions. All rights reserved.

using System.Reflection;
using CricketService.Hangfire.Contracts;
using CricketService.Hangfire.Models;
using Hangfire.Client;
using Hangfire.Common;
using Microsoft.Extensions.Logging;
using zipkin4net;

namespace CricketService.Hangfire.Attributes;

public class TracingClientJobFilterAttribute :
        JobFilterAttribute,
        IClientFilter
{
    private readonly ILogger<TracingClientJobFilterAttribute> _logger;
    private readonly ITraceRecorder _traceRecorder;

    public TracingClientJobFilterAttribute(
        ILogger<TracingClientJobFilterAttribute> logger,
        ITraceRecorder traceRecorder)
    {
        _logger = logger;
        _traceRecorder = traceRecorder;
    }

    public void OnCreating(CreatingContext context)
    {
        _logger.LogDebug("OnCreating()");

        if (Trace.Current is null)
        {
            _logger.LogError("Trace.Current is null");
            return;
        }

        if (context is null)
        {
            _logger.LogError("context is null");
            return;
        }

        var currentMethod = MethodBase.GetCurrentMethod();
        if (string.IsNullOrEmpty(currentMethod?.Name))
        {
            _logger.LogError("currentMethod.Name is null or empty");
            return;
        }

        var incomingApiTrace = Trace.Current;
        _traceRecorder.RecordIncoming(incomingApiTrace, currentMethod.Name);

        var outgoingTrace = Trace.Current = incomingApiTrace.Child();
        _traceRecorder.RecordOutgoing(outgoingTrace, currentMethod.Name);

        var dto = new SpanStateDto(outgoingTrace.CurrentSpan);
        context.SetJobParameter(typeof(SpanStateDto).Name, dto);
    }

    // This function exists to fulfill IClientFilter
    public void OnCreated(CreatedContext context)
    {
    }
}
