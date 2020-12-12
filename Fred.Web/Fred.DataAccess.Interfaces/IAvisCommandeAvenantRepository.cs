using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Avis;

namespace Fred.DataAccess.Avis
{
    /// <summary>
    /// Reposiory des avis sur les avenants des commandes
    /// </summary>
    public interface IAvisCommandeAvenantRepository : IFredRepository<AvisCommandeAvenantEnt>
    {
        /// <summary>
        /// Récupérer l'historique des avis sur une validation d'un avenat sur une commande
        /// </summary>
        /// <param name="commandeAvenantId">Identifiant de l'avenant sur une commande</param>
        /// <returns>Historique des avis</returns>
        List<AvisEnt> GetAvisByCommandeAvenantId(int commandeAvenantId);
    }
}
