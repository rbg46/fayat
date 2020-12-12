using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Fred.Framework.Web.AppInsights
{
    public class UnwantedTelemetryFilter : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor nextTelemetryProcessor;

        public UnwantedTelemetryFilter(ITelemetryProcessor nextTelemetryProcessor)
        {
            this.nextTelemetryProcessor = nextTelemetryProcessor;
        }

        public void Process(ITelemetry item)
        {
            if (IsRequestExcluded())
                return;

            nextTelemetryProcessor.Process(item);

            bool IsRequestExcluded()
            {
                if (!(item is RequestTelemetry request))
                    return false;

                bool isRequestExcluded = request.Name?.Contains("signalr") ?? false;

                return isRequestExcluded;
            }
        }
    }
}