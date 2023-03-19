using AutoMapper;
using CricketService.Api.Filters;
using CricketService.Api.Handlers;
using CricketService.Api.Middlewares;
using CricketService.Data.Extensions;
using CricketService.Data.Mappings;
using CricketService.Data.Options.Configs;
using CricketService.Data.Repositories;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Hangfire.Attributes;
using CricketService.Hangfire.Configs;
using CricketService.Hangfire.Contracts;
using CricketService.Hangfire.Extensions;
using CricketService.Hangfire.Postgres.Extensions;
using CricketService.Hangfire.Tracing;
using Hangfire;
using Hangfire.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CricketService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<CricketServiceContextOptions>().Bind(Configuration.GetSection(CricketServiceContextOptions.SectionName));
            services.AddOptions<HangfireOptions>().Bind(Configuration.GetSection(HangfireOptions.DefaultSectionName));
            services.AddScoped<ICricketPlayerRepository, CricketPlayerRepository>();
            services.AddScoped<ICricketMatchRepository, CricketMatchRepository>();
            services.AddScoped<ICricketTeamRepository, CricketTeamRepository>();
            services.AddScoped<IHangfireRepository, HangfireRepository>();
            services.AddCricketServiceDataLayer(Configuration);

            services.AddControllers(options =>
            {
                options.Filters.Add(new ModelStateExceptionFilter(new ValidationErrorHandler()));
            })
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
             });

            services.AddSingleton<ValidationErrorHandler>()
                .AddScoped<ModelStateExceptionFilter>();

            services.AddHangfirePostgres(Configuration);

            services.TryAddSingleton<ITraceRecorder>(new TraceRecorder("Cricket Service"));
            services.AddSingleton<JobFilterAttribute, TracingClientJobFilterAttribute>();
            services.AddSingleton<JobFilterAttribute, TracingServerJobFilterAttribute>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Swagger API Documentation",
                    Description = "Demo API for showing swagger",
                    Version = "v1",
                });
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CricketServiceProfile());
            });

            services.AddSingleton(mapperConfig.CreateMapper());
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IBackgroundJobClient backgroundJobClient)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseHangfireFilters();

            app.UseResultModifier();

            backgroundJobClient.Schedule(() => Console.WriteLine("Hello world from hangfire"), TimeSpan.FromSeconds(2));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=CricketMatch}/{action=Index}");
            });

            app.UseHangfireDashboard("/hangfire");

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Demo API");
            });
        }
    }
}
