
using Fred.Entities.Commande;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Représente un référentiel de données pour commandes des contrats intérimaires.
  /// </summary>
  public interface ICommandeContratInterimaireRepository : IRepository<CommandeContratInterimaireEnt>
  {
    /// <summary>
    /// Permet de savoir s'il existe une commande contrat intérimaire en fonction de la commande Id, du contrat Id et du ci Id
    /// </summary>
    /// <param name="contratId">identifiant unique du contrat intérimaire</param>
    /// <returns>La délégation</returns>
    CommandeContratInterimaireEnt GetCommandeContratInterimaireExist(int contratId);

    /// <summary>
    /// Permet de récupérer une commande contrat intérimaire en fonction du contrat Id et du ci Id
    /// </summary>
    /// <param name="contratId">identifiant unique du contrat intérimaire</param>
    /// <param name="ciId">identifiant unique du CI</param>
    /// <returns>La délégation</returns>
    CommandeContratInterimaireEnt GetCommandeContratInterimaire(int contratId, int ciId);

    /// <summary>
    /// Permet de récupérer une commande contrat intérimaire en fonction du commandeId
    /// </summary>
    /// <param name="commandeId">identifiant unique d'une commande</param>
    /// <returns>La CommandeContratInterimaire</returns>
    CommandeContratInterimaireEnt GetCommandeContratInterimaireByCommandeId(int commandeId);

    /// <summary>
    /// Permet d'ajouter une commande contrat intérimaire
    /// </summary>
    /// <param name="commandeContratInterimaireEnt">commande contrat intérimaire</param>
    /// <returns>La commande contrat intérimaire enregistrée</returns>
    CommandeContratInterimaireEnt AddCommandeContratInterimaire(CommandeContratInterimaireEnt commandeContratInterimaireEnt);

  }
}
