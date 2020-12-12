using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Fred.Framework.Web.AppInsights
{
    /// <summary>
    /// Modifie la façon dont ApplicationInsight génère le nom d'une métric en supprimant les paramètres de l'url
    /// See : https://stackoverflow.com/questions/44691658/stop-application-insights-including-path-parameters-in-the-operation-name
    /// See : https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/176
    /// See : https://docs.microsoft.com/fr-fr/azure/azure-monitor/app/api-filtering-sampling
    /// </summary>
    /// <example>
    /// Au lieu d'afficher dans AppInsight les éléments de la façon suivante (par défaut) : 
    /// -----URL---------time----quantity 
    /// Get api/ci/12    100ms   15
    /// Get api/ci/20    200ms   10
    /// Get api/ci/25    150ms   20
    /// 
    /// Ils sont affichés de la façon suivante : 
    /// -----URL---------time----quantity 
    /// Get api/ci/{id}  144ms   45
    /// </example>
    public class AppInsightOperationName : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            try
            {
                if (!IsValidOperation())
                {
                    return;
                }

                // Modifie le nom de l'opération AppInsight en remplacant les valeurs trouvées par le regex par [id]
                telemetry.Context.Operation.Name = ReplaceGuidWithIdToken();

                // Récupère dans les metadata de Aps.Net les informations des paramètres de la requête pour indiquer le nom de [Id]
                telemetry.Context.Operation.Name = ReplaceIdWithParameterName(telemetry);
            }
            catch (Exception ex)
            {
                string error = $"[ApplicationInsight] Cannot update operation name {telemetry.Context.Operation.Name} due to {ex.Message}";
                Console.WriteLine(error);
            }

            bool IsValidOperation()
            {
                return telemetry.Context?.Operation?.Name != null;
            }

            string ReplaceGuidWithIdToken()
            {
                const string guidPattern = @"([a-z0-9]{8}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{12})";

                return Regex.Replace(telemetry.Context.Operation.Name, guidPattern, "[id]", RegexOptions.IgnoreCase);
            }
        }

        private string ReplaceIdWithParameterName(ITelemetry telemetry)
        {
            string operationNameWithParam = telemetry.Context.Operation.Name;
            var requestTelemetry = telemetry as RequestTelemetry;
            if (requestTelemetry?.Url == null)
                return operationNameWithParam;

            string[] ops = telemetry.Context.Operation.Name.Split(' ');
            var method = new HttpMethod(ops[0]);
            IHttpRouteData route = GlobalConfiguration.Configuration.Routes.GetRouteData(new HttpRequestMessage(method, requestTelemetry.Url));
            if (route == null)
                return operationNameWithParam;

            route = CheckIfSubRoute();
            string template = BuildRouteTemplate();
            operationNameWithParam = method + " /" + template;

            return operationNameWithParam;

            IHttpRouteData CheckIfSubRoute()
            {
                if (route.Values != null && route.Values.ContainsKey("MS_SubRoutes"))
                {
                    route = (route.Values["MS_SubRoutes"] as IHttpRouteData[])?.FirstOrDefault() ?? route;
                }

                return route;
            }

            string BuildRouteTemplate()
            {
                var pathParametersToLog = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { "controller", "action", "id" };

                string routeTemplate = route.Route?.RouteTemplate;
                if (routeTemplate == null)
                {
                    routeTemplate = string.Join("/", route.Values.Keys.Select(k => pathParametersToLog.Contains(k) ? route.Values[k] as string : k));
                }

                foreach (string param in pathParametersToLog.ToList())
                {
                    routeTemplate = routeTemplate.Replace($"{{{param}}}", route.Values.ContainsKey(param) ? route.Values[param] as string : $"{{{param}}}");
                }

                return routeTemplate;
            }
        }
    }
}
