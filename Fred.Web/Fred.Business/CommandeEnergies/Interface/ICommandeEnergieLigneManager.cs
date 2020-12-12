using System.Collections.Generic;
using Fred.Business.Commande;
using Fred.Entities.Commande;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Interface Gestionnaire des Lignes de Commandes Energie
    /// </summary>
    public interface ICommandeEnergieLigneManager : IManager<CommandeLigneEnt>
    {
        /// <summary>
        /// Ajout d'une liste de ligne de commande énergie
        /// </summary>
        /// <param name="commandeEnergieLignes">Liste de lignes de commande</param>
        /// <returns>Liste de lignes de commande énergie ajoutés</returns>
        List<CommandeEnergieLigne> AddRange(List<CommandeEnergieLigne> commandeEnergieLignes);

        /// <summary>
        /// Mise à jour d'une liste de ligne de commande énergie
        /// </summary>
        /// <param name="commandeEnergieLignes">Liste de lignes de commande</param>
        /// <returns>Liste de lignes de commande énergie mise à jour</returns>
        List<CommandeEnergieLigne> UpdateRange(List<CommandeEnergieLigne> commandeEnergieLignes);

        /// <summary>
        /// Suppression d'une liste de ligne de commande énergie
        /// </summary>
        /// <param name="commandeEnergieLigneIds">Liste d'identifiant de lignes de commande</param>        
        void DeleteRange(List<int> commandeEnergieLigneIds);
    }
}
