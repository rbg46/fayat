using System.Configuration;

namespace Fred.Framework.ExternalServices.Configuration
{
    public abstract class ExternalServiceSettings : ConfigurationElement, IExternalServiceMetadata
    {
        private const string UrlPropertyName = "url";
        private const string TokenPathPropertyName = "tokenPath";

        [ConfigurationProperty(UrlPropertyName, IsRequired = true)]
        public string Url
        {
            get => this[UrlPropertyName].ToString();
            set => this[UrlPropertyName] = value;
        }

        [ConfigurationProperty(TokenPathPropertyName, IsRequired = true)]
        public string TokenPath
        {
            get => this[TokenPathPropertyName].ToString();
            set => this[TokenPathPropertyName] = value;
        }
    }
}
