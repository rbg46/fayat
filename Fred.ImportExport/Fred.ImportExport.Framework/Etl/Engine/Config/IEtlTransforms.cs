using System.Collections.Generic;
using Fred.ImportExport.Framework.Etl.Transform;

namespace Fred.ImportExport.Framework.Etl.Engine.Config
{

  /// <summary>
  /// Liste des processus de transformation
  /// </summary>
  /// <typeparam name="TI">Input</typeparam>
  /// <typeparam name="TR">Result</typeparam>
  public interface IEtlTransforms<TI, TR>
  {

    /// <summary>
    /// Liste des transformations a éxécuter dans l'ordre d'insertion
    /// </summary>
    IList<IEtlTransform<TI, TR>> Items { get; }
  }
}