using System.Configuration;
using Fred.Framework.ExternalServices.Configuration;
using Fred.Framework.ExternalServices.Configuration.ImportExport;
using Fred.Framework.Services;

namespace Fred.Framework.ExternalServices.ImportExport
{
    public abstract class ImportExportServiceDescriptor : IImportExportServiceDescriptor
    {
        protected abstract string GroupCode { get; }

        public ExternalServiceEndpoint GetRestEndpoint()
        {
            ImportExportSettings importExportSettings = LoadConfigurationSettings();

            string baseUrl = importExportSettings.Url;
            RestClient restClient = GetRestClient();

            return new ExternalServiceEndpoint
            {
                BaseUrl = baseUrl,
                RestClient = restClient
            };

            ImportExportSettings LoadConfigurationSettings()
            {
                const string externalServicesSectionName = "externalServices";
                var section = ConfigurationManager.GetSection(externalServicesSectionName) as ExternalServicesSettingsSection;

                return section.ImportExport;
            }

            RestClient GetRestClient()
            {
                ServiceAccountSettings serviceAccount = importExportSettings.ServiceAccounts.GetByGroupCode(GroupCode);
                string tokenUrl = $"{baseUrl}/{importExportSettings.TokenPath}";

                return new RestClient(serviceAccount.Username, serviceAccount.Password, tokenUrl);
            }
        }
    }
}
