using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elisoft.notification.Configuration.Configuration
{
    public interface IConfig
    {
        string SlackWebhookUrl { get; }
        string LogsDirectory { get; }
        string ApiKey { get; }
    }
}
