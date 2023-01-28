using System.Threading.Tasks;

using WebApiClientCore;
using WebApiClientCore.Attributes;

using Rubik.Service.Log;

namespace Rubik.Service.WebApi.Attributes
{
    public class RequestLoggingAttribute: LoggingFilterAttribute
    {
        protected override Task WriteLogAsync(ApiResponseContext context, LogMessage logMessage)
        {
            Logger.Instance.WebApi.Info(logMessage.ToIndentedString(spaceCount: 4));
            return Task.CompletedTask;
        }
    }
}
