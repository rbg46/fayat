using Fred.Entities.Referential;
using Fred.ImportExport.Business.CI.ImportMaterielEtl.Input;
using Fred.ImportExport.Business.CI.ImportMaterielEtl.Output;
using Fred.ImportExport.Business.CI.ImportMaterielEtl.Result;
using Fred.ImportExport.Business.CI.ImportMaterielEtl.Transform;
using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.Framework.Etl.Engine;
using Fred.ImportExport.Framework.Etl.Engine.Builder;
using Fred.ImportExport.Models.Materiel;
using NLog;
using System;
using System.Diagnostics;

namespace Fred.ImportExport.Business.CI.ImportMaterielEtl.Process
{
  /// <summary>
  /// ETL d'import des CIs.
  /// </summary>
  public class ImportMaterielProcess : EtlProcessBase<ImportMaterielModel, MaterielEnt>
  {
    #region Properties

    private readonly EtlExecutionLogger etlExecutionLogger = new EtlExecutionLogger();

    private ImportMaterielModel materiel = new ImportMaterielModel();

    private readonly string logLocation = "[FLUX MATERIEL][IMPORT DANS FRED]";

    #endregion Properties

    /// <summary>
    /// Ctor  
    /// </summary>
    public ImportMaterielProcess()
    {

    }

    /// <summary>
    ///   Renseigne les paramètres d'input
    /// </summary>
    /// <param name="materiel">Un materiel</param>
    public void Init(ImportMaterielModel materiel)
    {
      this.materiel = materiel;
    }


    /// <inheritdoc/>
    protected override void OnBegin()
    {
      etlExecutionLogger.Log($"{logLocation} : INFO : Démarrage de l'import du materiel.");
    }

    public override void Build()
    {
      var builder = new EtlBuilder<ImportMaterielModel, MaterielEnt>(Config);
      builder
        .Input(new ImportMaterielInput(materiel, etlExecutionLogger))
        .Transform(new ImportMaterielTransform(etlExecutionLogger))
        .Result(new ImportMaterielResult())
        .Output(new ImportMaterielOutput(etlExecutionLogger));
    }



    /// <inheritdoc />
    protected override void OnSuccess()
    {
      var message = $"{logLocation} : SUCCESS : Insertion ou modification du matériel dans FRED réussie.";
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
      var error = $"{logLocation} : ERROR : L'insertion ou la modification du matériel dans FRED a échouée.";
      etlExecutionLogger.Log(error);

      var rapportExecution = etlExecutionLogger.Print();
      LogManager.GetCurrentClassLogger().Error(rapportExecution);
      Debug.WriteLine(rapportExecution);
    }


  }
}
