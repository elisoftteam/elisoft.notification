using Elisoft.Email.Services;
using Elisoft.Notificator.Configuration.Configuration;
using Elisoft.Notificator.Core.Requests;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Elisoft.Notificator.Core.Handlers
{
    public class EmailNotificationRequestHandler : RequestHandlerAsync<EmailNotificationRequest>
    {
        private readonly IEmailNotificator _emailNotificator;
        private readonly IConfig _config;
        private readonly ILogger<EmailNotificationRequestHandler> _logger;

        public EmailNotificationRequestHandler(
            IEmailNotificator emailNotificator,
            IConfig config,
            ILogger<EmailNotificationRequestHandler> logger)
        {
            _emailNotificator = emailNotificator;
            _config = config;
            _logger = logger;
        }

        public override async Task<EmailNotificationRequest> HandleAsync(
            EmailNotificationRequest command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending message via Email.");

            await _emailNotificator.SendMessageAsync(
                _config.EmailSmtpHost,
                _config.EmailSmtpPort,
                _config.EmailUseSsl,
                _config.EmailUsername,
                _config.EmailPassword,
                _config.EmailFromAddress,
                _config.EmailFromName,
                command.To,
                command.Subject,
                command.Message,
                command.IsBodyHtml);

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
