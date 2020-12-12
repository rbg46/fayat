using System.Collections.Generic;
using Fred.ImportExport.Framework.Etl.Output;

namespace Fred.ImportExport.Framework.Etl.Engine.Config
{

  /// <summary>
  /// Liste des processus de sortie
  /// </summary>
  /// <typeparam name="TR">Result</typeparam>
  public interface IEtlOutputs<TR>
  {

    /// <summary>
    /// Liste des sorties a éxécuter dans l'ordre d'insertion
    /// </summary>
    IList<IEtlOutput<TR>> Items { get; }
  }
}