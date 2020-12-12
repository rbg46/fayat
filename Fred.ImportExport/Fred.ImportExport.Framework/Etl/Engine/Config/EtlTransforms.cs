using System.Collections.Generic;
using Fred.ImportExport.Framework.Etl.Transform;

namespace Fred.ImportExport.Framework.Etl.Engine.Config
{

  /// <inheritdoc/>
  public class EtlTransforms<TI, TR> : IEtlTransforms<TI, TR>
  {

    /// <inheritdoc/>
    public IList<IEtlTransform<TI, TR>> Items { get; } = new List<IEtlTransform<TI, TR>>();
  }
}