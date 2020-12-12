using System.Collections.Generic;
using Fred.Entities.Commande;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Représente un référentiel de données pour les avenants de commande.
    /// </summary>
    public interface ICommandeAvenantRepository : IRepository<CommandeAvenantEnt>
    {
        /// <summary>
        /// Liste des avenants d'une commande
        /// </summary>
        /// <param name="commandeId">commande Id</param>
        /// <returns>Liste des avenants</returns>
        IEnumerable<CommandeAvenantEnt> GetAvenantByCommandeId(int commandeId);

        /// <summary>
        /// Récupération de l'avenant d'une commande depuis son commandeId et son numeroAvenant
        /// </summary>
        /// <param name="commandeId">commandeId</param>
        /// <param name="numeroAvenant">numeroAvenant</param>
        /// <returns>CommandeAvenantEnt</returns>
        CommandeAvenantEnt GetAvenantByCommandeIdAndAvenantNumber(int commandeId, int numeroAvenant);


        /// <summary>
        /// Ajoute un avenant
        /// </summary>
        /// <param name="avenant">avenant</param>
        void AddAvenant(CommandeAvenantEnt avenant);


        /// <summary>
        /// Update d'un avenant
        /// </summary>
        /// <param name="avenant">avenant</param>
        void UpdateAvenant(CommandeAvenantEnt avenant);
    }
}
