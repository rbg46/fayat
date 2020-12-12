using System.Collections.Generic;
using Fred.Entities.Commande;

namespace Fred.Business.Commande
{
    /// <summary>
    /// Interface Gestionnaire des Types de Commande
    /// </summary>
    public interface ICommandeTypeManager : IManager<CommandeTypeEnt>
    {
        /// <summary>
        /// Récupération du type de commande par son identifiant
        /// </summary>
        /// <param name="commandeTypeId">Identifiant du type de commande</param>
        /// <returns>Entité type commande</returns>
        CommandeTypeEnt Get(int commandeTypeId);

        /// <summary>
        /// Récupération du type commande par son code
        /// </summary>
        /// <param name="code">Code du type commande</param>
        /// <returns>Entité type commande</returns>
        CommandeTypeEnt GetByCode(string code);

        /// <summary>
        /// Récupération de tous les types de commande
        /// </summary>
        /// <returns>Liste d'entité typeCommandeEnt</returns>
        List<CommandeTypeEnt> GetAll();
    }
}
