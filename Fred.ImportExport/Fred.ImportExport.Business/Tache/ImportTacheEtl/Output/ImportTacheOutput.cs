using System;
using System.Threading.Tasks;
using CommonServiceLocator;
using Fred.Business.Referential.Tache;
using Fred.Entities.Referential;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;

namespace Fred.ImportExport.Business.CI.ImportTacheEtl.Output
{
    /// <summary>
    /// Processus etl : Execution de la sortie de l'import des Taches
    /// </summary>
    internal class ImportTacheOutput : IEtlOutput<TacheEnt>
    {
        private readonly string logLocation = "[FLUX TACHE][IMPORT DANS FRED][OUTPUT]";

        private readonly Lazy<ITacheManager> tacheManager = new Lazy<ITacheManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<ITacheManager>();
        });

        private readonly Business.Etl.Process.EtlExecutionLogger etlExecutionLogger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="etlExecutionLogger">Logger de l'exection de l'etl</param>
        public ImportTacheOutput(Business.Etl.Process.EtlExecutionLogger etlExecutionLogger)
        {
            this.etlExecutionLogger = etlExecutionLogger;
        }

        /// <summary>
        /// Appelé par l'ETL
        /// </summary>
        /// <param name="result">liste des fournisseurs à envoyer à Fred</param>
        public async Task ExecuteAsync(IEtlResult<TacheEnt> result)
        {
            await Task.Run(() =>
            {
                etlExecutionLogger.LogAndSerialize($"{logLocation} : INFO : Insertion dans Fred.", result.Items);

                foreach (var item in result.Items)
                {
                    tacheManager.Value.AddOrUpdateTache(item);
                }
            });
        }
    }
}
