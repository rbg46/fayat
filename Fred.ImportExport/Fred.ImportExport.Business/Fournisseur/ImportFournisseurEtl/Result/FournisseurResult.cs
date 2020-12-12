using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Models;
using System.Collections.Generic;

namespace Fred.ImportExport.Business.Fournisseur.Etl.Result
{
  /// <inheritdoc/>
  public class FournisseurResult : IEtlResult<FournisseurModel>
  {
    /// <inheritdoc/>
    public IList<FournisseurModel> Items { get; set; } = new List<FournisseurModel>();
  }
}