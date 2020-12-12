using Fred.Entities.Moyen;
using Fred.Entities.Referential;
using Fred.ImportExport.Business.CI.ImportMoyenEtl.Input;
using Fred.ImportExport.Business.CI.ImportMoyenEtl.Output;
using Fred.ImportExport.Business.CI.ImportMoyenEtl.Result;
using Fred.ImportExport.Business.CI.ImportMoyenEtl.Transform;
using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.Framework.Etl.Engine;
using Fred.ImportExport.Framework.Etl.Engine.Builder;
using Fred.ImportExport.Models.Moyen;
using NLog;
using System;
using System.Diagnostics;

namespace Fred.ImportExport.Business.CI.ImportMoyenEtl.Process
{
    /// <summary>
    /// ETL d'import des moyens.
    /// </summary>
    public class ImportMoyenProcess : EtlProcessBase<MoyenModel, MaterielEnt>
    {
        #region Properties

        private readonly EtlExecutionLogger etlExecutionLogger = new EtlExecutionLogger();

        private MoyenModel moyen = new MoyenModel();

        private readonly string logLocation = "[FLUX Moyen][IMPORT DANS FRED]";

        #endregion Properties

        /// <summary>
        /// Ctor  
        /// </summary>
        public ImportMoyenProcess()
        {

        }

        /// <summary>
        ///   Renseigne les paramètres d'input
        /// </summary>
        /// <param name="moyen">Un Moyen</param>
        public void Init(MoyenModel moyen)
        {
            this.moyen = moyen;
        }


        /// <inheritdoc/>
        protected override void OnBegin()
        {
            etlExecutionLogger.Log($"{logLocation} : INFO : Démarrage de l'import du moyen.");
        }

        public override void Build()
        {
            var builder = new EtlBuilder<MoyenModel, MaterielEnt>(Config);
            builder
              .Input(new ImportMoyenInput(moyen, etlExecutionLogger))
              .Transform(new ImportMoyenTransform(etlExecutionLogger))
              .Result(new ImportMoyenResult())
              .Output(new ImportMoyenOutput(etlExecutionLogger));
        }

        /// <inheritdoc />
        protected override void OnSuccess()
        {
            var message = $"{logLocation} : SUCCESS : Insertion ou modification du moyen dans FRED réussie.";
            etlExecutionLogger.Log(message);
            var rapportExecution = etlExecutionLogger.Print();
            LogManager.GetCurrentClassLogger().Info(rapportExecution);
            Debug.WriteLine(rapportExecution);
        }

        /// <inheritdoc />
        protected override void OnError(Exception ex)
        {
            var errorMessage = $"{logLocation} : ERROR  " + ex.Message;
            etlExecutionLogger.Log(errorMessage);
            var error = $"{logLocation} : ERROR : L'insertion ou la modification du moyen dans FRED a échouée.";
            etlExecutionLogger.Log(error);

            var rapportExecution = etlExecutionLogger.Print();
            LogManager.GetCurrentClassLogger().Error(rapportExecution);
            Debug.WriteLine(rapportExecution);
        }


    }
}
