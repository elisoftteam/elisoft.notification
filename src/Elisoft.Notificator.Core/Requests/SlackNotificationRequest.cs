using Paramore.Brighter;

namespace Elisoft.Notificator.Core.Requests
{
  public class SlackNotificationRequest : Command
  {
    public SlackNotificationRequest() : base(Guid.NewGuid())
    {
    }
    public string WebhookUrl { get; set; }
    public string ChannelName { get; set; }
    public string Message { get; set; } = string.Empty;
  }
}
