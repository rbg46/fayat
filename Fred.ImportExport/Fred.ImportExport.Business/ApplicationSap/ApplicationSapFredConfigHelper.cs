using Fred.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Fred.ImportExport.Business.ApplicationSap
{
    /// <summary>
    /// Cette classe permet de recupérer les clés dans AppSettings.
    /// Elle recupere la cle SAP pour un type d'organisation (groupe ou societe) et le Code (groupe ou societe)
    /// </summary>
    public class ApplicationSapFredConfigHelper
    {
        private IEnumerable<KeyValuePair<string, string>> allAppSettingsKeys;

        /// <summary>
        ///  Recupere la cle SAP pour un type d'organisation (groupe ou societe) et le Code (groupe ou societe)
        /// </summary>
        /// <param name="organisationKeyType">un type d'organisation (groupe ou societe)</param>
        /// <param name="code"> le Code (groupe ou societe)</param>
        /// <returns>Un ApplicationSapParameter avec la propriete Found si la clé a été trouvée</returns>
        public ApplicationSapParameter GetApplicationSapParameterOnWebConfig(OrganisationType organisationKeyType, string code)
        {
            if (allAppSettingsKeys == null)
            {
                //evites de recupere toutes les clés lors d'un deuxieme appel.
                allAppSettingsKeys = ToPairs(ConfigurationManager.AppSettings);
            }

            var result = new ApplicationSapParameter();

            //construction des keys
            var urlKey = SapParameterHelper.BuildApplicationsSapKey(SapParameterHelper.UrlPrefix, organisationKeyType, code);
            var loginKey = SapParameterHelper.BuildApplicationsSapKey(SapParameterHelper.LoginPrefix, organisationKeyType, code);
            var passwordKey = SapParameterHelper.BuildApplicationsSapKey(SapParameterHelper.PasswordPrefix, organisationKeyType, code);

            //Recuperation
            var url = allAppSettingsKeys.FirstOrDefault(kvp => kvp.Key.ToLower() == urlKey).Value;
            var login = allAppSettingsKeys.FirstOrDefault(kvp => kvp.Key.ToLower() == loginKey).Value;
            var password = allAppSettingsKeys.FirstOrDefault(kvp => kvp.Key.ToLower() == passwordKey).Value;

            result.Url = url;
            result.Login = login;
            result.Password = password;

            return result;
        }

        private static IEnumerable<KeyValuePair<string, string>> ToPairs(NameValueCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            return collection.Cast<string>().Select(key => new KeyValuePair<string, string>(key, collection[key]));
        }
    }
}
