using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Models.Tache;
using System.Collections.Generic;

namespace Fred.ImportExport.Business.CI.ImportTacheEtl.Input
{
    /// <summary>
    /// Processus etl : Execution de l'input des Taches
    /// Passe plat
    /// </summary>
    public class ImportTacheInput : IEtlInput<TacheModel>
    {
        private readonly string logLocation = "[FLUX Tache][IMPORT DANS FRED][INPUT]";

        private readonly EtlExecutionLogger etlExecutionLogger;

        public ImportTacheInput(TacheModel moyen, EtlExecutionLogger etlExecutionLogger)
        {
            Tache = moyen;
            this.etlExecutionLogger = etlExecutionLogger;
        }

        /// <summary>
        /// Obtient ou définit la liste de CI.
        /// </summary>
        public TacheModel Tache { get; set; }

        /// <summary>
        /// Contient le resultat de l'importation Anael
        /// </summary>
        public IList<TacheModel> Items { get; set; }

        /// <inheritdoc/>
        /// Appelé par l'ETL
        public void Execute()
        {
            Items = new List<TacheModel> { Tache };
            etlExecutionLogger.LogAndSerialize($"{logLocation} : INFO : Recuperation du json.", Items);
        }
    }
}
