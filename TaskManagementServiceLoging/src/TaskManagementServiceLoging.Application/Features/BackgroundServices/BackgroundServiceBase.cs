using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using TaskManagementServiceLoging.Domain.Uptrace;

namespace TaskManagementServiceLoging.Application.Features.BackgroundServices
{
    public abstract class BackgroundServiceBase<T> : BackgroundService
    {
        private readonly string _serviceName;
        private readonly OpenTelemetryHolder _openTelemetryHolder;

        protected BackgroundServiceBase(string serviceName, OpenTelemetryHolder openTelemetryHolder)
        {
            _serviceName = serviceName;
            _openTelemetryHolder = openTelemetryHolder;
        }

        protected async Task ProcessMessage(T message)
        {
            using (var activity = _openTelemetryHolder.ActivitySource.StartActivity(_serviceName))
            {
                try
                {
                    await WorkWithMessageAsync(message);
                }
                catch (Exception ex)
                {
                    activity?.AddException(ex);
                    activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                }
            }
        }

        protected abstract Task WorkWithMessageAsync(T? message);
    }
}
