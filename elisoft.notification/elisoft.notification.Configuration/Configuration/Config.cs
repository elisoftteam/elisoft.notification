using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace elisoft.notification.Configuration.Configuration
{
    public class Config:IConfig 
    {
        private readonly IConfiguration _config;
        public Config(IConfiguration config)
        {
            _config = config;
        }
        public string LogsDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_config["Logging:File:LogsDirectory"]))
                {
                    throw new Exception("LogsDirectory is not set in appsettings.json");
                }
                return _config["Logging:File:LogsDirectory"] ?? "";
            }
        }
        public string SlackWebhookUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_config["Slack:WebhookUrl"]))
                {
                    throw new Exception("Slack WebhookUrl is not set in appsettings.json");
                }
                return _config["Slack:WebhookUrl"] ?? "";
            }
        }
        public string ApiKey
        {
            get
            {

                if (string.IsNullOrWhiteSpace(_config["Authentication:ApiKey"]))
                {
                    throw new Exception("ApiKey is not set in appsettings.json");
                }
                return _config["Authentication:ApiKey"] ?? "";
            }
        }
    }


}
