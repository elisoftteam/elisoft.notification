using System.Text.Json.Serialization;

namespace Elisoft.Notificator.Core.Enums
{
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum NotificationEnumChannel
    {
        Slack,
        Teams,
    }
}
