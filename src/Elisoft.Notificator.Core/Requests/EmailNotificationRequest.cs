using Paramore.Brighter;

namespace Elisoft.Notificator.Core.Requests
{
    public class EmailNotificationRequest : Command
    {
        public EmailNotificationRequest() : base(Guid.NewGuid())
        {
        }

        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsBodyHtml { get; set; }
    }
}
