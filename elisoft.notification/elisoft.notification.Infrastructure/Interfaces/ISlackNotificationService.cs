namespace elisoft.notification.Slack.Interfaces
{
    public interface ISlackNotificationService
    {
        Task<bool> SendMessageAsync(string message);
    }
}