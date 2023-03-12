using System;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using CricketService.Hangfire.Configs;
using CricketService.Hangfire.States;
using Microsoft.Extensions.Options;

namespace CricketService.Hangfire.Attributes
{
    public class ExpirationTimeAttribute :
        JobFilterAttribute,
        IApplyStateFilter,
        IElectStateFilter
    {
        public ExpirationTimeAttribute(IOptions<HangfireOptions> hangfireOptionsAccessor)
        {
            if (hangfireOptionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(hangfireOptionsAccessor));
            }

            var hangfireOptions = hangfireOptionsAccessor.Value;
            SucceedJobExpirationTimeout = hangfireOptions.SucceedJobExpirationTimeout;
            FailedJobExpirationTimeout = hangfireOptions.FailedJobExpirationTimeout;
        }

        public TimeSpan SucceedJobExpirationTimeout { get; set; }

        public TimeSpan FailedJobExpirationTimeout { get; set; }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            if (context.NewState.Name == SucceededState.StateName)
            {
                context.JobExpirationTimeout = SucceedJobExpirationTimeout;
            }
            else if (context.NewState.Name == FailedState.StateName)
            {
                context.JobExpirationTimeout = FailedJobExpirationTimeout;
            }
        }

        public void OnStateElection(ElectStateContext context)
        {
            var failedState = context.CandidateState as FailedState;
            if (failedState != null)
            {
                context.CandidateState = new FinalFailedState(failedState.Exception);
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}