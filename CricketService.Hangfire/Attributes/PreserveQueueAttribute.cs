// Copyright (c) Medidata Solutions. All rights reserved.

using Hangfire.Common;
using Hangfire.States;
using CricketService.Hangfire.Configs;

namespace CricketService.Hangfire.Attributes
{
    public class PreserveQueueAttribute : JobFilterAttribute, IElectStateFilter
    {
        public void OnStateElection(ElectStateContext context)
        {
            var enqueuedState = context.CandidateState as EnqueuedState;
            if (enqueuedState != null)
            {
                var queueName = context.GetJobParameter<string>(JobParameters.QueueName);
                if (!string.IsNullOrWhiteSpace(queueName))
                {
                    enqueuedState.Queue = queueName;
                }
                else
                {
                    context.SetJobParameter(JobParameters.QueueName, enqueuedState.Queue);
                }
            }
        }
    }
}