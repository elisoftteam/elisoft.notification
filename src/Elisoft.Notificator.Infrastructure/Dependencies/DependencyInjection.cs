using Elisoft.Notificator.Configuration.Configuration;
using Elisoft.Notificator.Core.Factories;
using Elisoft.Notificator.Core.Handlers;
using Elisoft.Notificator.Core.Services;
using Elisoft.Notificator.Slack.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Elisoft.Notificator.Infrastructure.Dependencies
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
      LoggingConfig.ConfigureServices(services, configuration);
      services.AddSingleton<IConfig, Config>();
      services.AddScoped<IRequestFactory, RequestFactory>();
      services.AddScoped<INotificationService, NotificationService>();
      services.AddHttpClient<ISlackNotificator, SlackNotificator>();
      services.AddBrighter()
                  .AutoFromAssemblies(new[] { typeof(SlackNotificationRequestHandler).Assembly });
            return services;
    }
  }
}