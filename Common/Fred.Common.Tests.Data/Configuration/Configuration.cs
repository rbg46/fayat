using System;
using System.IO;
using System.Text;

namespace Fred.Common.Tests.Data.Configuration
{
    /// <summary>
    /// Classe de configuration de Fred
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Permet de récuperer le chemin du répertoire d'execution de Fred
        /// </summary>
        /// <returns>le chemin du répertoire FRED</returns>
        public static string GetFredbaseDirectory()
        {
            var extraction = AppDomain.CurrentDomain.BaseDirectory.Split('\\');
            var FredbaseDirectory = new StringBuilder();
            int i = 0;
            while (extraction[i] != "Fred.Business.Tests")
            {
                FredbaseDirectory.Append(extraction[i]);
                FredbaseDirectory.Append("\\");
                i++;
            }
            return Path.Combine(FredbaseDirectory.ToString(), "Fred.Web\\");
        }
    }
}
