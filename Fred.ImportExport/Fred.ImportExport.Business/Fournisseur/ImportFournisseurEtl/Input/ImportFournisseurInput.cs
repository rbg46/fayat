using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Models;
using System.Collections.Generic;

namespace Fred.ImportExport.Business.Fournisseur.Etl.Input
{

  /// <summary>
  /// Processus etl : Execution de l'input des Fournisseurs
  /// Va chercher dans Anael la liste des fournisseurs
  /// </summary>
  public class ImportFournisseurInput : IEtlInput<FournisseurModel>
  {
    /// <summary>
    /// Obtient ou définit la liste de founisseur.
    /// </summary>
    public List<FournisseurModel> Fournisseurs { get; set; }

    /// <summary>
    ///   Obtient ou définit le Flux Fournisseur
    /// </summary>
    public FluxEnt Flux { get; set; }

    /// <summary>
    /// Obtient ou définit la liste de founisseur.
    /// </summary>
    public IList<FournisseurModel> Items { get; set; }

    /// <inheritdoc/>
    /// Appelé par l'ETL
    public void Execute()
    {
      Items = Fournisseurs;
    }

  }
}
