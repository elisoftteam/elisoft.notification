using System;
using System.IO;
using elisoft.notification.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;


namespace elisoft.notification.Infrastructure.Configuration
{
    public class LoggingConfig
    {
        private const string ApplicationName = "elisoft.notification";

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var loggingOptions = new LoggingOptions();
            configuration.Bind("Logging", loggingOptions); 

            string logDirectory = loggingOptions.File.LogsDirectory;
            string logPath = Path.Combine(logDirectory, ApplicationName, $"{ApplicationName}.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: logPath,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });
        }
    }
}