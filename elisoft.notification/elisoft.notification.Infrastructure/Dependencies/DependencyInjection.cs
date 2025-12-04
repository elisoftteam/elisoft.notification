using elisoft.notification.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace elisoft.notification.Infrastructure.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            LoggingConfig.ConfigureServices(services, configuration);


            return services;
        }
    }
}