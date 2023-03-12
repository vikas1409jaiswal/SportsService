// Copyright (c) Medidata Solutions. All rights reserved.

using System;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;

namespace CricketService.Hangfire.Attributes
{
    public class LogEverythingAttribute :
        JobFilterAttribute,
        IClientFilter,
        IServerFilter,
        IElectStateFilter,
        IApplyStateFilter
    {
        private readonly ILogger<LogEverythingAttribute> _logger;

        public LogEverythingAttribute(ILogger<LogEverythingAttribute> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void OnCreating(CreatingContext context)
        {
            _logger.LogInformation(
                "Creating a job based on method `{0}`...",
                context.Job.Method.Name);
        }

        public void OnCreated(CreatedContext context)
        {
            _logger.LogInformation(
                "Job `{0}` has been created based on method `{1}`",
                context.BackgroundJob?.Id,
                context.Job.Method.Name);
        }

        public void OnPerforming(PerformingContext context)
        {
            _logger.LogInformation(
                "Job `{0}` starting to be performed",
                context.BackgroundJob.Id);
        }

        public void OnPerformed(PerformedContext context)
        {
            _logger.LogInformation(
                "Job `{0}` has been performed",
                context.BackgroundJob.Id);
        }

        public void OnStateElection(ElectStateContext context)
        {
            var failedState = context.CandidateState as FailedState;
            if (failedState != null)
            {
                _logger.LogError(
                    "Job `{0}` has been failed due to an exception `{1}`",
                    context.BackgroundJob.Id,
                    failedState.Exception);
            }
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            _logger.LogInformation(
                "Job `{0}` state was changed from `{1}` to `{2}`",
                context.BackgroundJob.Id,
                context.OldStateName,
                context.NewState.Name);
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            _logger.LogInformation(
                "Job `{0}` state `{1}` was unapplied.",
                context.BackgroundJob.Id,
                context.OldStateName);
        }
    }
}