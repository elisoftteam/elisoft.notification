using Elisoft.Notificator.Configuration.Configuration;
using Elisoft.Notificator.Core.Requests;
using Elisoft.Pushover.Services;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Elisoft.Notificator.Core.Handlers
{
    public class PushoverNotificationRequestHandler : RequestHandlerAsync<PushoverNotificationRequest>
    {
        private readonly IPushoverNotificator _pushoverNotificator;
        private readonly IConfig _config;
        private readonly ILogger<PushoverNotificationRequestHandler> _logger;

        public PushoverNotificationRequestHandler(
            IPushoverNotificator pushoverNotificator,
            IConfig config,
            ILogger<PushoverNotificationRequestHandler> logger)
        {
            _pushoverNotificator = pushoverNotificator;
            _config = config;
            _logger = logger;
        }

        public override async Task<PushoverNotificationRequest> HandleAsync(
            PushoverNotificationRequest command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending message via Pushover.");

            await _pushoverNotificator.SendMessageAsync(
                _config.PushoverApiToken,
                _config.PushoverUserKey,
                command.Message,
                command.Title,
                command.Priority);

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
