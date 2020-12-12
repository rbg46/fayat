using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Models;
using System.Collections.Generic;

namespace Fred.ImportExport.Business.Fournisseur.Etl.Result
{
  /// <inheritdoc/>
  public class ImportFournisseurResult : IEtlResult<FournisseurFredModel>
  {
    /// <inheritdoc/>
    public IList<FournisseurFredModel> Items { get; set; } = new List<FournisseurFredModel>();
  }
}