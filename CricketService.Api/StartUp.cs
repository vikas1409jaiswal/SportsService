using System.Text.Json;
using CricketService.Data.Extensions;
using CricketService.Data.Options.Configs;
using CricketService.Data.Repositories;
using CricketService.Data.Repositories.Interfaces;

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
            services.AddAuthorization();
            services.AddCricketServiceDataLayer(Configuration);

            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=CricketMatch}/{action=Index}");
            });
        }
    }
}
