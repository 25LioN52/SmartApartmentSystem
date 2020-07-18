using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using SmartApartmentSystem.Infrastructure;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SmartApartmentSystem.Scheduler;
using SmartApartmentSystem.Infrastructure.RaspberryIO.Temperature;
using SmartApartmentSystem.Application.Devices.WaterTemperature;
using SmartApartmentSystem.Infrastructure.Data;
using SmartApartmentSystem.Application;
using SmartApartmentSystem.Application.Jobs;
using SmartApartmentSystem.Application.Devices.WaterTemperature.Queries;

namespace SmartApartmentSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string dbConnection;
            if (Environment.IsDevelopment())
            {
                dbConnection = @"Data Source=SasDb.db";
                services.AddSingleton<IWaterTemperatureDevice, TemperatureDeviceStub>();
            }
            else
            {
                dbConnection = @"Data Source=/local-db/SasDb.db";
                services.AddSingleton<IWaterTemperatureDevice, TemperatureDevice>();
            }
            services.AddMvc();
            
            services.AddMediatR(typeof(GetTempQuery).GetTypeInfo().Assembly);

            services.AddDbContext<ISasDb, SasDbContext>(options =>
                  options.UseSqlite(dbConnection));

            services.AddHostedService<TimedHostedService>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddSingleton<MainScheduler>();

            var optionsBuilder = new DbContextOptionsBuilder<SasDbContext>();
            optionsBuilder.UseSqlite(dbConnection);
            services.AddSingleton<ISasDb>(f => new SasDbContext(optionsBuilder.Options));
            services.AddSingleton<Listener>();
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
            serviceProvider.GetService<Listener>();
        }
    }
}
