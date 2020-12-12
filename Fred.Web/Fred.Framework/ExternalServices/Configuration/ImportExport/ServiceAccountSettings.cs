using System.Configuration;
using Fred.Framework.ExternalServices.ImportExport;

namespace Fred.Framework.ExternalServices.Configuration.ImportExport
{
    public class ServiceAccountSettings : ConfigurationElement, IServiceAccount
    {
        private const string GroupCodePropertyName = "groupCode";
        private const string UsernamePropertyName = "username";
        private const string PasswordPropertyName = "password";

        [ConfigurationProperty(GroupCodePropertyName, IsRequired = true)]
        public string GroupCode
        {
            get => this[GroupCodePropertyName].ToString();
            set => this[GroupCodePropertyName] = value;
        }

        [ConfigurationProperty(UsernamePropertyName, IsRequired = true)]
        public string Username
        {
            get => this[UsernamePropertyName].ToString();
            set => this[UsernamePropertyName] = value;
        }

        [ConfigurationProperty(PasswordPropertyName, IsRequired = true)]
        public string Password
        {
            get => this[PasswordPropertyName].ToString();
            set => this[PasswordPropertyName] = value;
        }
    }
}
