using Fred.Web.Shared.Models.Commande.List;

namespace Fred.Web.Models.Commande
{
  /// <summary>
  /// Représente graphique d'une liste de commandes groupées par CI
  /// </summary>
  public class CommandeGroupByCIModel
  {
    /// <summary>
    /// Centre d'imputation de regroupement
    /// </summary>
    public CIForCommandeListModel CI { get; set; }

    /// <summary>
    /// Liste des commandes regroupées
    /// </summary>
    public CommandeListModel[] Commandes { get; set; }
  }
}
