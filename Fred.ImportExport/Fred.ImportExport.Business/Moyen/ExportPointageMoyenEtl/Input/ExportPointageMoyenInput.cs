using System.Collections.Generic;
using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Models.Moyen;

namespace Fred.ImportExport.Business.Moyen.ExportPointageMoyenEtl.Input
{
    public class ExportPointageMoyenInput : IEtlInput<ExportPointageMoyenModel>
    {
        private readonly string logLocation = "[FLUX Moyen][EXPORT VERS TIBCO][INPUT]";

        private readonly EtlExecutionLogger etlExecutionLogger;

        public ExportPointageMoyenInput(ExportPointageMoyenModel model, EtlExecutionLogger etlExecutionLogger)
        {
            Model = model;
            this.etlExecutionLogger = etlExecutionLogger;
        }

        /// <summary>
        /// Obtient ou définit le Model
        /// </summary>
        public ExportPointageMoyenModel Model { get; set; }

        /// <summary>
        /// Obtient ou définit une liste du model d'entrée
        /// </summary>
        public IList<ExportPointageMoyenModel> Items { get; set; }

        /// <inheritdoc/>
        /// Appelé par l'ETL
        public void Execute()
        {
            Items = new List<ExportPointageMoyenModel> { Model };
            etlExecutionLogger.LogAndSerialize($"{logLocation} : INFO : Export vers TIBCO .", Items);
        }
    }
}
