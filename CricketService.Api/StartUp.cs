using CricketService.Api.Filters;
using CricketService.Api.Handlers;
using CricketService.Data.Extensions;
using CricketService.Data.Options.Configs;
using CricketService.Data.Repositories;
using CricketService.Data.Repositories.Interfaces;
using Newtonsoft.Json.Serialization;
using System;

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
            services.AddScoped<ICricketPlayerRepository, CricketPlayerRepository>();
            services.AddScoped<ICricketMatchRepository, CricketMatchRepository>();
            services.AddScoped<ICricketTeamRepository, CricketTeamRepository>();
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=CricketMatch}/{action=Index}");
            });
        }
    }
}
