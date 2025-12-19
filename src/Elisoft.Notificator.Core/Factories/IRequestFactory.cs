using Elisoft.Notificator.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Elisoft.Notificator.Core.Requests;

namespace Elisoft.Notificator.Core.Factories
{
  public interface IRequestFactory
  {
    object CreateRequest(NotificationEnumChannel channel, JsonElement jsonPayload);
  }
}
