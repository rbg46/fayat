using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Fred.Entities.Avis;

namespace Fred.Business.Avis
{
    /// <summary>
    /// Manager des avis
    /// </summary>
    public interface IAvisManager : IManager<AvisEnt>
    {
        /// <summary>
        /// Ajouter un avis à une commande
        /// </summary>
        /// <param name="commandeId">Identifiant d'une commande</param>
        /// <param name="avis">Avis à ajouter</param>
        /// <returns>Avis ajouté</returns>
        AvisEnt AddAvisForCommande(int commandeId, AvisEnt avis);

        /// <summary>
        /// Ajouter un avis à un avenant d'une commande
        /// </summary>
        /// <param name="commandeAvenantId">Identifiant de l'avenant</param>
        /// <param name="avis">Avis à ajouter</param>
        /// <returns>Avis ajouté</returns>
        AvisEnt AddAvisForCommandeAvenant(int? commandeAvenantId, AvisEnt avis);

        /// <summary>
        /// Récupérer l'historique des avis sur une validation de commande
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Historique des avis</returns>
        List<AvisEnt> GetHistoriqueAvisForCommande(int commandeId);

        /// <summary>
        /// Récupérer l'historique des avis sur une validation d'un avenat sur une commande
        /// </summary>
        /// <param name="commandeAvenantId">Identifiant de l'avenant sur une commande</param>
        /// <returns>Historique des avis</returns>
        List<AvisEnt> GetHistoriqueAvisForCommandeAvenant(int commandeAvenantId);
    }
}
