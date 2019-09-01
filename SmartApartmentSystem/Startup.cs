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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }



    //internal class TimedHostedService : IHostedService, IDisposable
    //{
    //    private readonly ILogger _logger;
    //    private Timer _timer;

    //    public TimedHostedService(ILogger<TimedHostedService> logger)
    //    {
    //        _logger = logger;
    //    }

    //    public Task StartAsync(CancellationToken cancellationToken)
    //    {
    //        _logger.LogInformation("Timed Background Service is starting.");

    //        _timer = new Timer(DoWork, null, TimeSpan.Zero,
    //            TimeSpan.FromSeconds(5));

    //        return Task.CompletedTask;
    //    }

    //    private void DoWork(object state)
    //    {
    //        _logger.LogInformation("Timed Background Service is working.");
    //    }

    //    public Task StopAsync(CancellationToken cancellationToken)
    //    {
    //        _logger.LogInformation("Timed Background Service is stopping.");

    //        _timer?.Change(Timeout.Infinite, 0);

    //        return Task.CompletedTask;
    //    }

    //    public void Dispose()
    //    {
    //        _timer?.Dispose();
    //    }
    //}
}
