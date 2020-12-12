using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.ImportExport.Framework.Log
{
    public interface IImportExportLoggingService
    {
        /// <summary>
        /// Log une information
        /// </summary>
        /// <param name="message">l'information passee</param>
        void LogInfo(string message);

        /// <summary>
        /// Log une erreur
        /// </summary>
        /// <param name="message">le message passe</param>
        void LogError(string message);

        /// <summary>
        /// Log une erreur
        /// </summary>
        /// <param name="message">le message passe</param>
        /// <param name="innerException">l'exception parent</param>
        void LogError(string message, Exception innerException);

        /// <summary>
        /// Log une information de type debug
        /// </summary>
        /// <param name="message">le message passe</param>
        void LogDebug(string message);

        /// <summary>
        /// Retourne le niveau de log des logging rules par rapport a un critere de recherche
        /// </summary>
        /// <param name="regexTypeLogger">le critere de recherche en regex</param>
        /// <returns>Le niveau de log en string</returns>
        string GetMinLogLevelFor(string regexTypeLogger);

        /// <summary>
        /// Change le niveau de log des logging rules par rapport a un critere de recherche
        /// </summary>
        /// <param name="logLevel">le nouveau niveau de log</param>
        /// <param name="regex">le critere de recherche en regex</param>
        void SetLogLevel(string logLevel, string regex);
    }
}
