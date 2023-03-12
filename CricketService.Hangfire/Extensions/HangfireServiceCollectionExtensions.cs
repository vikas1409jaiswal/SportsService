// Copyright (c) Medidata Solutions. All rights reserved.

using Hangfire.Common;
using CricketService.Hangfire.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace CricketService.Hangfire.Extensions
{
    public static class HangfireServiceCollectionExtensions
    {
        public static IServiceCollection AddHangfireAttributes(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<JobFilterAttribute, CustomAutomaticRetryAttribute>();
            serviceCollection.AddSingleton<JobFilterAttribute, PreserveQueueAttribute>();
            serviceCollection.AddSingleton<JobFilterAttribute, LogEverythingAttribute>();
            serviceCollection.AddSingleton<JobFilterAttribute, ExpirationTimeAttribute>();
            return serviceCollection;
        }
    }
}