using Fred.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fred.Business.Societe
{
    /// <summary>
    /// Interface Manager Associe Sep
    /// </summary>
    public interface IAssocieSepManager : IManager<AssocieSepEnt>
    {
        /// <summary>
        ///  Récupération tous les  associés SEP
        /// </summary>
        /// <returns>Liste des associés SEP</returns>
        IQueryable<AssocieSepEnt> GetAll();

        /// <summary>
        ///     Récupération des associés SEP
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Liste des associés SEP</returns>
        IEnumerable<AssocieSepEnt> GetAll(int societeId);

        /// <summary>
        ///     Insertion d'une liste d'associés SEP
        /// </summary>        
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="associeSeps">Liste d'associés SEP</param>
        /// <returns>Nouvelle liste d'associés SEP</returns>
        List<AssocieSepEnt> CreateOrUpdateRange(int societeId, List<AssocieSepEnt> associeSeps);

        /// <summary>
        ///   Retourner la requête de récupération des associés SEP
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        List<AssocieSepEnt> Search(List<Expression<Func<AssocieSepEnt, bool>>> filters,
                                          Func<IQueryable<AssocieSepEnt>, IOrderedQueryable<AssocieSepEnt>> orderBy = null,
                                          List<Expression<Func<AssocieSepEnt, object>>> includeProperties = null,
                                          int? page = null,
                                          int? pageSize = null,
                                          bool asNoTracking = false);

        /// <summary>
        /// Suppression d'une liste d'associés SEP par leur identifiants        
        /// </summary>
        /// <param name="associeSepIds">Liste d'identifiants d'associés SEP</param>
        void DeleteRange(List<int> associeSepIds);
    }
}
