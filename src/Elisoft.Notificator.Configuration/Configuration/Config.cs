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

        public string EmailSmtpHost
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_config["Email:SmtpHost"]))
                {
                    throw new Exception("Email SmtpHost is not set in appsettings.json");
                }

                return _config["Email:SmtpHost"] ?? "";
            }
        }

        public int EmailSmtpPort
        {
            get
            {
                if (!int.TryParse(_config["Email:SmtpPort"], out var port))
                {
                    throw new Exception("Email SmtpPort is not set correctly in appsettings.json");
                }

                return port;
            }
        }

        public bool EmailUseSsl
        {
            get
            {
                if (!bool.TryParse(_config["Email:UseSsl"], out var useSsl))
                {
                    throw new Exception("Email UseSsl is not set correctly in appsettings.json");
                }

                return useSsl;
            }
        }

        public string EmailUsername
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_config["Email:Username"]))
                {
                    throw new Exception("Email Username is not set in appsettings.json");
                }

                return _config["Email:Username"] ?? "";
            }
        }

        public string EmailPassword
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_config["Email:Password"]))
                {
                    throw new Exception("Email Password is not set in appsettings.json");
                }

                return _config["Email:Password"] ?? "";
            }
        }

        public string EmailFromAddress
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_config["Email:FromAddress"]))
                {
                    throw new Exception("Email FromAddress is not set in appsettings.json");
                }

                return _config["Email:FromAddress"] ?? "";
            }
        }

        public string? EmailFromName => _config["Email:FromName"];
    }
}
