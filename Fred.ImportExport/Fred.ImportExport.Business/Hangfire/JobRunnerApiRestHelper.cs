using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Fred.Entities;
using Fred.Framework.ExternalServices.Configuration;
using Fred.Framework.ExternalServices.Configuration.ImportExport;
using Fred.Framework.Services;

namespace Fred.ImportExport.Business.Hangfire
{
    public static class JobRunnerApiRestHelper
    {
        private const string ExternalServicesSectionName = "externalServices";
        private const string JobRunnerApiPath = "api/JobRunner";

        private static readonly Uri WebApiRootUri;
        private static readonly string WebApiTokenUrl;
        private static readonly Dictionary<string, Credentials> JobCredentials;

        static JobRunnerApiRestHelper()
        {
            ImportExportSettings importExportSettings = LoadConfigurationSettings();

            WebApiRootUri = new Uri(importExportSettings.Url);
            WebApiTokenUrl = new Uri(WebApiRootUri, importExportSettings.TokenPath).ToString();
            JobCredentials = LoadCredentials();

            ImportExportSettings LoadConfigurationSettings()
            {
                var section = ConfigurationManager.GetSection(ExternalServicesSectionName) as ExternalServicesSettingsSection;

                return section?.ImportExport;
            }

            Dictionary<string, Credentials> LoadCredentials()
            {
                IEnumerable<ServiceAccountSettings> serviceAccountSettings = importExportSettings.ServiceAccounts.Cast<ServiceAccountSettings>();

                return serviceAccountSettings.ToDictionary(sas => sas.GroupCode, sas => new Credentials { Username = sas.Username, Password = sas.Password });
            }
        }

        public static async Task PostAsync(string endpoint, string groupCode)
        {
            await PostAsync(endpoint, groupCode, null);
        }

        public static async Task PostAsync(string endpoint, string groupCode, object parameter)
        {
            string webApiFullUrl = GetEndpointUrl(endpoint);
            RestClient restClient = GetRestClientForGroup(groupCode);

            await restClient.PostAndEnsureSuccessAsync(webApiFullUrl, parameter);
        }

        public static async Task<T> PostAsync<T>(string endpoint, string groupCode, object parameter)
        {
            string webApiFullUrl = GetEndpointUrl(endpoint);
            RestClient restClient = GetRestClientForGroup(groupCode);

            return await restClient.PostAndEnsureSuccessAsync<T>(webApiFullUrl, parameter);
        }

        private static string GetEndpointUrl(string endpoint)
        {
            var jobWebApiFullUri = new Uri(WebApiRootUri, JobRunnerApiPath);

            return $"{jobWebApiFullUri}/{endpoint}";
        }

        private static RestClient GetRestClientForGroup(string groupCode)
        {
            if (!JobCredentials.ContainsKey(groupCode))
            {
                groupCode = Constantes.CodeGroupeDefault;
            }

            Credentials credentials = JobCredentials[groupCode];

            return new RestClient(credentials.Username, credentials.Password, WebApiTokenUrl);
        }
    }
}
