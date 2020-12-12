using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;

namespace Fred.ImportExport.Framework.Log
{
    public class ImportExportLoggingService : IImportExportLoggingService
    {
        private static Logger importLogger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Log une information
        /// </summary>
        /// <param name="message">l'information passee</param>
        public void LogInfo(string message)
        {
            importLogger.Info(message);
        }

        /// <summary>
        /// Log une erreur
        /// </summary>
        /// <param name="message">le message passe</param>
        public void LogError(string message)
        {
            importLogger.Error(message);
        }

        /// <summary>
        /// Log une erreur
        /// </summary>
        /// <param name="message">le message passe</param>
        /// <param name="innerException">l'erreur parent</param>
        public void LogError(string message, Exception innerException)
        {
            importLogger.Error(innerException, message);
        }

        /// <summary>
        /// Log une information de type debug
        /// </summary>
        /// <param name="message">le message passe</param>
        public void LogDebug(string message)
        {
            importLogger.Debug(message);
        }

        /// <summary>
        /// Retourne le niveau de log des logging rules par rapport a un critere de recherche
        /// </summary>
        /// <param name="regexTypeLogger">le critere de recherche en regex</param>
        /// <returns>Le niveau de log en string</returns>
        public string GetMinLogLevelFor(string regexTypeLogger)
        {
            var rules = NLog.LogManager.Configuration.LoggingRules;

            if (regexTypeLogger == null || rules == null || !rules.Any())
            {
                return null;
            }

            //Rule regex pour matcher le type de logger
            Regex validator = new Regex(regexTypeLogger);

            if (rules.Any(x => x.Targets != null && x.Targets.Any() && validator.IsMatch(x.Targets[0].Name)))
            {
                string minLevel = rules.Where(x => validator.IsMatch(x.Targets[0].Name)).SelectMany(x => x.Levels)?.First()?.Name;
                return minLevel;
            }

            return null;
        }

        /// <summary>
        /// Change le niveau de log des logging rules par rapport a un critere de recherche
        /// </summary>
        /// <param name="logLevel">le nouveau niveau de log</param>
        /// <param name="regex">le critere de recherche en regex</param>
        public void SetLogLevel(string logLevel, string regex)
        {
            var logLevelObj = LogLevel.FromString(logLevel);
            var rules = NLog.LogManager.Configuration.LoggingRules;
            Regex validator = new Regex(regex);
            foreach (var rule in rules.Where(x => validator.IsMatch(x.Targets[0].Name)))
            {
                if (!rule.IsLoggingEnabledForLevel(logLevelObj))
                {
                    rule.EnableLoggingForLevel(logLevelObj);
                }

                //disable les niveaux de log verbeux si on passe le niveau Info
                if (logLevelObj == LogLevel.FromString("Info"))
                {
                    rule.DisableLoggingForLevel(LogLevel.FromString("Debug"));
                    rule.DisableLoggingForLevel(LogLevel.FromString("Trace"));
                }
            }

            //Call to update existing Loggers created with GetLogger() or 
            //GetCurrentClassLogger()
            NLog.LogManager.ReconfigExistingLoggers();
        }
    }
}
