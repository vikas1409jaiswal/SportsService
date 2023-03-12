// Copyright (c) Medidata Solutions. All rights reserved.

using zipkin4net;

namespace CricketService.Hangfire.Contracts
{
    public interface ITraceRecorder
    {
        void RecordIncoming(Trace trace, string rpc);

        void RecordOutgoing(Trace trace, string rpc);
    }
}