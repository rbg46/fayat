using System.Collections.Generic;
using Fred.Entities.Commande;

namespace Fred.Business.Commande
{
    /// <summary>
    /// Interface Gestionnaire des Statuts Commande
    /// </summary>
    public interface IStatutCommandeManager : IManager<StatutCommandeEnt>
    {
        /// <summary>
        /// Récupération du statut commande par son identifiant
        /// </summary>
        /// <param name="statutCommandeId">Identifiant du statut commande</param>
        /// <returns>Entité statut commande</returns>
        StatutCommandeEnt Get(int statutCommandeId);

        /// <summary>
        /// Récupération du statut commande par son code
        /// </summary>
        /// <param name="code">Code du statut commande</param>
        /// <returns>Entité statut commande</returns>
        StatutCommandeEnt GetByCode(string code);

        /// <summary>
        /// Récupération de tous les statuts de commande
        /// </summary>
        /// <returns>Liste d'entité StatutCommandeEnt</returns>
        List<StatutCommandeEnt> GetAll();
    }
}
