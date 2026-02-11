using Paramore.Brighter;

namespace Elisoft.Notificator.Core.Requests
{
    public class PushoverNotificationRequest : Command
    {
        public PushoverNotificationRequest() : base(Guid.NewGuid())
        {
        }

        public string Message { get; set; } = string.Empty;
        public string? Title { get; set; }
        public int? Priority { get; set; }
    }
}
