using System.Configuration;
using System.Diagnostics;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace Fred.Framework.Web.AppInsights
{
    public static class AppInsightsConfig
    {
        public static void RegisterAppInsights()
        {
            TelemetryConfiguration activeTelemetryConfiguration = TelemetryConfiguration.Active;

            SetInstrumentationKey();
            ExcludeUnwantedTelemetry();
            ApplyOperationNameAggreation();
            DisableAppInsightsOnDebug();

            void SetInstrumentationKey()
            {
                string instrumentationKey = ConfigurationManager.AppSettings["Microsoft:ApplicationInsight:Key"];

                activeTelemetryConfiguration.InstrumentationKey = instrumentationKey;
            }

            void ExcludeUnwantedTelemetry()
            {
                TelemetryProcessorChainBuilder builder = activeTelemetryConfiguration.DefaultTelemetrySink.TelemetryProcessorChainBuilder;
                builder.Use((next) => new UnwantedTelemetryFilter(next));

                builder.Build();
            }

            void ApplyOperationNameAggreation() => activeTelemetryConfiguration.TelemetryInitializers.Add(new AppInsightOperationName());
        }

        [Conditional("DEBUG")]
        private static void DisableAppInsightsOnDebug()
        {
            TelemetryConfiguration.Active.DisableTelemetry = true;
        }
    }
}
