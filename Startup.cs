using System.Device.I2c;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using SmartApartmentSystem.Infrastructure;
using System.Reflection;
using Microsoft.OpenApi.Models;
using SmartApartmentSystem.Data;
using SmartApartmentSystem.Queries;
using SmartApartmentSystem.RaspberryIO.Temperature;
using SmartApartmentSystem.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SmartApartmentSystem.Scheduler;

namespace SmartApartmentSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            services.AddMediatR(typeof(DeleteScheduleCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetModuleStatusQuery).GetTypeInfo().Assembly);

            services.AddDbContext<SasDbContext>(options =>
                  options.UseSqlite("Data Source=SasDb.db"));

            services.AddHostedService<TimedHostedService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddSingleton<TemperatureDevice>();
            services.AddSingleton<MainScheduler>();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            //if (app.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V2"); });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            var serviceProvider = app.ApplicationServices;
            serviceProvider.GetService<MainScheduler>();
        }
    }
}
