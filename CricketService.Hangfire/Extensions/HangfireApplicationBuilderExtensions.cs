using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CricketService.Hangfire.Extensions
{
    public static class HangfireApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHangfireFilters(
            this IApplicationBuilder app)
        {
            ExcludeJobFilters(new Type[]
              {
            typeof(CaptureCultureAttribute),
            typeof(AutomaticRetryAttribute),
              });

            var filters = app.ApplicationServices.GetServices<JobFilterAttribute>();
            if (filters != null && filters.Any())
            {
                AddJobFilters(filters.ToArray<object>());
            }

            return app;
        }

        private static void ExcludeJobFilters(Type[] excludingFilterTypes)
        {
            foreach (var typeToExclude in excludingFilterTypes)
            {
                var filter = GlobalJobFilters.Filters
                    .Where(x => x.Instance.GetType() == typeToExclude)
                    .SingleOrDefault()?.Instance;

                if (filter != null)
                {
                    GlobalJobFilters.Filters.Remove(filter);
                }
            }
        }

        private static void AddJobFilters(object[] filters)
        {
            const int OrderOffeset = 50;

            for (var i = 0; i < filters.Length; i++)
            {
                GlobalJobFilters.Filters.Add(filters[i], OrderOffeset + i);
            }
        }
    }
}