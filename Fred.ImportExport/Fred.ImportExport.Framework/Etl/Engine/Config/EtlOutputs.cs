using System.Collections.Generic;
using Fred.ImportExport.Framework.Etl.Output;

namespace Fred.ImportExport.Framework.Etl.Engine.Config
{

  /// <inheritdoc/>
  public class EtlOutputs<TR> : IEtlOutputs<TR>
  {

    /// <inheritdoc/>
    public IList<IEtlOutput<TR>> Items { get; } = new List<IEtlOutput<TR>>();
  }
}