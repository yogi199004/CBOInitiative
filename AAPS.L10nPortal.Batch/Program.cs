using AAPS.L10NPortal.Common;
using Hangfire;
using Hangfire.SqlServer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configValue = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
     .Enrich.WithProperty("Environment", "Batch - " + configValue.Environment)
    .WriteTo.Seq(configValue.Sequrl, period: TimeSpan.Zero, batchPostingLimit: 5).CreateLogger();
builder.Logging.AddSerilog(logger);


// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddHangfire(configuration => configuration
       .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
       .UseSimpleAssemblyNameTypeSerializer()
       .UseRecommendedSerializerSettings()
       .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
       {
           CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
           SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
           QueuePollInterval = TimeSpan.Zero,
           UseRecommendedIsolationLevel = true,
           DisableGlobalLocks = true,
           
       }));
builder.Services.AddHangfireServer();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseHangfireDashboard("/dashboard");

app.UseAuthorization();

app.MapControllers();

app.Run();
