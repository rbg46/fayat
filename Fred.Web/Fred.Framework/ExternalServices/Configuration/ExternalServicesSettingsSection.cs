using System.Configuration;
using Fred.Framework.ExternalServices.Configuration.ImportExport;

namespace Fred.Framework.ExternalServices.Configuration
{
    public class ExternalServicesSettingsSection : ConfigurationSection
    {
        private const string ImportExportName = "importExport";

        [ConfigurationProperty(ImportExportName, IsRequired = true)]
        public ImportExportSettings ImportExport
        {
            get => this[ImportExportName] as ImportExportSettings;
            set => this[ImportExportName] = value;
        }
    }
}
