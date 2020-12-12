using System.Threading.Tasks;
using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.DataAccess.ExternalService.ExportMoyenPointageProxy;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.Moyen;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Exceptions;
using Fred.Web.Shared.Extentions;

namespace Fred.ImportExport.Business.Moyen.ExportPointageMoyenEtl.Input
{
    /// <summary>
    /// Export pointage moyen output : La classe outpu de l'export des pointages des moyens .
    /// </summary>
    public class ExportPointageMoyenOutput : IEtlOutput<GestionMoyenInRecord>
    {
        private readonly string logLocation = "[EXPORT POINTAGE MOYEN][OUTPUT]";

        /// <summary>
        /// Etl execution logger
        /// </summary>
        private readonly EtlExecutionLogger etlExecutionLogger;

        public ExportPointageMoyenOutput(EtlExecutionLogger etlExecutionLogger)
        {
            this.etlExecutionLogger = etlExecutionLogger;
        }

        /// <summary>
        /// Résultat du traitement
        /// </summary>
        public EnvoiPointageMoyenResult PointageMoyenResult { get; set; }

        /// <summary>
        /// Exécution de l'output => 
        /// </summary>
        /// <param name="result">Resultat de l'opération de transformation</param>
        public async Task ExecuteAsync(IEtlResult<GestionMoyenInRecord> result)
        {
            await Task.Run(() =>
            {
                etlExecutionLogger.LogAndSerialize($"{logLocation} : INFO : Envoi du pointage des moyens vers TIBCO.", result.Items);

                MoyenTibcoRepositoryExterne moyenRepoExterne = new MoyenTibcoRepositoryExterne();
                GestionMoyenInRecord[] records = result.Items as GestionMoyenInRecord[];
                if (records.IsNullOrEmpty())
                {
                    throw new FredIeBusinessException($"Aucun pointage à envoyer . Problème de conversion à un tableau de GestionMoyenInRecord");
                }

                PointageMoyenResult = moyenRepoExterne.SendPointageToTibco(records);
            });
        }
    }
}
