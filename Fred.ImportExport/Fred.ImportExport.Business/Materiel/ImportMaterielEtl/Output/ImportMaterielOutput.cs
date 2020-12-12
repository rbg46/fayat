using System;
using System.Threading.Tasks;
using CommonServiceLocator;
using Fred.Business.Referential.Materiel;
using Fred.Entities.Referential;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;

namespace Fred.ImportExport.Business.CI.ImportMaterielEtl.Output
{
    /// <summary>
    /// Processus etl : Execution de la sortie de l'import des Materiels
    /// </summary>
    internal class ImportMaterielOutput : IEtlOutput<MaterielEnt>
    {
        private readonly string logLocation = "[FLUX MATERIEL][IMPORT DANS FRED][OUTPUT]";

        private readonly Lazy<IMaterielManager> materielManager = new Lazy<IMaterielManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IMaterielManager>();
        });

        private readonly Business.Etl.Process.EtlExecutionLogger etlExecutionLogger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="materielMgr">Le gestionnaire des materiels</param>
        /// <param name="mapper">Le gestionnaire Automapper.</param>
        /// <param name="etlExecutionLogger">Logger de l'exection de l'etl</param>
        public ImportMaterielOutput(Business.Etl.Process.EtlExecutionLogger etlExecutionLogger)
        {
            this.etlExecutionLogger = etlExecutionLogger;
        }

        /// <summary>
        /// Appelé par l'ETL
        /// </summary>
        /// <param name="result">liste des fournisseurs à envoyer à Fred</param>
        public async Task ExecuteAsync(IEtlResult<MaterielEnt> result)
        {
            await Task.Run(() =>
            {
                etlExecutionLogger.LogAndSerialize($"{logLocation} : INFO : Insertion dans Fred.", result.Items);
                // Ici je met a jour ou je creer en fonction d'une expression sur le code et sur la societeid
                materielManager.Value.InsertOrUpdate(m => new { m.Code, m.SocieteId }, result.Items);
            });
        }
    }
}
