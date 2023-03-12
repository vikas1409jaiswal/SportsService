using zipkin4net;

namespace CricketService.Hangfire.Models
{
    public class SpanStateDto
    {
        // default constructor needed for hangfire serialization
        public SpanStateDto()
        {
        }

        public SpanStateDto(ITraceContext traceContext)
        {
            TraceIdHigh = traceContext.TraceIdHigh;
            TraceId = traceContext.TraceId;
            ParentSpanId = traceContext.ParentSpanId;
            SpanId = traceContext.SpanId;
            Sampled = traceContext.Sampled;
            Debug = traceContext.Debug;
        }

        public long TraceIdHigh { get; set; }

        public long TraceId { get; set; }

        public long? ParentSpanId { get; set; }

        public long SpanId { get; set; }

        public bool? Sampled { get; set; }

        public bool Debug { get; set; }

        public SpanState ConvertToSpanState()
        {
            return new SpanState(TraceIdHigh, TraceId, ParentSpanId, SpanId, Sampled, Debug);
        }
    }
}