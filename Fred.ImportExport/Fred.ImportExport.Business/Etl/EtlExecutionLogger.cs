using System;
using System.Text;
using Newtonsoft.Json;
using NLog;

namespace Fred.ImportExport.Business.Etl.Process
{
    public class EtlExecutionLogger
    {
        private readonly string logText = "[LOG]";
        private readonly string warnText = "[WARN]";
        private readonly string errorText = "[ERROR]";

        private readonly StringBuilder logs;

        public EtlExecutionLogger()
        {
            logs = new StringBuilder();
        }

        /// <summary>
        /// Log une information
        /// </summary>
        /// <param name="log">message log</param>
        public void Log(string log)
        {
            logs.AppendLine(logText + log);
        }

        /// <summary>
        /// Warn une information
        /// </summary>
        /// <param name="warn">warn message </param>
        public void Warn(string warn)
        {
            logs.AppendLine(warnText + warn);
        }

        /// <summary>
        /// Log une erreur
        /// </summary>
        /// <param name="error">erreur </param>
        public void Error(string error)
        {
            logs.AppendLine(errorText + error);
        }

        /// <summary>
        ///  Imprime le rapport des logs et serialize l'object dataToSerialize
        /// </summary>log
        /// <param name="log">message log</param>
        /// <param name="dataToSerialize">object a serialisé</param>
        public void LogAndSerialize(string log, object dataToSerialize)
        {
            try
            {
                logs.AppendLine(logText + log);
                string dataSerializde = JsonConvert.SerializeObject(dataToSerialize, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                logs.AppendLine(dataSerializde);
            }
            catch
            {

                // je ne fais rien car un log ne doit pas provoquer d'exception
            }
        }

        /// <summary>
        /// Imprime le rapport des logs
        /// </summary>
        /// <returns>Le rapport des logs</returns>
        public string Print()
        {
            return logs.ToString();
        }

        /// <summary>
        /// Logs toutes les lignes du rapport d'execution
        /// </summary>
        /// <returns>le rapport d'execution</returns>
        public string LogsAllLines()
        {
            var rapportExecution = logs.ToString();
            string[] lines = rapportExecution.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                if (line.StartsWith(logText))
                {
                    var newLine = line.Substring(logText.Length);
                    LogManager.GetCurrentClassLogger().Info(newLine);
                }
                if (line.StartsWith(warnText))
                {
                    var newLine = line.Substring(warnText.Length);
                    LogManager.GetCurrentClassLogger().Warn(newLine);
                }
                if (line.StartsWith(errorText))
                {
                    var newLine = line.Substring(errorText.Length);
                    LogManager.GetCurrentClassLogger().Error(newLine);
                }
            }
            return logs.ToString();
        }
    }
}
