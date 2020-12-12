using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Models.Materiel;
using System.Collections.Generic;

namespace Fred.ImportExport.Business.CI.ImportMaterielEtl.Input
{
  /// <summary>
  /// Processus etl : Execution de l'input des Materiels
  /// Passe plat
  /// </summary>
  public class ImportMaterielInput : IEtlInput<ImportMaterielModel>
  {
    private readonly string logLocation = "[FLUX MATERIEL][IMPORT DANS FRED][INPUT]";

    private readonly EtlExecutionLogger etlExecutionLogger;

    public ImportMaterielInput(ImportMaterielModel materiel, EtlExecutionLogger etlExecutionLogger)
    {
      this.Materiel = materiel;
      this.etlExecutionLogger = etlExecutionLogger;
    }

    /// <summary>
    /// Obtient ou définit la liste de CI.
    /// </summary>
    public ImportMaterielModel Materiel { get; set; }

    /// <summary>
    /// Contient le resultat de l'importation Anael
    /// </summary>
    public IList<ImportMaterielModel> Items { get; set; }

    /// <inheritdoc/>
    /// Appelé par l'ETL
    public void Execute()
    {
      Items = new List<ImportMaterielModel> { Materiel };
      etlExecutionLogger.LogAndSerialize($"{logLocation} : INFO : Recuperation du json.", Items);
    }
  }
}
