using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Result;

namespace Fred.ImportExport.Framework.Etl.Transform
{


  /// <summary>
  /// Représente un processus pour éxécuter le code métier
  /// </summary>
  /// <typeparam name="TI">Input</typeparam>
  /// <typeparam name="TR">Output</typeparam>
  public interface IEtlTransform<TI, TR>
  {

    /// <summary>
    /// Appelé par l'ETL
    /// Exécution du code métier de traitement
    /// </summary>
    /// <param name="input">Input</param>
    /// <param name="result">Result : Peut être null. L'instancier si besoin</param>
    void Execute(IEtlInput<TI> input, ref IEtlResult<TR> result);
  }


}