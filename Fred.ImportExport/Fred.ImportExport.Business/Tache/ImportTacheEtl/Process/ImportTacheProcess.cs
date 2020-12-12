using Fred.Entities.Referential;
using Fred.ImportExport.Business.CI.ImportTacheEtl.Input;
using Fred.ImportExport.Business.CI.ImportTacheEtl.Output;
using Fred.ImportExport.Business.CI.ImportTacheEtl.Result;
using Fred.ImportExport.Business.CI.ImportTacheEtl.Transform;
using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.Framework.Etl.Engine;
using Fred.ImportExport.Framework.Etl.Engine.Builder;
using Fred.ImportExport.Models.Tache;
using NLog;
using System;
using System.Diagnostics;

namespace Fred.ImportExport.Business.CI.ImportTacheEtl.Process
{
    /// <summary>
    /// ETL d'import des tâches.
    /// </summary>
    public class ImportTacheProcess : EtlProcessBase<TacheModel, TacheEnt>
    {
        #region Properties

        private readonly EtlExecutionLogger etlExecutionLogger = new EtlExecutionLogger();

        private TacheModel moyen = new TacheModel();

        private readonly string logLocation = "[FLUX Tache][IMPORT DANS FRED]";

        #endregion Properties

        /// <summary>
        /// Ctor  
        /// </summary>
        public ImportTacheProcess()
        {

        }

        /// <summary>
        ///   Renseigne les paramètres d'input
        /// </summary>
        /// <param name="moyen">Un Tache</param>
        public void Init(TacheModel moyen)
        {
            this.moyen = moyen;
        }


        /// <inheritdoc/>
        protected override void OnBegin()
        {
            etlExecutionLogger.Log($"{logLocation} : INFO : Démarrage de l'import d'une tâche.");
        }

        public override void Build()
        {
            var builder = new EtlBuilder<TacheModel, TacheEnt>(Config);
            builder
              .Input(new ImportTacheInput(moyen, etlExecutionLogger))
              .Transform(new ImportTacheTransform(etlExecutionLogger))
              .Result(new ImportTacheResult())
              .Output(new ImportTacheOutput(etlExecutionLogger));
        }

        /// <inheritdoc />
        protected override void OnSuccess()
        {
            var message = $"{logLocation} : SUCCESS : Insertion ou modification d'une tâche dans FRED réussie.";
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
            var error = $"{logLocation} : ERROR : L'insertion ou la modification d'une tâche dans FRED a échouée.";
            etlExecutionLogger.Log(error);

            var rapportExecution = etlExecutionLogger.Print();
            LogManager.GetCurrentClassLogger().Error(rapportExecution);
            Debug.WriteLine(rapportExecution);
        }


    }
}
