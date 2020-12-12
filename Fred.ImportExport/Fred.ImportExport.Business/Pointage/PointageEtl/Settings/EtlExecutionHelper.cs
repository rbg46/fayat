using Newtonsoft.Json;
using System.Text;

namespace Fred.ImportExport.Business.Pointage.PointageEtl.Settings
{
  /// <summary>
  /// Cette classe permet de logger tous les evenements pendant l'execution de l'etl
  /// Cela permettra de logger un rapport de l'execution de l'etl.
  /// </summary>
  public class EtlExecutionHelper
  {
    private readonly StringBuilder logs;

    public EtlExecutionHelper()
    {
      logs = new StringBuilder();
    }

    /// <summary>
    /// Log une information
    /// </summary>
    /// <param name="log">message log</param>
    public void Log(string log)
    {
      logs.AppendLine(log);
    }

    /// <summary>
    ///  Imprime le rapport des logs et serialize l'object dataToSerialize
    /// </summary>log
    /// <param name="log">message log</param>
    /// <param name="dataToSerialize">object a serialisé</param>
    public void LogAndSerialize(string log, object dataToSerialize)
    {
      try
      {
        logs.AppendLine(log);
        string dataSerializde = JsonConvert.SerializeObject(dataToSerialize, Formatting.Indented,
        new JsonSerializerSettings()
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        logs.AppendLine(dataSerializde);
      }
      catch
      {

        // je ne fais rien car un log ne doit pas provoquer d'exception
      }
    }


    /// <summary>
    /// Imprime le rapport des logs
    /// </summary>
    /// <returns>Le rapport des logs</returns>
    public string Print()
    {
      return logs.ToString();
    }

  }
}
