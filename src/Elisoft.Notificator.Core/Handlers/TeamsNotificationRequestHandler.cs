using Elisoft.Notificator.Core.Requests;
using Elisoft.Teams.Services;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Elisoft.Notificator.Core.Handlers
{
    public class TeamsNotificationRequestHandler : RequestHandlerAsync<TeamsNotificationRequest>
    {
        private readonly ITeamsNotificator _teamsNotificator;
        private readonly ILogger<TeamsNotificationRequestHandler> _logger;

        public TeamsNotificationRequestHandler(
            ITeamsNotificator teamsNotificator,
            ILogger<TeamsNotificationRequestHandler> logger)
        {
            _teamsNotificator = teamsNotificator;
            _logger = logger;
        }

        public override async Task<TeamsNotificationRequest> HandleAsync(
            TeamsNotificationRequest command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending message via Teams.");

            await _teamsNotificator.SendMessageAsync(command.WebhookUrl, command.Message);
            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
