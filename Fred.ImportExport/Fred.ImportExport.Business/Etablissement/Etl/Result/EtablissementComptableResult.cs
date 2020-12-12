using Fred.Entities.Referential;
using Fred.ImportExport.Framework.Etl.Result;
using System.Collections.Generic;

namespace Fred.ImportExport.Business.Etablissement.Etl.Result
{

  /// <inheritdoc/>
  public class EtablissementComptableResult : IEtlResult<EtablissementComptableEnt>
  {
    /// <inheritdoc/>
    public IList<EtablissementComptableEnt> Items { get; set; } = new List<EtablissementComptableEnt>();
  }
}