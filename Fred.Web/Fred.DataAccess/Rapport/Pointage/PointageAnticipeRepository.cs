using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Fred.EntityFramework;

namespace Fred.DataAccess.Rapport.Pointage
{
    /// <summary>
    ///   AddPointage
    ///   DAL pour les pointages
    /// </summary>
    public class PointageAnticipeRepository : PointageBaseRepository<PointageAnticipeEnt>, IPointageAnticipeRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="PointageAnticipeRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow">Unit of Work</param>
        public PointageAnticipeRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        ///   liste rapport
        /// </summary>
        /// <returns>Liste des Rapports.</returns>
        public IQueryable<PointageAnticipeEnt> GetDefaultQuery()
        {
            return Context.PointagesAnticipes.Include(o => o.Ci)
                          .Include(o => o.Ci.Organisation)
                          .Include(o => o.Personnel.Societe)
                          .Include(o => o.Personnel.EtablissementPaie)
                          .Include(o => o.CodeAbsence)
                          .Include(o => o.CodeMajoration)
                          .Include(o => o.CodeDeplacement)
                          .Include(o => o.CodeZoneDeplacement)
                          .Include(o => o.ListPrimes).ThenInclude(ooo => ooo.Prime)
                          .Where(o => !o.DateSuppression.HasValue);
        }

        #region Get

        /// <summary>
        ///   Recherche la liste des pointages correspondants aux prédicats
        /// </summary>
        /// <param name="predicateWhere">Prédicat de filtrage des résultats</param>
        /// <param name="orderBy">Ordre de trie</param>
        /// <returns>IEnumerable contenant les rapports correspondants aux critères de recherche</returns>
        public IEnumerable<PointageAnticipeEnt> SearchPointageAnticipeWithFilter(Func<PointageAnticipeEnt, bool> predicateWhere, string orderBy)
        {
            var queryablePointages = GetDefaultQuery();
            List<PointageAnticipeEnt> pointages = queryablePointages.Where(predicateWhere).AsQueryable().OrderBy(orderBy).ToList();

            return pointages;
        }

        #endregion

        /// <summary>
        ///   Retourne une liste de pointages anticipés en fonction du personnel et d'une date
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="date">La date du pointage</param>
        /// <returns>Une liste de pointages anticipés</returns>
        public IEnumerable<PointageAnticipeEnt> GetListPointagesAnticipesByPersonnelIdAndDatePointage(int personnelId, DateTime date)
        {
            return GetDefaultQuery().Where(o => o.PersonnelId == personnelId && o.DatePointage == date).AsNoTracking();
        }

        /// <summary>
        ///   Retourne une liste de pointages anticipés pour un personnel dans un exercice paie
        /// </summary>
        /// <param name="personnel">Le personnel</param>
        /// <returns>Le une liste de pointages</returns>
        public IEnumerable<PointageAnticipeEnt> GetListPointagesAnticipesInExerciceByPersonnel(PersonnelEnt personnel)
        {
            return
              GetDefaultQuery()
                .Where(o => o.PersonnelId == personnel.PersonnelId)
                .Where(o => o.DatePointage >= new DateTime(DateTime.Now.Year, personnel.Societe.MoisDebutExercice.Value, 1)
                            &&
                            o.DatePointage <=
                            new DateTime(DateTime.Now.Year, personnel.Societe.MoisFinExercice.Value, 1).AddMonths(1).AddDays(-1));
        }

        /// <summary>
        ///   Récupère la liste des Rapport Ligne Reel par User
        /// </summary>
        /// <param name="userid">identifiant de l'utilisateur (GSP)</param>
        /// <returns>Liste des RapportLigne reelles</returns>
        public IEnumerable<int> GetAllPersonnelAnticipebyUser(int userid)
        {
            var qryPointagesAnticipes = GetDefaultQuery();
            var personnelsAnticipes = qryPointagesAnticipes
              .Where(p => p.AuteurCreationId != null && (int)p.AuteurCreationId == userid)
              .Where(p => p.PersonnelId != null)
              .Select(p => (int)p.PersonnelId)
              .Distinct();

            return personnelsAnticipes;
        }
    }
}