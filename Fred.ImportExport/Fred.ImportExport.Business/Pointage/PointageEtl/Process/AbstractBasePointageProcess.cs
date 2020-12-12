using Fred.ImportExport.Business.Pointage.PointageEtl.Parameter;
using Fred.ImportExport.Business.Pointage.PointageEtl.Settings;
using Fred.ImportExport.Framework.Etl.Engine;
using Fred.ImportExport.Framework.Exceptions;
using NLog;
using System;
using System.Diagnostics;

namespace Fred.ImportExport.Business.Pointage.PointageEtl.Process
{
  /// <summary>
  /// ETL de base pour l'export des pointage personnel vers SAP
  /// </summary>
  /// <typeparam name="TI">Input Template</typeparam>  
  /// <typeparam name="TR">Result Template</typeparam>
  public abstract class AbstractBasePointageProcess<TI, TR> : EtlProcessBase<TI, TR>, IPointageProcess
  {
    private readonly string logLocation = "[ETL PROCESS]";

    public EtlExecutionHelper EtlExecutionHelper { get; set; } = new EtlExecutionHelper();

    public EtlPointageParameter Parameter { get; set; }

    public void Init(EtlPointageParameter parameter)
    {
      this.Parameter = parameter;
    }


    /// <inheritdoc/>
    protected override void OnBegin()
    {
      // Si l'import ne peut pas être exécuté, alors arrêt de l'etl
      string error = CanExecuteImport();
      if (!string.IsNullOrEmpty(error))
      {
        throw new FredIeEtlStopException(error);
      }
    }

    /// <inheritdoc />
    protected override void OnSuccess()
    {
      var message = $"{Parameter.LogPrefix} : {logLocation} : SUCCESS : Envoie du pointage personnel vers SAP à réussi (RapportId = {Parameter.RapportId})";
      EtlExecutionHelper.Log(message);
      LogManager.GetCurrentClassLogger().Info(message);
      Debug.WriteLine(EtlExecutionHelper.Print());
    }

    /// <inheritdoc />
    protected override void OnError(Exception ex)
    {
      var error = $"{Parameter.LogPrefix} : {logLocation} : ERROR : Envoie du pointage personnel vers SAP à échoué (RapportId = {Parameter.RapportId})";
      EtlExecutionHelper.Log(error);
      LogManager.GetCurrentClassLogger().Error(error);
      Debug.WriteLine(EtlExecutionHelper.Print());
    }

    /// <summary>
    /// Vérification du flux
    /// </summary>
    /// <returns>
    /// si la chaine est vide alors le flux peut être exécuté
    /// si la chaine n'est pas vide, retourne un msg d'erreur</returns>
    private string CanExecuteImport()
    {
      string msg = string.Empty;

      return msg;
    }


  }
}
