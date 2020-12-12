using System.Configuration;

namespace Fred.Framework.ExternalServices.Configuration.ImportExport
{
    [ConfigurationCollection(typeof(ServiceAccountSettings))]
    public class ServiceAccountSettingsCollection : ConfigurationElementCollection
    {
        public ServiceAccountSettings GetByGroupCode(string groupCode) => BaseGet(groupCode) as ServiceAccountSettings;
        protected override ConfigurationElement CreateNewElement() => new ServiceAccountSettings();
        protected override object GetElementKey(ConfigurationElement element) => (element as ServiceAccountSettings).GroupCode;
    }
}
