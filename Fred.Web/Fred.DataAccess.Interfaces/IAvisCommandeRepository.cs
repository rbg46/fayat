using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Avis;

namespace Fred.DataAccess.Avis
{
    /// <summary>
    /// Reposiory des avis sur les commandes
    /// </summary>
    public interface IAvisCommandeRepository : IFredRepository<AvisCommandeEnt>
    {
        /// <summary>
        /// Ajouter une relation avis - commande
        /// </summary>
        /// <param name="avisCommande">AvisCommande à ajouter</param>
        /// <returns>AvisCommande ajoutée (attachée)</returns>
        void Add(AvisCommandeEnt avisCommande);

        /// <summary>
        /// Récupérer l'historique des avis sur une validation de commande
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Historique des avis</returns>
        List<AvisEnt> GetAvisByCommandeId(int commandeId);
    }
}
