using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Societe.Classification;
using Fred.EntityFramework;

namespace Fred.DataAccess.Societe.Classification
{
    /// <summary>
    /// Repository Classification 
    /// </summary>
    public class SocieteClassificationRepository : FredRepository<SocieteClassificationEnt>, ISocieteClassificationRepository
    {
        public SocieteClassificationRepository(FredDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Retourner la requête de récupération des classifications sociétés par groupe
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <param name="onlyActive">flag pour avoir seulement les actifs</param>
        /// <returns>Une liste classifications sociétés.</returns>
        public IEnumerable<SocieteClassificationEnt> GetByGroupeId(int groupeId, bool? onlyActive)
        {
            var filters = new List<Expression<Func<SocieteClassificationEnt, bool>>>
            {
                c => c.GroupeId == groupeId
            };
            if (onlyActive.HasValue && onlyActive.Value)
            {
                filters.Add(c => c.Statut);
            }
            return Get(filters).Select(PopulateSocietes);
        }

        /// <summary>
        /// Retourner la requête de récupération des classifications sociétés active
        /// </summary>
        /// <returns>Une liste classifications sociétés actives</returns>
        public IEnumerable<SocieteClassificationEnt> GetOnlyActive()
        {
            var filters = new List<Expression<Func<SocieteClassificationEnt, bool>>>
            {
                c => c.Statut
            };
            return Get(filters).Select(PopulateSocietes);
        }

        /// <summary>
        ///   Retourner la requête de récupération des classifications sociétés.
        /// </summary>
        /// <param name="searchText">Le filtre</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <returns>Une liste classifications sociétés.</returns>
        public IEnumerable<SocieteClassificationEnt> Search(string searchText, int? page = null, int? pageSize = null)
        {
            var filters = new List<Expression<Func<SocieteClassificationEnt, bool>>>
            {
                f => f.CodeLibelle.Contains(searchText)
            };
            return Get(filters, null, null, page, pageSize).Select(PopulateSocietes);
        }

        /// <summary>
        /// Permet de joindre la liste des sociétés avec les classifications
        /// </summary>
        /// <param name="classification">classifciations societes</param>
        /// <returns>une classification join societes</returns>
        public SocieteClassificationEnt PopulateSocietes(SocieteClassificationEnt classification)
        {
            classification.Societes = Context.Societes.Where(s => classification.SocieteClassificationId == s.SocieteClassificationId).ToList();

            return classification;
        }
    }
}
