using System.Collections.Generic;

namespace Fred.ImportExport.Framework.Etl.Result
{

  /// <summary>
  /// Représente une classe qui contient le résultat des transformations
  /// </summary>
  /// <typeparam name="TR">Type des données</typeparam>
  public interface IEtlResult<TR>
  {

    /// <summary>
    /// Type de données contenant le résultat des transformations
    /// </summary>
    IList<TR> Items { get; set; }
  }
}