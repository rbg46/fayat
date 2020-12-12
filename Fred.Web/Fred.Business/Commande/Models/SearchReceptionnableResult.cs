using Fred.Entities.Commande;
using System.Collections.Generic;

namespace Fred.Business
{
  /// <summary>
  /// Classe representant le resultat d'une recheche de commande receptionnable
  /// </summary>
  public class SearchReceptionnableResult
  {
    /// <summary>
    /// Liste de commandes en fonction du paging
    /// </summary>
    public List<CommandeEnt> Commandes { get; internal set; }

    /// <summary>
    /// Nombre total d'element sans le paging
    /// </summary>
    public int TotalCount { get; internal set; }
  }
}