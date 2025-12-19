using Elisoft.Notificator.Core.Requests;
using Elisoft.Notificator.Slack.Services;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;
using Serilog.Core;
using System.Text.Json;

namespace Elisoft.Notificator.Core.Handlers
{
  public class SlackNotificationRequestHandler : RequestHandlerAsync<SlackNotificationRequest>
  {
    private readonly ISlackNotificator _slackNotificator;
    private readonly ILogger<SlackNotificationRequestHandler> _logger;

    public SlackNotificationRequestHandler(ISlackNotificator slackNotificator, ILogger<SlackNotificationRequestHandler> logger)
    {
      _slackNotificator = slackNotificator;
      _logger = logger;
    }

    public override async Task<SlackNotificationRequest> HandleAsync(SlackNotificationRequest command, CancellationToken cancellationToken = default)
    {
      _logger.LogInformation($"Sent to slack. Channel name: {command.ChannelName}");

      await _slackNotificator.SendMessageAsync(command.WebhookUrl, command.ChannelName, command.Message);
      return await base.HandleAsync(command, cancellationToken);
    }
  }
}