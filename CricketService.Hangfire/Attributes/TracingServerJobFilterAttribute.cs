// Copyright (c) Medidata Solutions. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Hangfire.Common;
using Hangfire.Server;
using CricketService.Hangfire.Contracts;
using CricketService.Hangfire.Models;
using Microsoft.Extensions.Logging;
using zipkin4net;

namespace CricketService.Hangfire.Attributes;

public class TracingServerJobFilterAttribute :
    JobFilterAttribute,
    IServerFilter
{
    private readonly ILogger<TracingServerJobFilterAttribute> _logger;
    private readonly ITraceRecorder _traceRecorder;

    public TracingServerJobFilterAttribute(
        ILogger<TracingServerJobFilterAttribute> logger,
        ITraceRecorder traceRecorder)
    {
        _logger = logger;
        _traceRecorder = traceRecorder;
    }

    public void OnPerforming(PerformingContext context)
    {
        _logger.LogDebug("OnPerforming()");

        if (context is null)
        {
            _logger.LogError("context is null");
            return;
        }

        var dto = context.GetJobParameter<SpanStateDto>(typeof(SpanStateDto).Name);
        if (dto is null)
        {
            _logger.LogError("dto is null in context job parameters");
            return;
        }

        var currentMethod = MethodBase.GetCurrentMethod();
        if (string.IsNullOrEmpty(currentMethod?.Name))
        {
            _logger.LogError("currentMethod.Name is null or empty");
            return;
        }

        var incomingTrace = Trace.CreateFromId(dto.ConvertToSpanState());
        if (incomingTrace is null)
        {
            _logger.LogError("Unable to create trace from span job job parameter");
            return;
        }

        _traceRecorder.RecordIncoming(incomingTrace, currentMethod.Name);

        var outgoingTrace = Trace.Current = incomingTrace.Child();
        _traceRecorder.RecordOutgoing(outgoingTrace, currentMethod.Name);
    }

    // This function exists to fulfill IServerFilter
    public void OnPerformed(PerformedContext context)
    {
    }
}
