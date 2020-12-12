using Fred.ImportExport.Business.Pointage.PointageEtl.Transform;
using Fred.ImportExport.Framework.Etl.Result;
using System.Collections.Generic;

namespace Fred.ImportExport.Business.Pointage.PointageEtl.Result
{
  /// <summary>
  /// Processus etl : Contiens le résultat de la transformations.
  /// </summary>
  public class BasePointageResult : IEtlResult<ExportRapportLigneModel>
  {
    public IList<ExportRapportLigneModel> Items { get; set; } = new List<ExportRapportLigneModel>();
  }
}
