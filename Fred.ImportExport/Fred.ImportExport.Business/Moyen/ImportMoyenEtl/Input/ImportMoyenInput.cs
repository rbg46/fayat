﻿using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Models.Moyen;
using System.Collections.Generic;

namespace Fred.ImportExport.Business.CI.ImportMoyenEtl.Input
{
    /// <summary>
    /// Processus etl : Execution de l'input des Moyens
    /// Passe plat
    /// </summary>
    public class ImportMoyenInput : IEtlInput<MoyenModel>
    {
        private readonly string logLocation = "[FLUX Moyen][IMPORT DANS FRED][INPUT]";

        private readonly EtlExecutionLogger etlExecutionLogger;

        public ImportMoyenInput(MoyenModel moyen, EtlExecutionLogger etlExecutionLogger)
        {
            Moyen = moyen;
            this.etlExecutionLogger = etlExecutionLogger;
        }

        /// <summary>
        /// Obtient ou définit la liste de CI.
        /// </summary>
        public MoyenModel Moyen { get; set; }

        /// <summary>
        /// Contient le resultat de l'importation Anael
        /// </summary>
        public IList<MoyenModel> Items { get; set; }

        /// <inheritdoc/>
        /// Appelé par l'ETL
        public void Execute()
        {
            Items = new List<MoyenModel> { Moyen };
            etlExecutionLogger.LogAndSerialize($"{logLocation} : INFO : Recuperation du json.", Items);
        }
    }
}
