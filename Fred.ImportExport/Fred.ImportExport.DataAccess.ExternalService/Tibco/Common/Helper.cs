using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.ImportExport.DataAccess.ExternalService.Tibco.Common
{
    public static class Helper
    {

        /// <summary>
        /// Vérifie si l'url Tibco est bien configurée au niveau de la Web.config
        /// L'url utilisée dans Settings est juste pour que ExportmoyenPointageProxy compile !
        /// </summary>
        /// <param name="key">Key configuration</param>
        /// <returns>L'url de à utiliser , null si aucune url valid n'a été trouvée</returns>
        public static string GetUrlWebConfig(string key)
        {

            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            bool isValid = appSettings.AllKeys.Contains(key)
                            && !string.IsNullOrEmpty(appSettings[key])
                            && !appSettings[key].Contains(key);
            if (isValid)
            {
                return appSettings[key];
            }

            return null;
        }
    }
}
