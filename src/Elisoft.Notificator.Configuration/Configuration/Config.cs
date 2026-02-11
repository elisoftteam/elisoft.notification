using Microsoft.Extensions.Configuration;

namespace Elisoft.Notificator.Configuration.Configuration
{
    public class Config : IConfig
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

        public string TwilioAccountSid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_config["Twilio:AccountSid"]))
                {
                    throw new Exception("Twilio AccountSid is not set in appsettings.json");
                }
                return _config["Twilio:AccountSid"] ?? "";
            }
        }

        public string TwilioAuthToken
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_config["Twilio:AuthToken"]))
                {
                    throw new Exception("Twilio AuthToken is not set in appsettings.json");
                }
                return _config["Twilio:AuthToken"] ?? "";
            }
        }

        public string TwilioFromNumber
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_config["Twilio:FromNumber"]))
                {
                    throw new Exception("Twilio FromNumber is not set in appsettings.json");
                }
                return _config["Twilio:FromNumber"] ?? "";
            }
        }

        public string PushoverApiToken
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_config["Pushover:ApiToken"]))
                {
                    throw new Exception("Pushover ApiToken is not set in appsettings.json");
                }
                return _config["Pushover:ApiToken"] ?? "";
            }
        }

        public string PushoverUserKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_config["Pushover:UserKey"]))
                {
                    throw new Exception("Pushover UserKey is not set in appsettings.json");
                }
                return _config["Pushover:UserKey"] ?? "";
            }
        }
    }
}
