using Elisoft.Notificator.Core.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Elisoft.Notificator.Core.Models
{
    public class NotificationDto
    {
        public NotificationEnumChannel Channel { get; set; }
        public JsonElement Payload { get; set; }
    }
}
