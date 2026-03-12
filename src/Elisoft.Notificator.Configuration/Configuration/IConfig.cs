using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elisoft.Notificator.Configuration.Configuration
{
  public interface IConfig
  {
    string LogsDirectory { get; }
    string ApiKey { get; }
    
    string TwilioAccountSid { get; }
    string TwilioAuthToken { get; }
    string TwilioFromNumber { get; }
    string PushoverApiToken { get; }
    string PushoverUserKey { get; }
    string EmailSmtpHost { get; }
    int EmailSmtpPort { get; }
    bool EmailUseSsl { get; }
    string EmailUsername { get; }
    string EmailPassword { get; }
    string EmailFromAddress { get; }
    string? EmailFromName { get; }
  }
}
