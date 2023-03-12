using System;
using System.Diagnostics.CodeAnalysis;

namespace CricketService.Hangfire.Configs
{
    public class HangfireOptions
    {
        public const string DefaultSectionName = "Hangfire";

        public string DatabaseConnectionString { get; set; } = string.Empty;

        public int AutomaticRetryAttempts { get; set; }

        public int DelayInSecondsFuncBase { get; set; }

        public int DelayInSecondsFuncJitterMaxValue { get; set; }

        public TimeSpan FailedJobExpirationTimeout { get; set; }

        public TimeSpan SucceedJobExpirationTimeout { get; set; }
    }
}