using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.ImportExport.Models.Log
{
    public class LogConfigurationModel
    {
        /// <summary>
        /// Niveau de log en String :
        /// Fatal, Error, Warn, Info, Debug, Trace
        /// </summary>
        public string LogLevel { get; set; }

        /// <summary>
        /// Critere de recherche sur les type de logging rules
        /// </summary>
        public string RegexRules { get; set; }
    }
}
