using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Commande;
using Fred.Entities.CommandeLigne.QuantiteNegative;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Interface repository Lignes Commande
    /// </summary>
    public interface ICommandeLignesRepository : IRepository<CommandeLigneEnt>
    {
        /// <summary>
        ///   Retourne la ligne de commande portant l'identifiant unique du materiel externe.
        /// </summary>
        /// <param name="materielId">Identifiant du materiel externe.</param>
        /// <returns>La ligne de commande retrouvée, sinon nulle.</returns>
        CommandeLigneEnt GetCommandeLigneByMaterielId(int materielId);

        /// <summary>
        /// retourne une commande suivant l'id de la ligne de commande
        /// </summary>
        /// <param name="commandeLigneId">Id de la ligne de commande</param>
        /// <returns>une commande</returns>
        Task<CommandeLigneEnt> GetCommandeLigneByIdAsync(int commandeLigneId);

        /// <summary>
        /// retourne la commande ligne avec les dépenses qui lui sont liées
        /// </summary>
        /// <param name="commandeLigneId"></param>
        /// <returns></returns>
        CommandeLigneWithReceptionQuantiteModel GetCommandeLigneWithReceptionQuantiteById(int commandeLigneId);

        /// <summary>
        /// retourne les commandes lignes avec les dépenses qui leurs sont liées
        /// </summary>
        /// <param name="commandeLigneId"></param>
        /// <returns></returns>
        List<CommandeLigneWithReceptionQuantiteModel> GetCommandeLignesWithReceptionQuantiteByIds(List<int> commandeLigneIds);

        /// <summary>
        /// Vérrouille une commande ligne
        /// </summary>
        /// <param name="commandeLigneId"></param>
        void LockCommandeLigne(int commandeLigneId, int auteurModificationId);

        /// <summary>
        /// Dévérrouille une commande ligne
        /// </summary>
        /// <param name="commandeLigneId"></param>
        void UnlockCommandeLigne(int commandeLigneId, int auteurModificationId);

        List<CommandeLigneEnt> Get(List<Expression<Func<CommandeLigneEnt, bool>>> filters,
                                                       Func<IQueryable<CommandeLigneEnt>, IOrderedQueryable<CommandeLigneEnt>> orderBy = null,
                                                       List<Expression<Func<CommandeLigneEnt, object>>> includeProperties = null,
                                                       int? page = null,
                                                       int? pageSize = null,
                                                       bool asNoTracking = true);

        /// <summary>
        /// Ajout d'une ligne de commande
        /// </summary>
        /// <param name="ligne">ligne de la commande</param>
        void AddCommandeLigne(CommandeLigneEnt ligne);

        /// <summary>
        /// Update d'une ligne de commande
        /// </summary>
        /// <param name="ligne">ligne de la commande</param>
        void UpdateCommandeLigne(CommandeLigneEnt ligne);

        /// <summary>
        /// suppression d'une ligne de commande
        /// </summary>
        /// <param name="ligne">ligne de la commande</param>
        void DeleteCommandeLigne(CommandeLigneEnt ligne);

        CommandeLigneQuantiteNegativeModel GetCommandeLigneWithReceptionsQuantities(int commandeLigneId);

        List<CommandeLigneQuantiteNegativeModel> GetCommandeLignesWithReceptionsQuantities(List<int> commandesLigneIds);

    }
}
