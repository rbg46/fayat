using Fred.Entities.Referential;
using Fred.ImportExport.Framework.Etl.Result;
using System.Collections.Generic;

namespace Fred.ImportExport.Business.CI.ImportMaterielEtl.Result
{
  /// <summary>
  /// Processus etl : Contiens le résultat des transformations.
  /// </summary>
  public class ImportMaterielResult : IEtlResult<MaterielEnt>
  {
    public IList<MaterielEnt> Items { get; set; } = new List<MaterielEnt>();
  }
}
