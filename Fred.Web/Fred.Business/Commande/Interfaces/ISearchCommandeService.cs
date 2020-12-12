using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.Personnel;
using Fred.Entities.Commande;
using Fred.Entities.Personnel;

namespace Fred.Business.Commande
{
    /// <summary>
    /// Interface Commande Serach Utility,ici en implimente tous les codes qui concernent les filtres pour la Commnande ENT
    /// </summary>
    public interface ISearchCommandeService : IService
    {
        /// <summary>
        ///   Retourner la requête de récupération des commandes
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        IEnumerable<CommandeEnt> Search(List<Expression<Func<CommandeEnt, bool>>> filters,
                                             Func<IQueryable<CommandeEnt>, IOrderedQueryable<CommandeEnt>> orderBy = null,
                                             List<Expression<Func<CommandeEnt, object>>> includeProperties = null,
                                             int? page = null,
                                             int? pageSize = null,
                                             bool asNoTracking = false);

        /// <summary>
        /// renvoie la liste des Auteurs suivant le filtre
        /// </summary>
        /// <param name="search">model de recherches</param>
        /// <returns>renvoie la liste des Auteurs</returns>
        List<PersonnelEnt> SearchCommandeAuthors(SearchLightPersonnelModel search);

        /// <summary>
        ///   Cherche une liste de commandes.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des commandes.</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>Une liste de commande.</returns>
        IEnumerable<CommandeEnt> SearchCommandes(string text, int? page, int? pageSize);

        /// <summary>
        ///   Retourne la liste des commandes filtrée, triée (avec pagination)
        /// </summary>
        /// <param name="filter">Objet de recherche et de tri des commandes</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>Retourne la liste des commandes filtrée, triée et paginée et le count total d'elements</returns>
        SearchCommandeListWithFilterResult SearchCommandeListWithFilter(SearchCommandeEnt filter, int? page, int? pageSize);

        SearchReceptionnableResult SearchReceivableOrders(SearchReceivableOrdersFilter filter, int page = 1, int pageSize = 20);
    }
}
