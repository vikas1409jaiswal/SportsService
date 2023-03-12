// Copyright (c) Medidata Solutions. All rights reserved.

using System;
using System.Reflection;
using CricketService.Hangfire.Contracts;
using zipkin4net;

namespace CricketService.Hangfire.Tracing;

public class TraceRecorder : ITraceRecorder
{
    private readonly string _serviceName;

    public TraceRecorder(string serviceName)
    {
        _serviceName = serviceName;
    }

    public void RecordIncoming(
        Trace trace,
        string rpc)
    {
        RecordBase(trace);

        if (string.IsNullOrEmpty(rpc))
        {
            throw new ArgumentNullException(nameof(rpc));
        }

        trace.Record(Annotations.Rpc(rpc + "::incoming"));
    }

    public void RecordOutgoing(
        Trace trace,
        string rpc)
    {
        RecordBase(trace);

        if (string.IsNullOrEmpty(rpc))
        {
            throw new ArgumentNullException(nameof(rpc));
        }

        trace.Record(Annotations.Rpc(rpc + "::outgoing"));

        trace.Record(Annotations.ConsumerStart());
        trace.Record(Annotations.WireRecv());
        trace.Record(Annotations.WireSend());
        trace.Record(Annotations.ConsumerStop());
    }

    private void RecordBase(
        Trace trace)
    {
        if (trace is null)
        {
            throw new ArgumentNullException(nameof(trace));
        }

        trace.Record(Annotations.ServiceName(_serviceName));
        trace.Record(Annotations.Tag("ExecutingAssembly", Assembly.GetEntryAssembly()?.GetName().Name));
        trace.Record(Annotations.Tag("Class", typeof(TraceRecorder).FullName));
        trace.Record(Annotations.Tag("CorrelationId", trace.CorrelationId.ToString()));
        trace.Record(Annotations.Tag("CurrentSpan.TraceId", trace.CurrentSpan.TraceId.ToString()));
        trace.Record(Annotations.Tag("CurrentSpan.SpanId", trace.CurrentSpan.SpanId.ToString()));
        trace.Record(Annotations.Tag("CurrentSpan.ParentSpanId", trace.CurrentSpan.ParentSpanId.HasValue ? trace.CurrentSpan.ParentSpanId.ToString() : "NULL"));
        trace.Record(Annotations.Tag("CurrentSpan.TraceIdHigh", trace.CurrentSpan.TraceIdHigh.ToString()));
    }
}
