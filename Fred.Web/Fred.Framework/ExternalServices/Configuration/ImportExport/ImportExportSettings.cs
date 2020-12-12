using System.Configuration;

namespace Fred.Framework.ExternalServices.Configuration.ImportExport
{
    public class ImportExportSettings : ExternalServiceSettings
    {
        private const string ServiceAccountsCollectionName = "serviceAccounts";

        [ConfigurationProperty(ServiceAccountsCollectionName, IsRequired = true)]
        public ServiceAccountSettingsCollection ServiceAccounts { get => this[ServiceAccountsCollectionName] as ServiceAccountSettingsCollection; }
    }
}
