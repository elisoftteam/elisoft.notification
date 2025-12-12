using elisoft.notification.Configuration.Configuration;
using elisoft.notification.Slack.Services;
using elisoft.notification.Slack.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace elisoft.notification.Infrastructure.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            LoggingConfig.ConfigureServices(services, configuration);
            services.AddSingleton<IConfig, Config>();
            services.AddHttpClient<ISlackNotificationService, SlackNotificationService>();

            return services;
        }
    }
}