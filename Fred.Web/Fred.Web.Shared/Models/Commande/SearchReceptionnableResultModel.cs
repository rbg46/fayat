using Fred.Web.Models.Commande;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models
{

  /// <summary>
  /// Classe representant le resultat d'une recheche de commande receptionnable
  /// </summary>
  public class SearchReceptionnableResultModel
  {
    /// <summary>
    /// Liste de commandes en fonction du paging
    /// </summary>
    public List<CommandeModel> Commandes { get; internal set; }

    /// <summary>
    /// Nombre total d'element sans le paging
    /// </summary>
    public int TotalCount { get; internal set; }
  }
}
