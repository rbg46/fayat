using System;
using System.Diagnostics;
using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.Business.Moyen.ExportPointageMoyenEtl.Input;
using Fred.ImportExport.DataAccess.ExternalService;
using Fred.ImportExport.DataAccess.ExternalService.ExportMoyenPointageProxy;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.Moyen;
using Fred.ImportExport.Framework.Etl.Engine;
using Fred.ImportExport.Framework.Etl.Engine.Builder;
using Fred.ImportExport.Models.Moyen;
using NLog;

namespace Fred.ImportExport.Business.Moyen.ExportPointageMoyenEtl.Transform
{
    /// <summary>
    /// Processus etl : Transformation du resultat.
    /// </summary>
    public class ExportPointageMoyenProcess : EtlProcessBase<ExportPointageMoyenModel, GestionMoyenInRecord>
    {
        private readonly string logLocation = "[FLUX Moyen][Export du pointage des moyens]";

        private readonly EtlExecutionLogger etlExecutionLogger = new EtlExecutionLogger();

        /// <summary>
        /// Constructeur de l'export pointage moyen process
        /// </summary>
        public ExportPointageMoyenProcess() { }

        /// <summary>
        /// Résultat du traitement
        /// </summary>
        public EnvoiPointageMoyenResult PointageMoyenResult { get; set; }

        
        private ExportPointageMoyenModel ExportpointageModel { get; set; }

        /// <summary>
        /// Initialisation de l'export des pointages process
        /// </summary>
        /// <param name="exportPointageModel">Export pointage model</param>
        public void Init(ExportPointageMoyenModel exportPointageModel)
        {
            ExportpointageModel = exportPointageModel;
        }

        /// <summary>
        /// Démarrage du process
        /// </summary>
        protected override void OnBegin()
        {
            etlExecutionLogger.Log($"{logLocation} : INFO : Démarrage de l'export du pointage des moyens");
        }

        /// <summary>
        /// La méthode qui construit le processing
        /// </summary>
        public override void Build()
        {
            EtlBuilder<ExportPointageMoyenModel, GestionMoyenInRecord> builder = new EtlBuilder<ExportPointageMoyenModel, GestionMoyenInRecord>(Config);
            builder
              .Input(new ExportPointageMoyenInput(ExportpointageModel, etlExecutionLogger))
              .Transform(new ExportPointageMoyenTransform(etlExecutionLogger))
              .Result(new ExportPointageMoyenResult())
              .Output(new ExportPointageMoyenOutput(etlExecutionLogger));
        }

        protected override void OnError(Exception ex)
        {
            var errorMessage = $"{logLocation} : ERROR  " + ex.Message;
            etlExecutionLogger.Log(errorMessage);
            var error = $"{logLocation} : ERROR : L'insertion du pointage des moyens a échoué";
            etlExecutionLogger.Log(error);

            var rapportExecution = etlExecutionLogger.Print();
            LogManager.GetCurrentClassLogger().Error(rapportExecution);
            Debug.WriteLine(rapportExecution);
            UpdatePointageMoyenResult(errorMessage);
        }

        protected override void OnSuccess()
        {
            string message = $"{logLocation} : SUCCESS : Insertion du pointage des moyens réussie.";
            etlExecutionLogger.Log(message);
            string rapportExecution = etlExecutionLogger.Print();
            LogManager.GetCurrentClassLogger().Info(rapportExecution);
            Debug.WriteLine(rapportExecution);
            UpdatePointageMoyenResult();
        }

        /// <summary>
        /// Update pointage moyen result
        /// </summary>
        /// <param name="errorMessage">Message d'erreur, s'il n'existe pas alors l'opération est un succés</param>
        private void UpdatePointageMoyenResult(string errorMessage = null)
        {
            ExportPointageMoyenOutput output = Config.OutPuts.Items[0] as ExportPointageMoyenOutput;
            if (output != null)
            {
                PointageMoyenResult = output.PointageMoyenResult;
            }
            else
            {
                PointageMoyenResult = new EnvoiPointageMoyenResult(Constantes.TibcoRetourErrorCode, 
                    string.IsNullOrEmpty(errorMessage) ? 
                    "Error export pointage moyen" : errorMessage);
            }
        }
    }
}
