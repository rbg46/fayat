using System.Collections.Generic;
using Fred.Entities.Commande;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Repository des Reprises des données
    /// </summary>
    public interface IRepriseValidationCommandeRepository : IMultipleRepository
    {

        /// <summary>
        /// Marque les commande comme validées et sauvegarde en base
        /// </summary>
        /// <param name="commandes">Les commande a validées</param>
        void SetCommandesAsValidatedAndSaveCommandes(List<CommandeEnt> commandes);
        /// <summary>
        /// Met a jour l'HangfireId a jour
        /// </summary>
        /// <param name="commandes">Les commande a validées</param>
        void SetHangfireIdsAndSaveCommandes(List<CommandeEnt> commandes);
    }
}
