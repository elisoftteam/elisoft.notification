namespace Elisoft.Notificator.Slack.Services
{
  public interface ISlackNotificator
  {
    Task<bool> SendMessageAsync(string webhookUrl, string channelName, string messageText);
  }
}