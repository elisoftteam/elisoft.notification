using Paramore.Brighter;

namespace Elisoft.Notificator.Core.Requests
{
    public class TeamsNotificationRequest : Command
    {
        public TeamsNotificationRequest() : base(Guid.NewGuid())
        {
        }

        public string WebhookUrl { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
