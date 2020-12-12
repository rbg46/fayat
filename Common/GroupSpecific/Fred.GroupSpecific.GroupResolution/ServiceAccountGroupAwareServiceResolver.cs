using System.Collections.Generic;
using Fred.DesignPatterns.DI;
using Fred.Entities;
using Fred.Framework.Security;

namespace Fred.GroupSpecific.GroupResolution
{
    public class ServiceAccountGroupAwareServiceResolver : GroupAwareServiceResolver
    {
        private readonly ISecurityManager securityManager;

        public ServiceAccountGroupAwareServiceResolver(IDependencyInjectionService dependencyInjectionService, ISecurityManager securityManager)
            : base(dependencyInjectionService)
        {
            this.securityManager = securityManager;
        }

        public override string GetCurrentGroupCode()
        {
            string username = securityManager.GetCurrentServiceAccount();
            Dictionary<string, string> serviceAccountGroupCodes = ServiceAccountGroupCodes();

            if (string.IsNullOrEmpty(username) || !serviceAccountGroupCodes.ContainsKey(username))
            {
                return Constantes.CodeGroupeDefault;
            }

            return serviceAccountGroupCodes[username];

            Dictionary<string, string> ServiceAccountGroupCodes() => new Dictionary<string, string>
            {
                { "userserviceFtp", Constantes.CodeGroupeFTP },
                { "userserviceStorm", Constantes.CodeGroupeRZB },
                { "userserviceFes", Constantes.CodeGroupeFES }
            };
        }
    }
}
