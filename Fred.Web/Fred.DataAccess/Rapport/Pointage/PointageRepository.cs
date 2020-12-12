using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fred.DataAccess.Interfaces;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Web.Models.PointagePersonnel;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using static Fred.Entities.Constantes;

namespace Fred.DataAccess.Rapport.Pointage
{
    public class PointageRepository : PointageBaseRepository<RapportLigneEnt>, IPointageRepository
    {
        public PointageRepository(FredDbContext Context)
          : base(Context)
        {
        }

        /// <summary>
        /// Récupère un RapportLigne en fonction de son Identifiant
        /// </summary>
        /// <param name="rapportLigneId">Identifiant du RapportLigneId</param>
        /// <param name="includes">Includes lors du get</param>
        /// <returns>Un pointage</returns>
        public RapportLigneEnt Get(int rapportLigneId, List<Expression<Func<RapportLigneEnt, object>>> includes)
        {
            RapportLigneEnt result = this.Get(null, null, includes, null, null).FirstOrDefault(x => x.RapportLigneId == rapportLigneId);
            if (result != null && result.DateSuppression.HasValue)
            {
                return null;
            }
            return result;
        }

        /// <inheritdoc />
        public IEnumerable<RapportLigneEnt> GetRapportLigneAllSync()
        {
            return this.Context.RapportLignes.AsNoTracking();
        }

        /// <inheritdoc />
        public IEnumerable<RapportLignePrimeEnt> GetRapportLignePrimeAllSync()
        {
            return this.Context.RapportLignePrimes.AsNoTracking();
        }

        /// <inheritdoc />
        public IEnumerable<RapportLigneTacheEnt> GetRapportLigneTacheAllSync()
        {
            return this.Context.RapportLigneTaches.AsNoTracking();
        }

        /// <summary>
        /// Récupère l'entité RapportLigneTache correspondant au identifiants passés en paramètre
        /// </summary>
        /// <param name="rapportLigneTacheId">Identifiant de la liaison pointage/tâche</param>
        /// <returns>Retourne l'entité RapportLigneTache correspondant au identifiants passés en paramètre</returns>
        public RapportLigneTacheEnt GetRapportLigneTacheById(int rapportLigneTacheId)
        {
            return Context.RapportLigneTaches.AsNoTracking().FirstOrDefault(rlt => rlt.RapportLigneTacheId == rapportLigneTacheId);
        }

        /// <inheritdoc />
        public IEnumerable<RapportStatutEnt> GetRapportStatutAllSync()
        {
            return this.Context.RapportStatuts.AsNoTracking();
        }

        /// <inheritdoc />
        public IEnumerable<RapportTacheEnt> GetRapportTacheAllSync()
        {
            return this.Context.RapportTache.AsNoTracking();
        }

        /// <inheritdoc />
        public IEnumerable<RapportLigneEnt> GetAllLight(int rapportId)
        {
            return this.Context.RapportLignes.Where(x => x.RapportId == rapportId);
        }

        /// <summary>
        /// liste rapport
        /// </summary>
        /// <returns>Liste des Rapports.</returns>
        public IQueryable<RapportLigneEnt> GetDefaultQuery()
        {
            return Context.RapportLignes
                .Include(o => o.Ci.Organisation)
                .Include(o => o.Ci.EtablissementComptable.Societe)
                .Include(o => o.Ci.Societe)
                .Include(o => o.Personnel.Societe.Organisation)
                .Include(o => o.Personnel.Ressource)
                .Include(o => o.Personnel.EtablissementPaie)
                .Include(o => o.Personnel.EtablissementRattachement)
                .Include(o => o.CodeAbsence)
                .Include(o => o.CodeMajoration)
                .Include(o => o.ListRapportLigneMajorations).ThenInclude(x => x.CodeMajoration)
                .Include(o => o.CodeDeplacement).Include(o => o.CodeZoneDeplacement)
                .Include(o => o.Materiel)
                .Include(o => o.ListRapportLignePrimes).ThenInclude(ooo => ooo.Prime)
                .Include(o => o.ListRapportLigneTaches).ThenInclude(ooo => ooo.Tache)
                .Where(o => !o.DateSuppression.HasValue);
        }

        /// <summary>
        /// Récupère la liste des pointage réels d'un rapport
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport</param>
        /// <returns> Liste des pointage réels d'un rapport </returns>
        public IEnumerable<RapportLigneEnt> GetPointageReelByRapportId(int rapportId)
        {
            return GetDefaultQuery().Where(p => p.RapportId == rapportId).ToList();
        }

        /// <summary>
        /// Retourne un pointage sans Attachement au Contexte
        /// </summary>
        /// <param name="rapportLigneId">Identifiant du pointage</param>
        /// <returns>Pointage</returns>
        public RapportLigneEnt GetById(int rapportLigneId)
        {
            return Context.RapportLignes.AsNoTracking().First(p => p.RapportLigneId == rapportLigneId);
        }

        /// <summary>
        /// Recherche la liste des pointages correspondants aux prédicats
        /// </summary>
        /// <param name="predicateWhere">Prédicat de filtrage des résultats</param>
        /// <param name="orderBy">Ordre de tri</param>
        /// <returns>IEnumerable contenant les lignes de rapport correspondant aux critères de recherche</returns>
        public IEnumerable<RapportLigneEnt> SearchPointageWithFilter(Expression<Func<RapportLigneEnt, bool>> predicateWhere, string orderBy)
        {
            return Context.RapportLignes.Include(o => o.Ci.Organisation).Include(o => o.Ci.EtablissementComptable.Societe)
                          .Include(o => o.Ci.Societe).Include(o => o.Ci.Societe.Groupe).Include(o => o.Personnel.Societe).Include(o => o.Personnel.Ressource)
                          .Include(o => o.Personnel.EtablissementPaie).Include(o => o.Personnel.EtablissementRattachement)
                          .Include(o => o.CodeAbsence).Include(o => o.CodeMajoration)
                          .Include(o => o.ListRapportLigneMajorations).ThenInclude(x => x.CodeMajoration)
                          .Include(o => o.CodeDeplacement).Include(o => o.CodeZoneDeplacement)
                          .Include(o => o.Materiel).Include(o => o.ListRapportLignePrimes).ThenInclude(ooo => ooo.Prime)
                          .Include(o => o.ListRapportLigneTaches).ThenInclude(ooo => ooo.Tache)
                          .Include(o => o.Personnel.Manager).Include(o => o.ListCodePrimeAstreintes).ThenInclude(l => l.CodeAstreinte)
                          .Include(o => o.ListRapportLigneAstreintes).ThenInclude(l => l.ListCodePrimeSortiesAstreintes).ThenInclude(y => y.CodeAstreinte)
                          .Include(o => o.Personnel.Societe.Organisation)
                          .AsNoTracking()
                          .Where(o => !o.DateSuppression.HasValue)
                          .Where(predicateWhere)
                          .OrderBy(orderBy)
                          .ToList();
        }

        /// <summary>
        /// Indique si des pointages existent.
        /// </summary>
        /// <param name="predicates">Prédicats de filtrage des résultats.</param>
        /// <returns>True si des pointages existent, sinon false.</returns>
        public bool IsPointagesExists(params Expression<Func<RapportLigneEnt, bool>>[] predicates)
        {
            IQueryable<RapportLigneEnt> query = Get()
                .Where(o => !o.DateSuppression.HasValue)
                .WhereMulti(predicates);
            return query.Any();
        }

        /// <summary>
        /// Recherche la liste des pointages correspondants aux prédicats par page
        /// </summary>
        /// <param name="predicateWhere">Prédicat de filtrage des résultats</param>
        /// <param name="orderBy">Ordre de tri</param>
        /// <param name="page">Page actuelle</param>
        /// <param name="pageSize">Taille de page</param>
        /// <returns>IEnumerable contenant les lignes de rapport correspondant aux critères de recherche par page</returns>
        public IEnumerable<RapportLigneEnt> SearchPointageWithFilterByPage(Expression<Func<RapportLigneEnt, bool>> predicateWhere, string orderBy, int page, int pageSize)
        {
            return Get()
                        .Include(x => x.AffectationMoyen.Ci).Include(x => x.AffectationMoyen.Personnel).Include(x => x.Ci.Societe).Include(x => x.Ci.EtablissementComptable).Include(x => x.AffectationMoyen.Materiel.Ressource.SousChapitre.Chapitre).Include(x => x.AffectationMoyen.Materiel.SiteAppartenance).Include(x => x.AffectationMoyen.Materiel.Societe)
                        .Include(x => x.AffectationMoyen.Materiel.EtablissementComptable)
                        .Include(x => x.AffectationMoyen.Site)
                        .Include(x => x.AffectationMoyen.TypeAffectation)
                        .Where(predicateWhere)
                        .OrderBy(orderBy)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        /// <summary>
        /// Recherche la liste des pointages correspondants aux prédicats
        /// </summary>
        /// <param name="predicateWhere">Prédicat de filtrage des résultats</param>
        /// <param name="orderBy">Ordre de tri</param>
        /// <returns>IEnumerable contenant les lignes de rapport correspondant aux critères de recherche par page</returns>
        public List<RapportLigneEnt> SearchPointageReelWithMoyenFilter(Expression<Func<RapportLigneEnt, bool>> predicateWhere, string orderBy)
        {
            return Get()
                .Include(x => x.AffectationMoyen.Ci)
                .Include(x => x.AffectationMoyen.Personnel)
                .Include(x => x.Ci.Societe)
                .Include(x => x.Ci.EtablissementComptable)
                .Include(x => x.AffectationMoyen.Materiel.Ressource.SousChapitre.Chapitre)
                .Include(x => x.AffectationMoyen.Materiel.SiteAppartenance)
                .Include(x => x.AffectationMoyen.Materiel.Societe)
                .Include(x => x.AffectationMoyen.Materiel.EtablissementComptable)
                .Include(x => x.AffectationMoyen.Site)
                .Include(x => x.AffectationMoyen.TypeAffectation)
                .Where(predicateWhere)
                .OrderBy(orderBy)
                .ToList();
        }

        /// <summary>
        /// Récupère la liste du personnel supervisé parr l'utilisateur
        /// </summary>
        /// <param name="referentId">identifiant de l'utilisateur référent (correspondant paie)</param>
        /// <returns>List des entité Personnel représentant les personnes supervisé</returns>
        public IEnumerable<PersonnelEnt> GetPerimetrePointageByCreatorValidator(int referentId)
        {
            IIncludableQueryable<RapportLigneEnt, RapportEnt> queryableRapportLignes = GetDefaultQuery().Include(o => o.Rapport);
            return queryableRapportLignes.Where(p => p.Rapport.AuteurCreationId == referentId).Select(p => p.Personnel).Distinct().ToList();
        }

        /// <summary>
        /// Retourne une liste de pointages réels en fonction du personnel et d'une date
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="date">La date du pointage</param>
        /// <returns>Une liste de pointages réels</returns>
        public IEnumerable<RapportLigneEnt> GetListPointagesReelsByPersonnelIdAndDatePointage(int personnelId, int ciId, DateTime date)
        {
            return Context.RapportLignes
                          .Include(o => o.CodeAbsence)
                          .Include(o => o.ListRapportLignePrimes).ThenInclude(p => p.Prime)
                          .Include(o => o.ListRapportLigneAstreintes)
                          .Where(o => o.PersonnelId == personnelId &&
                                      o.CiId == ciId &&
                                      o.DatePointage.Year == date.Year &&
                                      o.DatePointage.Month == date.Month &&
                                      o.DatePointage.Day == date.Day &&
                                      !o.DateSuppression.HasValue);
        }

        /// <summary>
        /// Retourne une liste de pointages réels en fonction du ci et d'une date
        /// </summary>
        /// <param name="ciId">L'identifiant du CI</param>
        /// <param name="periode">La periode</param>
        /// <returns>Une liste de pointages réels</returns>
        public IEnumerable<RapportLigneEnt> GetListPointagesReelsByCiIdFromPeriode(int ciId, DateTime periode)
        {
            DateTime periodeDebut = periode.GetPeriode();
            return GetQueryForValorisationRefresh()
                .Where(o => o.CiId == ciId && o.DatePointage >= periodeDebut)
                .AsNoTracking();
        }

        /// <summary>
        /// Retourne une liste de pointages réels en fonction du List du ci et d'une date
        /// </summary>
        /// <param name="ciIds">List Ci ID</param>
        /// <param name="periode">La periode</param>
        /// <returns>Une liste de pointages réels</returns>
        public IEnumerable<RapportLigneEnt> GetListPointagesReelsByCiIdListFromPeriode(List<int> ciIds, DateTime periode)
        {
            DateTime periodeDebut = periode.GetPeriode();
            return GetQueryForValorisationRefresh()
                .Where(o => ciIds.Contains(o.CiId) && o.DatePointage >= periodeDebut)
                .AsNoTracking();
        }

        private IQueryable<RapportLigneEnt> GetQueryForValorisationRefresh()
        {
            return GetDefaultQuery()
                .Include(r => r.Ci.Societe.TypeSociete)
                .Include(r => r.Ci.Societe.AssocieSeps)
                .ThenInclude(a => a.TypeParticipationSep)
                .Include(r => r.Materiel.CommandeLignes)
                .ThenInclude(c => c.Unite);
        }

        /// <summary>
        /// Retourne une liste de pointages réels en fonction du personnel et d'un mois
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="month">La période du pointage</param>
        /// <returns>Une liste de pointages réels</returns>
        public IEnumerable<RapportLigneEnt> GetListPointagesOnMonthForPersonnelId(int personnelId, DateTime month)
        {
            List<RapportLigneEnt> rapportLignes = Context.RapportLignes
                          .Include(o => o.CodeAbsence)
                          .Include(o => o.CodeMajoration)
                          .Include(o => o.CodeDeplacement)
                          .Include(o => o.CodeZoneDeplacement)
                          .Include(o => o.AuteurCreation.Personnel)
                          .Include(o => o.AuteurModification.Personnel)
                          .Where(o => o.PersonnelId == personnelId && o.DatePointage.Month == month.Month && o.DatePointage.Year == month.Year && !o.Rapport.DateSuppression.HasValue && !o.DateSuppression.HasValue)
                          .OrderBy(p => p.DatePointage)
                          .ThenBy(p => p.Ci.Code)
                          .AsNoTracking()
                          .ToList();
            List<int> rapportLignesIds = rapportLignes.Select(x => x.RapportLigneId).Distinct().ToList();
            List<int> ciIds = rapportLignes.Select(p => p.CiId).Distinct().ToList();
            List<int?> personnelIds = rapportLignes.Select(p => p.PersonnelId).Distinct().ToList();
            List<int> rapportIds = rapportLignes.Select(x => x.RapportId).Distinct().ToList();
            List<int?> materielIds = rapportLignes.Select(x => x.MaterielId).Distinct().ToList();
            List<RapportLigneMajorationEnt> listRapportLigneMajorations = Context.RapportLigneMajorations.Include(x => x.CodeMajoration).Where(x => rapportLignesIds.Contains(x.RapportLigneId)).AsNoTracking().ToList();
            List<RapportLignePrimeEnt> listRapportLignePrimes = Context.RapportLignePrimes.Include(x => x.Prime).Where(x => rapportLignesIds.Contains(x.RapportLigneId)).AsNoTracking().ToList();
            List<RapportLigneTacheEnt> listRapportLigneTaches = Context.RapportLigneTaches.Include(x => x.Tache).Where(x => rapportLignesIds.Contains(x.RapportLigneId)).AsNoTracking().ToList();
            List<RapportLigneAstreinteEnt> listRapportLigneAstreintes = Context.RapportLigneAstreintes.Include(x => x.ListCodePrimeSortiesAstreintes).ThenInclude(y => y.CodeAstreinte).Where(x => rapportLignesIds.Contains(x.RapportLigneId)).AsNoTracking().ToList();
            List<Entities.CI.CIEnt> cis = Context.CIs
             .Include(o => o.CIType)
             .Include(o => o.Organisation)
             .Include(o => o.EtablissementComptable.Societe)
             .Include(o => o.Societe.Groupe)
             .Where(p => ciIds.Contains(p.CiId)).AsNoTracking().ToList();
            List<PersonnelEnt> personnels = Context.Personnels
                .Include(o => o.Societe)
                .Include(o => o.Ressource)
                .Include(o => o.EtablissementPaie)
                .Include(o => o.EtablissementRattachement)
                .Where(p => personnelIds.Contains(p.PersonnelId)).AsNoTracking().ToList();
            List<RapportEnt> rapports = Context.Rapports
                  .Include(o => o.ValideurCDC.Personnel)
                  .Include(o => o.ValideurCDT.Personnel)
                  .Include(o => o.ValideurDRC.Personnel)
                  .Include(o => o.AuteurVerrou.Personnel)
                  .Where(p => rapportIds.Contains(p.RapportId)).AsNoTracking().ToList();
            List<MaterielEnt> materiels = Context.Materiels
                 .Include(o => o.Societe)
                .Where(p => materielIds.Contains(p.MaterielId)).AsNoTracking().ToList();
            foreach (RapportLigneEnt rapportLigne in rapportLignes)
            {
                rapportLigne.Ci = cis.FirstOrDefault(x => x.CiId == rapportLigne.CiId);
                rapportLigne.Personnel = personnels.FirstOrDefault(x => x.PersonnelId == rapportLigne.PersonnelId);
                rapportLigne.Rapport = rapports.FirstOrDefault(x => x.RapportId == rapportLigne.RapportId);
                rapportLigne.Rapport.ListLignes = rapportLignes.Where(x => x.RapportId == rapportLigne.RapportId).ToList();
                rapportLigne.Materiel = materiels.FirstOrDefault(x => x.MaterielId == rapportLigne.MaterielId);
                rapportLigne.ListRapportLigneMajorations = listRapportLigneMajorations.Where(x => x.RapportLigneId == rapportLigne.RapportLigneId).ToList();
                rapportLigne.ListRapportLignePrimes = listRapportLignePrimes.Where(x => x.RapportLigneId == rapportLigne.RapportLigneId).ToList();
                rapportLigne.ListRapportLigneTaches = listRapportLigneTaches.Where(x => x.RapportLigneId == rapportLigne.RapportLigneId).ToList();
                rapportLigne.ListRapportLigneAstreintes = listRapportLigneAstreintes.Where(x => x.RapportLigneId == rapportLigne.RapportLigneId).ToList();
            }
            return rapportLignes;
        }

        /// <summary>
        /// Retourne une liste de pointages réels pour un personnel dans un exercice paie (à partir du mois de début d'exercice
        /// jusqu'au dernier jour du mois de fin d'exercice
        /// </summary>
        /// <param name="personnel">Le personnel</param>
        /// <returns>Le une liste de pointages</returns>
        public IEnumerable<RapportLigneEnt> GetListPointagesReelsInExerciceByPersonnel(PersonnelEnt personnel)
        {
            DateTime dateDebutExercice = new DateTime(DateTime.Now.Year, personnel.Societe.MoisDebutExercice.Value, 1);
            DateTime dateFinExercice = new DateTime(DateTime.Now.Year, personnel.Societe.MoisFinExercice.Value, 1).AddMonths(1).AddDays(-1);
            return GetDefaultQuery()
                .Where(o => o.PersonnelId == personnel.PersonnelId)
                .Where(o => o.DatePointage >= dateDebutExercice && o.DatePointage <= dateFinExercice);
        }

        /// <summary>
        /// Récupère la liste des Rapport Ligne Reel par User
        /// </summary>
        /// <param name="userid">identifiant de l'utilisateur (GSP)</param>
        /// <param name="annee">Année de filtrage</param>
        /// <param name="mois">Mois de filtrage</param>
        /// <returns>Liste des RapportLigne reelles</returns>
        public IEnumerable<int> GetAllPersonnelReelbyUser(int userid, int annee, int mois)
        {
            IIncludableQueryable<RapportLigneEnt, RapportEnt> qryRapportLignes = GetDefaultQuery().Include(o => o.Rapport);
            return qryRapportLignes.Where(p => p.PersonnelId != null)
                                   .Where(p => (p.Rapport != null && p.Rapport.AuteurVerrouId != null && (int)p.Rapport.AuteurVerrouId == userid)
                                    && (p.DatePointage.Year == annee && p.DatePointage.Month == mois)).Select(p => (int)p.PersonnelId).Distinct();

        }

        /// <summary>
        /// Récupère la liste des Rapport Ligne Reel par User
        /// </summary>
        /// <param name="userid">identifiant de l'utilisateur (GSP)</param>
        /// <param name="annee">Année de filtrage</param>
        /// <param name="mois">Mois de filtrage</param>
        /// <returns>Liste des RapportLigne reelles</returns>
        public IEnumerable<PointageBase> GetPointageVerrouillesByUserId(int userid, int annee, int mois)
        {
            Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<RapportLigneEnt, RapportEnt> qryRapportLignes = GetDefaultQuery().Include(o => o.Rapport);

            return qryRapportLignes.Where(p => p.PersonnelId != null)
              .Where(p => (p.Rapport != null && p.Rapport.AuteurVerrouId != null && (int)p.Rapport.AuteurVerrouId == userid)
                          && (p.DatePointage.Year == annee && p.DatePointage.Month == mois))
              .Distinct();
        }

        /// <summary>
        /// Indique si des pointages verrouillés par un utilisateur existent.
        /// </summary>
        /// <param name="userid">identifiant de l'utilisateur (GSP)</param>
        /// <param name="annee">Année de filtrage</param>
        /// <param name="mois">Mois de filtrage</param>
        /// <returns>True si des pointages existent, sinon false.</returns>
        public bool IsPointagesVerrouillesByUserExists(int userid, int annee, int mois)
        {
            return Get().Any(l =>
                  !l.DateSuppression.HasValue
                && l.PersonnelId != null && l.Personnel != null
                && l.DatePointage.Year == annee && l.DatePointage.Month == mois
                && l.Rapport != null && l.Rapport.AuteurVerrouId.HasValue && l.Rapport.AuteurVerrouId.Value == userid);
        }

        /// <summary>
        /// Récupère la liste des Rapport Ligne Reel par User
        /// </summary>
        /// <param name="userid">identifiant de l'utilisateur (GSP)</param>
        /// <param name="datemin">Année de filtrage</param>
        /// <param name="datemax">Mois de filtrage</param>
        /// <returns>Liste des RapportLigne reelles</returns>
        public IEnumerable<PointageBase> GetPointageVerrouillesByUserIdByPeriode(int userid, DateTime datemin, DateTime datemax)
        {
            return GetDefaultQuery().Include(o => o.Rapport)
              .Where(p => p.PersonnelId != null)
              .Where(p => (p.Rapport != null && p.Rapport.AuteurVerrouId != null && (int)p.Rapport.AuteurVerrouId == userid)
                          && (p.DatePointage >= datemin && p.DatePointage < datemax));
        }

        /// <summary>
        /// Supprimer la liste des lignes de rapport sauf celle spécifié
        /// </summary>
        /// <param name="rapportId">L'identifiant du rapport </param>
        /// <returns>Liste des RapportLigne qui n'existe pas dans la liste rapportLigneIdList</returns>
        public IEnumerable<RapportLigneEnt> GetOtherRapportLignes(int rapportId, List<int> rapportLigneIdList)
        {
            if (rapportLigneIdList != null && rapportLigneIdList.Any())
            {
                return this.Context.RapportLignes.Where(l => l.RapportId == rapportId && !rapportLigneIdList.Contains(l.RapportLigneId));
            }

            return this.Context.RapportLignes.Where(l => l.RapportId == rapportId);
        }

        /// <summary>
        /// Get personnel summary 
        /// </summary>
        /// <param name="personnelIdList">Personnel id list</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>IEnumerable of Personnel rapport summary</returns>
        public IEnumerable<PersonnelRapportSummaryEnt> GetPersonnelPointageSummary(List<int> personnelIdList, DateTime mondayDate)
        {
            DateTime sundayDate = mondayDate.AddDays(6);
            if (personnelIdList == null)
            {
                return new List<PersonnelRapportSummaryEnt>();
            }

            IQueryable<RapportLigneEnt> rapportPersonnelQuery = Query().Include(x => x.ListRapportLigneMajorations.Select(y => y.CodeMajoration)).Include(x => x.ListRapportLigneTaches)
                                        .Filter(x => x.PersonnelId.HasValue && x.DateSuppression == null && personnelIdList.Contains(x.PersonnelId.Value) && x.DatePointage >= mondayDate && x.DatePointage <= sundayDate).Get();
            var personnelRapportSummaryQuery = CalculPersonnelPointageSummaryTotalHours(rapportPersonnelQuery);
            return personnelRapportSummaryQuery.ToList();
        }

        private IQueryable<RapportLigneEnt> GetRapportLignesByPersonnelListAndPeriod(List<int> personnelIdList, DateTime mondayDate, DateTime sundayDate)
        {
            return Query()
                .Include(x => x.ListRapportLigneMajorations.Select(y => y.CodeMajoration))
                .Filter(x => x.PersonnelId.HasValue
                    && x.DateSuppression == null
                    && (personnelIdList.Contains(x.PersonnelId.Value) || x.Ci.IsAbsence)
                    && x.DatePointage >= mondayDate && x.DatePointage <= sundayDate)
                .Get();
        }

        /// <summary>
        /// Get ETAM/IAC summary 
        /// </summary>
        /// <param name="personnelIdList">Personnel id list</param>
        /// <param name="mondayDate">La date du lundi</param>
        /// <returns>IEnumerable of Personnel rapport summary</returns>
        public IEnumerable<PersonnelRapportSummaryEnt> GetEtamIacPointageSummary(List<int> personnelIdList, DateTime mondayDate)
        {
            List<PersonnelRapportSummaryEnt> result = new List<PersonnelRapportSummaryEnt>();
            DateTime sundayDate = mondayDate.AddDays(6);

            if (personnelIdList == null || !personnelIdList.Any())
            {
                return new List<PersonnelRapportSummaryEnt>();
            }
            List<RapportLigneEnt> rapportLignes = Query().Include(l => l.ListRapportLigneMajorations)
                                                        .Filter(x => x.PersonnelId.HasValue
                                                                       && x.DateSuppression == null
                                                                       && personnelIdList.Contains(x.PersonnelId.Value)
                                                                       && x.DatePointage >= mondayDate && x.DatePointage <= sundayDate)
                                                         .Get().ToList();
            foreach (int personnelId in personnelIdList)
            {
                List<RapportLigneEnt> personnelRapportLignes = rapportLignes.Where(l => l.PersonnelId == personnelId).ToList();
                result.Add(new PersonnelRapportSummaryEnt
                {
                    PersonnelId = personnelId,
                    TotalHeuresNormale = personnelRapportLignes.Sum(l => l.HeureNormale),
                    TotalHeuresAbsence = personnelRapportLignes.Sum(l => l.HeureAbsence),
                    TotalHeuresMajorations = personnelRapportLignes.Sum(l => (l.ListRapportLigneMajorations.Any() ? l.ListRapportLigneMajorations.Sum(m => m.HeureMajoration) : 0))
                });
            }
            return result;
        }

        /// <summary>
        /// Get ci pointage summary
        /// </summary>
        /// <param name="ciIdList">Ci id list</param>
        /// <param name="personnelStatut">Personnel statut</param>
        /// <param name="mondayDate">La date du lundi</param>
        /// <returns>IEnumerable de CiPointageSummary</returns>
        public IEnumerable<RapportLigneEnt> GetCiPointageSummary(List<int> ciIdList, string personnelStatut, DateTime mondayDate)
        {
            if (ciIdList.IsNullOrEmpty())
            {
                return new List<RapportLigneEnt>();
            }
            DateTime sundayDate = mondayDate.AddDays(6);
            return Query().Include(c => c.Ci)
                        .Include(c => c.Personnel)
                        .Include(c => c.ListRapportLigneTaches)
                        .Include(c => c.ListRapportLigneMajorations.Select(x => x.CodeMajoration))
                        .Filter(x => ciIdList.Contains(x.CiId) && x.Personnel != null && x.Personnel.Statut == personnelStatut && x.DateSuppression == null && x.DatePointage >= mondayDate && x.DatePointage <= sundayDate)
                        .Get().ToList();
        }

        /// <summary>
        /// Recupere les rapports lignes pour determiner le total des heures normales
        /// </summary>
        /// <param name="personnelIds">personnelIds</param>
        /// <param name="ciIds">ciIds</param>
        /// <param name="pointageDates">pointageDates</param>
        /// <returns>des rapports lignes</returns>
        public IEnumerable<RapportLigneEnt> GetRapportLignesForCalculateTotalHeuresNormalesAndMajorations(List<int> personnelIds, IEnumerable<int> ciIds, IEnumerable<DateTime> pointageDates)
        {
            return this.Context.RapportLignes
              .Where(l => l.PersonnelId.HasValue && personnelIds.Contains(l.PersonnelId.Value) && ciIds.Contains(l.CiId) && pointageDates.Contains(l.DatePointage) && l.DateSuppression == null).AsNoTracking().ToList();
        }

        /// <summary>
        /// Permet de récupérer le total des heures pointées( HeureNormale + HeureMajoration) a partir d'une liste de pointages
        /// </summary>
        /// <param name="rapportLignes">pointages</param>
        /// <param name="personnelId">personnelId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="pointageDate">pointageDate</param>
        /// <returns>le total des heures normales</returns>
        public double CalculateTotalHeuresNormalesAndMajorations(IEnumerable<RapportLigneEnt> rapportLignes, int personnelId, int ciId, DateTime pointageDate)
        {
            return rapportLignes.Where(l => l.PersonnelId.HasValue && l.PersonnelId.Value == personnelId && l.CiId == ciId && l.DatePointage == pointageDate && l.DateSuppression == null)
                .Sum(l => (double?)(l.HeureNormale + l.HeureMajoration)) ?? 0;
        }

        /// <summary>
        /// Retourne une liste de pointage dans une période donné et pour un périmètre précisé pour l'export excel des pointages intérimaire
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet Pointage Personnel Export Model</param>
        /// <param name="ciid">Liste d'identifiant unique de ci</param>
        /// <returns>Liste de pointages</returns>
        public IEnumerable<RapportLigneEnt> GetListPointagesInterimaireVerrouillesByPeriode(PointagePersonnelExportModel pointagePersonnelExportModel, IEnumerable<int> ciid)
        {
            pointagePersonnelExportModel.DateDebut = pointagePersonnelExportModel.DateDebut.Value.Date;
            pointagePersonnelExportModel.DateFin = pointagePersonnelExportModel.DateFin.Value.GetLimitsOfMonth().EndDate;
            IQueryable<RapportLigneEnt> query = Query()
              .Include(p => p.Personnel).Include(p => p.Ci)
              .Filter(p => p.Personnel.IsInterimaire).Filter(p => p.Rapport.RapportStatutId == 5)
              .Filter(p => pointagePersonnelExportModel.DateDebut <= p.DatePointage && p.DatePointage <= pointagePersonnelExportModel.DateFin)
              .Filter(p => ciid.Contains(p.CiId)).Filter(p => p.DateSuppression == null)
              .Get();
            if (pointagePersonnelExportModel.Rapport == 0)
            {
                query = query.Where(p => p.AuteurCreationId == pointagePersonnelExportModel.Utilisateur.UtilisateurId);
            }
            return query.AsNoTracking().ToList();
        }

        /// <summary>
        /// Check if rapport ligne existance
        /// </summary>
        /// <param name="rapportId">Rapport identifier</param>
        /// <param name="personnelId">Personnel Identifier</param>
        /// <returns>RapportLigne Identifier if exist</returns>
        public int CheckRapportLigneExistance(int rapportId, int personnelId)
        {
            RapportLigneEnt rapportLigne = this.Context.RapportLignes.FirstOrDefault(x => x.PersonnelId.HasValue && x.RapportId == rapportId && x.PersonnelId.Value == personnelId && x.DateSuppression == null);
            return rapportLigne != null ? rapportLigne.RapportLigneId : 0;
        }

        /// <summary>
        /// Get list des rapports ligne for synthese mensuelle
        /// </summary>
        /// <param name="personnelId">Identifiants du personnel</param>
        /// <param name="firstDayInMonth">le premier jour du mmois</param>
        /// <param name="lastDayInMonth">le dernier jour du mois</param>
        /// <returns>List des rapports ligne</returns>
        public IEnumerable<RapportLigneEnt> GetRapportLigneForSynthesMensuelle(int personnelId, DateTime firstDayInMonth, DateTime lastDayInMonth)
        {
            IEnumerable<RapportLigneEnt> rapportLignes = this.Context.RapportLignes.Where(x => x.PersonnelId.Value == personnelId && x.DatePointage >= firstDayInMonth && x.DatePointage <= lastDayInMonth && x.DateSuppression == null);
            return !rapportLignes.IsNullOrEmpty() ? rapportLignes : new List<RapportLigneEnt>();
        }

        /// <summary>
        /// Récuperer la list des personnels Ids affecte au Ci
        /// </summary>
        /// <param name="ciList">List des Ci</param>
        /// <param name="firstDayInMonth">Date du pointage</param>
        /// <param name="lastDayInMonth">Date du dernier jour du mois</param>
        /// <returns>List des peronnel identifiant </returns>
        public IEnumerable<RapportLigneEnt> GetEtamIacAffectationByCiList(IEnumerable<int> ciList, DateTime firstDayInMonth, DateTime lastDayInMonth)
        {
            return Context.RapportLignes.Include(x => x.ListRapportLignePrimes)
                          .Include(y => y.Personnel).Include(y => y.Rapport).Include(x => x.Rapport.RapportStatut)
                          .Where(x => ciList.Contains(x.CiId) && x.Personnel.Statut != TypePersonnel.Ouvrier && x.DatePointage >= firstDayInMonth && x.DatePointage <= lastDayInMonth && x.DateSuppression == null)
                          .ToList();
        }

        /// <summary>
        /// Get Etam Iac rapports for validation
        /// </summary>
        /// <param name="personnelId">Etam or Iac identifier</param>
        /// <param name="firstDayInMonth">Date du premier jour du mois</param>
        /// <param name="lastDayInMonth">Dernier jour du mois</param>
        /// <returns>List des rapports Lignes</returns>
        public IEnumerable<RapportLigneEnt> GetEtamIacRapportsForValidation(int personnelId, DateTime firstDayInMonth, DateTime lastDayInMonth)
        {
            return Context.RapportLignes.Include(x => x.RapportLigneStatut)
                          .Include(x => x.Rapport).Include(x => x.Rapport.RapportStatut)
                          .Include(x => x.ListRapportLignePrimes).Include(x => x.ListRapportLigneMajorations).ThenInclude(m => m.CodeMajoration)
                          .Where(x => x.PersonnelId != null && x.PersonnelId.Value == personnelId && x.DatePointage >= firstDayInMonth && x.DatePointage <= lastDayInMonth && x.DateSuppression == null)
                          .ToList();
        }

        /// <summary>
        /// Retourne le nombre de pointage sur un contrat interimaire
        /// </summary>
        /// <param name="contratInterimaireEnt">Contrat Interimaire</param>
        /// <returns>Un nombre de pointage</returns>
        public int GetPointageForContratInterimaire(ContratInterimaireEnt contratInterimaireEnt)
        {
            return Context.RapportLignes
                    .Where(rl => rl.PersonnelId == contratInterimaireEnt.InterimaireId)
                    .Where(rl => rl.DatePointage >= contratInterimaireEnt.DateDebut && rl.DatePointage <= contratInterimaireEnt.DateFin)
                    .Where(rl => rl.DateSuppression == null)
                    .Count();
        }

        /// <summary>
        ///  Retourne les pointages interimaires qui n'ont pas encore été réceptionnés
        /// </summary>      
        /// <param name="interimaireId">interimaireId</param>
        /// <param name="dateDebut">dateDebut</param>
        /// <param name="dateFin">dateFin</param>
        /// <returns>Liste d'ids des reception non receptionnees</returns>
        public List<RapportLigneEnt> GetPointagesInterimaireNonReceptionnees(int interimaireId, DateTime dateDebut, DateTime dateFin)
        {
            return Context.RapportLignes
                    .Where(rl => rl.PersonnelId == interimaireId && rl.Personnel.IsInterimaire)
                    .Where(rl => !rl.ReceptionInterimaire)
                    .Where(rl => rl.DatePointage >= dateDebut && rl.DatePointage <= dateFin)
                    .Where(rl => rl.DateSuppression == null)
                    .OrderBy(x => x.DatePointage)
                    .AsNoTracking()
                    .ToList();

        }

        public async Task<IEnumerable<RapportLigneEnt>> GetPointagesAsync(DateTime period)
        {
            int year = period.Year;
            int month = period.Month;

            return await Context.RapportLignes
                .AsNoTracking()
                .Include(rl => rl.Personnel.Societe.Groupe)
                .Include(rl => rl.Rapport)
                .Include(rl => rl.CodeAbsence)
                .Include(rl => rl.Ci.CIType)
                .Include(rl => rl.Ci.CompteInterneSep)
                .Include(rl => rl.CodeDeplacement)
                .Include(rl => rl.CodeZoneDeplacement)
                .Include(rl => rl.CodeMajoration)
                .Include(rl => rl.ListRapportLignePrimes).ThenInclude(rlp => rlp.Prime)
                .Include(rl => rl.ListRapportLigneMajorations).ThenInclude(rlm => rlm.CodeMajoration)
                .Include(rl => rl.ListCodePrimeAstreintes).ThenInclude(rlca => rlca.CodeAstreinte)
                .Include(rl => rl.ListCodePrimeAstreintes).ThenInclude(rlca => rlca.RapportLigneAstreinte)
                .Include(rl => rl.ListRapportLigneAstreintes).ThenInclude(rla => rla.Astreinte)
                .Include(rl => rl.ListRapportLigneTaches).ThenInclude(rlt => rlt.Tache)
                .Where(rl =>
                    !rl.DateSuppression.HasValue
                    && !rl.Rapport.DateSuppression.HasValue
                    && rl.Rapport.DateChantier.Month == month
                    && rl.Rapport.DateChantier.Year == year
                    && rl.Personnel != null
                    && (!rl.Personnel.DateSortie.HasValue || rl.Personnel.DateSortie.Value.Year >= year && rl.Personnel.DateSortie.Value.Month >= month)
                ).ToListAsync();
        }

        /// <summary>
        /// Retourne une liste de pointage dans une période donné et pour un périmètre précisé pour l'export excel des pointages personnel hebdomadaire
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet Pointage Personnel Export Model</param>
        /// <param name="ciid">Liste d'identifiant unique de ci</param>
        /// <returns>Liste de pointage</returns>
        public IEnumerable<RapportLigneEnt> GetListPointagePersonnelHebdomadaire(PointagePersonnelExportModel pointagePersonnelExportModel, IEnumerable<int> ciid)
        {
            pointagePersonnelExportModel.DateDebut = pointagePersonnelExportModel.DateDebut?.Date;
            pointagePersonnelExportModel.DateFin = pointagePersonnelExportModel.DateFin?.Date;
            IQueryable<RapportLigneEnt> query = Query().Include(p => p.Personnel).Include(p => p.Personnel.EtablissementRattachement)
              .Include(p => p.Personnel.Societe).Include(p => p.CodeAbsence)
              .Filter(p => p.Rapport.RapportStatutId == 5).Filter(p => p.Personnel != null)
              .Filter(p => p.DatePointage >= pointagePersonnelExportModel.DateDebut && p.DatePointage <= pointagePersonnelExportModel.DateFin)
              .Filter(p => p.DateSuppression == null).Filter(p => ciid.Contains(p.CiId))
              .Get();
            if (pointagePersonnelExportModel.Rapport == 0)
            {
                query = query.Where(p => p.Rapport.AuteurVerrouId == pointagePersonnelExportModel.Utilisateur.UtilisateurId);
            }
            if (pointagePersonnelExportModel.Personnel != null)
            {
                query = query.Where(p => p.PersonnelId == pointagePersonnelExportModel.Personnel.PersonnelId);
            }
            return query.ToList();
        }

        /// <summary>
        /// Retourne l'identifiant du matériel
        /// </summary>
        /// <param name="rapportLigneId">L'ientifiant ddu pointage</param>
        /// <returns>L'identifiant du matériel</returns>
        public int? GetMaterielId(int rapportLigneId)
        {
            return Context.RapportLignes.AsNoTracking().SingleOrDefault(p => p.RapportLigneId == rapportLigneId)?.MaterielId;
        }

        /// <summary>
        /// Retourne l'identifiant du personnel
        /// </summary>
        /// <param name="rapportLigneId">L'ientifiant ddu pointage</param>
        /// <returns>L'identifiant du personnel</returns>
        public int? GetPersonnelId(int rapportLigneId)
        {
            return Context.RapportLignes.AsNoTracking().SingleOrDefault(p => p.RapportLigneId == rapportLigneId)?.PersonnelId;
        }

        /// <summary>
        /// Retourne vrai si la prime journalière elle ddéjà dans un autre pointage
        /// </summary>
        /// <param name="primeId">identifiant de la prime</param>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="datePointage">Date du pointage</param>
        /// <param name="rapportligneId">Identifiant de la ligne de rapport</param>
        /// <returns>La prime journalière si elle existe</returns>
        public bool IsPrimeJournaliereAlreadyExists(int primeId, int personnelId, DateTime datePointage, int rapportligneId)
        {
            List<RapportLigneEnt> listPointages = Context.RapportLignes.Include("ListRapportLignePrimes.Prime").Where(p => p.PersonnelId == personnelId && p.DatePointage == datePointage && p.RapportLigneId != rapportligneId).ToList();
            foreach (RapportLigneEnt pointage in listPointages)
            {
                if (pointage.ListRapportLignePrimes.Any(o => !o.IsDeleted && o.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere && o.PrimeId == primeId && o.IsChecked))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Recupere les rapports lignes pour determiner si la prime journalière elle déjà dans un autre pointage
        /// </summary>
        /// <param name="personnelIds">Liste de personnelIds</param>
        /// <param name="datesPointagesOfPointages">datesPointagesOfPointages</param>
        /// <returns>des rapports lignes</returns>
        public IEnumerable<RapportLigneEnt> GetRapportLignesWithPrimesForCalculatePrimeJournaliereAlreadyExists(List<int> personnelIds, List<DateTime> datesPointagesOfPointages)
        {
            return Context.RapportLignes
                .Include(rl => rl.ListRapportLignePrimes).ThenInclude(rlp => rlp.Prime)
                .Where(p => p.PersonnelId.HasValue && personnelIds.Contains(p.PersonnelId.Value) && datesPointagesOfPointages.Contains(p.DatePointage) && p.DateSuppression == null)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Retourne vrai si la prime journalière elle déjà dans un autre pointage a partir d'une liste de rapports lignes
        /// </summary>
        /// <param name="rapportLignes">Liste de rapport lignes</param>
        /// <param name="primeId">identifiant de la prime</param>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="datePointage">Date du pointage</param>
        /// <param name="rapportligneId">Identifiant de la ligne de rapport</param>
        /// <returns>La prime journalière si elle existe</returns>
        public bool IsPrimeJournaliereAlreadyExists(IEnumerable<RapportLigneEnt> rapportLignes, int primeId, int personnelId, DateTime datePointage, int rapportligneId)
        {
            List<RapportLigneEnt> listPointages = rapportLignes.Where(p => p.PersonnelId == personnelId && p.DatePointage == datePointage && p.RapportLigneId != rapportligneId).ToList();
            foreach (RapportLigneEnt pointage in listPointages)
            {
                if (pointage.ListRapportLignePrimes.Any(o => !o.IsDeleted && o.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere && o.PrimeId == primeId && o.IsChecked))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Retouner la list des rapports lignes qui contient des primes TR ou IR
        /// </summary>
        /// <param name="personnelId">Personnel identifer</param>
        /// <param name="mondayDate">Date début de semaine</param>
        /// <param name="sundayDate">Date fin du semaine</param>
        /// <returns>List des rapports lignes</returns>
        public List<RapportLigneEnt> PrimePersonnelAffected(int personnelId, DateTime mondayDate, DateTime sundayDate)
        {
            return Context.RapportLignes.Include(x => x.ListRapportLignePrimes).ThenInclude(y => y.Prime)
                                  .Where(x => x.PersonnelId == personnelId && x.DatePointage >= mondayDate && x.DatePointage <= sundayDate && x.DateSuppression == null).ToList();
        }

        /// <summary>
        /// Retouner la liste des lignes de rapport
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="mondayDate">Date début de semaine</param>
        /// <param name="sundayDate">Date fin du semaine</param>
        /// <returns>Liste des lignes de rapport</returns>
        public IEnumerable<RapportLigneEnt> GetRapportLigneByPersonnelIdAndWeek(int personnelId, DateTime mondayDate, DateTime sundayDate)
        {
            return Context.RapportLignes.Include(x => x.ListRapportLigneMajorations).ThenInclude(y => y.CodeMajoration)
                                        .Include(x => x.ListRapportLignePrimes).ThenInclude(y => y.Prime)
                                  .Where(x => x.PersonnelId == personnelId && x.DatePointage >= mondayDate && x.DatePointage <= sundayDate && x.DateSuppression == null)
                                  .OrderBy(x => x.DatePointage);
        }

        /// <summary>
        /// Retourne les pointages vérouiller par rapport à personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique d'un personnel</param>
        /// <returns>Liste de pointage vérouiller</returns>
        public IEnumerable<RapportLigneEnt> GetPointageVerrouillerByPersonnelId(int personnelId)
        {
            return Context.RapportLignes.Where(rl => rl.PersonnelId == personnelId).Where(rl => rl.Rapport.RapportStatutId == 5).AsNoTracking().ToList();
        }

        /// <summary>
        /// Ajout ou mise à jour de masse des lignes de rapports
        /// </summary>
        /// <param name="rapportLignes">Liste de lignes de rapports</param>
        public void AddOrUpdateRapportLigneList(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            /* using (var ctxt = new FredDbContext()) N'a pas été utilisé ici car les mises à jour ne fonctionnent pas avec ce nouveau Contexte =>  mystère et boule de gomme d'entity framework... */
            using (IDbContextTransaction dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    IEnumerable<RapportLigneEnt> rapportLignesListToUpdate = rapportLignes.Where(x => x.RapportLigneId > 0);

                    Context.RapportLignes.UpdateRange(rapportLignesListToUpdate);

                    // Ajout des fournisseurs
                    IEnumerable<RapportLigneEnt> addedList = rapportLignes.Where(x => x.RapportLigneId == 0);
                    if (!addedList.IsNullOrEmpty())
                    {
                        Context.RapportLignes.AddRange(addedList);
                    }

                    Context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    throw new FredRepositoryException(e.Message, e);
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<RapportLigneEnt> GetpointageByPersonnelAndCi(int personnelId, int ciId)
        {
            return Context.RapportLignes.Where(x => x.PersonnelId == personnelId && x.CiId == ciId && x.DateSuppression == null).ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<RapportLigneEnt> GetPointageByCisPersonnelsAndDates(IEnumerable<int> ciIdList, IEnumerable<int> personnelIdList, DateTime startDate, DateTime endDate)
        {
            return Query()
                    .Include(o => o.ListRapportLigneTaches)
                    .Include(o => o.ListRapportLigneAstreintes)
                    .Include(o => o.ListRapportLigneMajorations)
                    .Include(o => o.ListRapportLignePrimes)
                    .Filter(x => !x.DateSuppression.HasValue)
                    .Filter(x => ciIdList.Contains(x.CiId) || !x.PersonnelId.HasValue || (x.PersonnelId.HasValue && personnelIdList.Contains(x.PersonnelId.Value)))
                    .Filter(x => x.DatePointage >= startDate && x.DatePointage <= endDate)
                    .Get()
                    .ToList();
        }

        /// <summary>
        /// Get pointage By Personnel And Ci And Date Astreinte
        /// </summary>
        /// <param name="personnelId"></param>
        /// <param name="ciId"></param>
        /// <param name="dateAstreinte"></param>
        /// <returns></returns>
        public RapportLigneEnt GetpointageByPersonnelAndCiAndDateAstreinte(int personnelId, int ciId, DateTime dateAstreinte)
        {
            return Context.RapportLignes.FirstOrDefault(x => x.CiId == ciId && x.PersonnelId == personnelId && x.DatePointage == dateAstreinte && !x.DateSuppression.HasValue);
        }

        /// <inheritdoc/>
        public IEnumerable<RapportLigneEnt> GetPointageByCiPersonnelAndDates(int ciId, int personnelId, DateTime startDate, DateTime endDate)
        {
            return Query()
                .Include(o => o.ListRapportLigneTaches)
                .Include(o => o.ListRapportLigneAstreintes)
                .Include(o => o.ListRapportLigneMajorations)
                .Include(o => o.ListRapportLignePrimes)
                .Filter(x => x.CiId == ciId && x.PersonnelId == personnelId && x.DateSuppression == null)
                .Filter(x => x.DatePointage >= startDate && x.DatePointage <= endDate)
                .Get()
                .ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<RapportLigneEnt> GetPointageByPersonnelAndCiByDate(int personnelId, int ciId, DateTime mondayDate)
        {
            DateTime sundayDate = mondayDate.AddDays(6);
            return Context.RapportLignes.Where(x => x.PersonnelId == personnelId && x.CiId == ciId && x.DateSuppression == null && x.DatePointage >= mondayDate && x.DatePointage <= sundayDate).ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<PersonnelRapportSummaryEnt> GetPointageChallengeSecurite(PointagePersonnelExportModel pointagePersonnelExportModel, IEnumerable<int> ciid)
        {
            pointagePersonnelExportModel.DateDebut = pointagePersonnelExportModel.DateDebut.Value.Date;
            pointagePersonnelExportModel.DateFin = pointagePersonnelExportModel.DateFin.Value.Date;
            IQueryable<RapportLigneEnt> rapportLignes = Query().Include(l => l.ListRapportLigneMajorations)
                .Include(l => l.CodeAbsence).Include(l => l.Rapport)
                .Filter(x => x.Rapport.RapportStatutId.Equals(5)) //Rapport vérrouillé
                .Filter(x => x.PersonnelId.HasValue).Filter(x => x.DateSuppression == null)
                .Filter(x => x.DatePointage >= pointagePersonnelExportModel.DateDebut && x.DatePointage.Date <= pointagePersonnelExportModel.DateFin)
                .Filter(x => ciid.Contains(x.CiId))
                .Get();
            if (pointagePersonnelExportModel.Rapport == 0)
            {
                rapportLignes = rapportLignes.Where(p => p.Rapport.AuteurVerrouId == pointagePersonnelExportModel.Utilisateur.UtilisateurId);
            }
            if (pointagePersonnelExportModel.Personnel != null)
            {
                rapportLignes = rapportLignes.Where(p => p.PersonnelId == pointagePersonnelExportModel.Personnel.PersonnelId);
            }

            IEnumerable<PersonnelRapportSummaryEnt> result;

            if (pointagePersonnelExportModel.Utilisateur.Societe.Groupe.Code == CodeGroupeFES)
            {
                result = rapportLignes.GroupBy(r => r.PersonnelId).Select(g =>
               new PersonnelRapportSummaryEnt
               {
                   PersonnelId = (int)g.Key,
                   CiId = g.Select(rl => rl.CiId).FirstOrDefault(),
                   TotalHeuresNormale = g.Sum(rl => rl.HeureNormale),
                   TotalHeuresAbsence = g.Where(a => a.CodeAbsence.Code.Equals("05")).Sum(rl => rl.HeureAbsence),//heures en délégation (code absence = 05) 
                   TotalHeuresMajorations = g.Sum(rl => (rl.ListRapportLigneMajorations.Any() ? rl.ListRapportLigneMajorations.Sum(m => m.HeureMajoration) : 0))
               });
            }
            else
            {
                result = rapportLignes.GroupBy(r => r.PersonnelId).Select(g =>
               new PersonnelRapportSummaryEnt
               {
                   PersonnelId = (int)g.Key,
                   CiId = g.Select(rl => rl.CiId).FirstOrDefault(),
                   TotalHeuresNormale = g.Sum(rl => rl.HeureNormale),
                   TotalHeuresAbsence = g.Where(a => a.CodeAbsence.Code.Equals("05")).Sum(rl => rl.HeureAbsence),//heures en délégation (code absence = 05) 
                   TotalHeuresMajorations = g.Sum(rl => rl.HeureMajoration)
               });
            }

            return result.ToList();
        }

        /// <inheritdoc/>
        public ExportTibcoRapportLigneModel[] GetPointageMoyenBetweenDates(DateTime startDate, DateTime endDate)
        {
            return
                Query().Filter(o => o.AffectationMoyenId.HasValue).Filter(x => x.DatePointage >= startDate && x.DatePointage <= endDate)
                .Get()
                .Select(x => new ExportTibcoRapportLigneModel
                {
                    Annee = x.DatePointage.Year.ToString(),
                    Mois = x.DatePointage.Month.ToString(),
                    DatePointage = x.DatePointage,
                    CiCode = x.Ci.Code,
                    Commentaire = x.AffectationMoyen.Commentaire,
                    ConducteurMatricule = x.AffectationMoyen.Conducteur.Matricule,
                    ConducteurNom = x.AffectationMoyen.Conducteur.Nom,
                    ConducteurPrenom = x.AffectationMoyen.Conducteur.Prenom,
                    ConducteurSociete = x.AffectationMoyen.Conducteur.Societe.Code,
                    EtablissementComptableCi = x.Ci.EtablissementComptable.Code,
                    EtablissementComptableCode = x.AffectationMoyen.Materiel.EtablissementComptable.Code,
                    ImmatriculationMateriel = x.AffectationMoyen.Materiel.Immatriculation,
                    ImmatriculationMaterielLocation = x.AffectationMoyen.MaterielLocation.Immatriculation,
                    MoyenCode = x.AffectationMoyen.Materiel.Code,
                    PersonnelMatricule = x.Personnel.Matricule,
                    PersonnelNom = x.Personnel.Nom,
                    PersonnelPrenom = x.Personnel.Prenom,
                    PersonnelSociete = x.Personnel.Societe.Code,
                    SocieteCi = x.Ci.Societe.Code,
                    SocieteCode = x.AffectationMoyen.Materiel.Societe.Code,
                    Quantite = x.HeuresMachine
                })
                .ToArray();
        }

        /// <inheritdoc/>
        public List<RapportLigneEnt> MajorationPersonnelAffected(int personnelId, DateTime mondayDate, DateTime sundayDate)
        {
            return Context.RapportLignes.Include(x => x.ListRapportLigneMajorations).ThenInclude(y => y.CodeMajoration)
                                  .Where(x => x.PersonnelId == personnelId && x.DatePointage >= mondayDate && x.DatePointage <= sundayDate && x.DateSuppression == null).ToList();
        }

        /// <summary>
        /// Récupère la liste des pointages, dans le périmètre de l'utilisateur connecté, des rapports verrouillés d'une période donnée
        /// </summary>
        /// <param name="periode">Période choisie</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="etablissementPaieIdList">Liste des indentifiants d'établissements de paie</param>
        /// <returns>Liste de pointages</returns>
        public IEnumerable<RapportLigneEnt> GetAllLockedPointagesForSocieteOrEtablissementForVerificationCiSep(DateTime periode, int? societeId, IEnumerable<int> etablissementPaieIdList)
        {
            return Query().Include(rl => rl.Ci.Societe.TypeSociete).Include(rl => rl.Ci.EtablissementComptable).Include(rl => rl.Personnel)
                   .Filter(x => ((societeId.HasValue && x.Ci.SocieteId == societeId && !etablissementPaieIdList.Any())
                           || (x.Personnel.EtablissementPaieId.HasValue && etablissementPaieIdList.Contains(x.Personnel.EtablissementPaieId.Value))) &&
                           x.LotPointageId.HasValue && x.LotPointage.Periode.Month == periode.Month && x.LotPointage.Periode.Year == periode.Year).Get();
        }

        /// <summary>
        /// Get Rapport ligne by rapport and personnel identifiers
        /// </summary>
        /// <param name="rapportId">Rapport identifier</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>Rapport ligne</returns>
        public RapportLigneEnt GetRapportLigneByRapportIdAndPersonnelId(int rapportId, int personnelId)
        {
            return Context.RapportLignes.Include(x => x.CodeAbsence).Include(x => x.ListCodePrimeAstreintes).Include(x => x.ListRapportLigneAstreintes).Include(x => x.ListRapportLigneMajorations).Include(x => x.ListRapportLignePrimes).Include(x => x.ListRapportLigneTaches).FirstOrDefault(x => x.RapportId == rapportId && x.PersonnelId == personnelId && x.DateSuppression == null);
        }

        /// <summary>
        /// Get rapport ligne statut
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="datePointage">Date pointage</param>
        /// <returns>Rapport ligne statut</returns>
        public string GetRapportLigneStatutCode(int personnelId, int ciId, DateTime datePointage)
        {
            RapportLigneEnt rapportLigne = Context.RapportLignes.Include(x => x.RapportLigneStatut)
                                                                .FirstOrDefault(x => x.PersonnelId == personnelId &&
                                                                                x.CiId == ciId &&
                                                                                x.DatePointage == datePointage &&
                                                                                !x.DateSuppression.HasValue);
            if (rapportLigne != null && rapportLigne.RapportLigneStatut != null)
            {
                return rapportLigne.RapportLigneStatut.Code;
            }
            return string.Empty;
        }

        /// <summary>
        /// Récuperer les rapports lignes par l'affectation moyen identifier
        /// </summary>
        /// <param name="affectationMoyenIdList">Affectation moyen list des identifiers</param>
        /// <returns>List des rapports lignes</returns>
        public List<RapportLigneEnt> GetPointageByAffectaionMoyenIds(IEnumerable<int> affectationMoyenIdList)
        {
            return Context.RapportLignes.Where(x => affectationMoyenIdList.Contains(x.AffectationMoyenId.Value) && !x.DateSuppression.HasValue).ToList();
        }

        /// <summary>
        /// Get Rapport Lignes des absence FIGGO pour une période
        /// </summary>
        /// <param name="dateDebut">Date de debut</param>
        /// <param name="dateFin">Date de fin</param>
        /// <returns>listes des absences FIGGO</returns>
        public IEnumerable<RapportLigneEnt> GetListPointagesFiggoByPeriode(DateTime dateDebut, DateTime dateFin)
        {
            return Context.RapportLignes.Include(r => r.Ci).Include(r => r.AffectationAbsence).Include(r => r.Personnel).Where(r => r.Ci.IsAbsence && dateDebut.Date <= r.DatePointage.Date && r.DatePointage.Date <= dateFin.Date).AsNoTracking();
        }

        /// <summary>
        /// Récupére le pointage des personnels
        /// </summary>
        /// <param name="filter">filtre</param>
        /// <returns>Liste des lignes des rapports</returns>
        public List<RapportLigneSelectModel> GetPointagePersonnelByFilter(ExportPointagePersonnelFilterModel filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(FeatureRapport.ExportPointagePersonnel_Error_Input);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RapportLigneEnt, RapportLigneSelectModel>();
                cfg.CreateMap<PersonnelEnt, PersonnelSelectModel>();
                cfg.CreateMap<RapportLigneTacheEnt, LigneTacheSelectModel>();
                cfg.CreateMap<RapportLigneMajorationEnt, LigneMajorationSelectModel>();
                cfg.CreateMap<RapportLignePrimeEnt, LignePrimeSelectModel>();
                cfg.CreateMap<RapportLigneAstreinteEnt, LigneAstreinteSelectModel>();
            });

            var rapportList = Context.RapportLignes.ProjectTo<RapportLigneSelectModel>(config)
                 .Where(filter.WhereRapport)
                 .Where(filter.WhereBetweenDates)
                 .Where(filter.WhereEtablissement)
                 .Where(rl => rl.DatePointage <= rl.Personnel.DateSortie || rl.Personnel.DateSortie == null)
                 .Where(x => !x.DateSuppression.HasValue)
                 .ToList();

            if (filter != null && !(string.IsNullOrEmpty(filter.SocieteCode)))
            {
                string group = Context.Societes.Include(zz => zz.Groupe).FirstOrDefault(x => x.Code.Equals(filter.SocieteCode))?.Groupe?.Code;
                rapportList.ForEach(item =>
                {
                    if (string.IsNullOrEmpty(group))
                    {
                        throw new ArgumentNullException(FeatureRapport.Group_non_existent);
                    }
                    //TO DO il faut voir ajouter des modifs de la Us 5253
                    item.MaxHeuresTravailleesJour = group.Contains("GFES") ? 7 : 12;
                });
            }

            var astraintesList = Context.CodeAstreintes.ToList();
            var RapportLigneAstreinteIdList = rapportList.SelectMany(x => x.ListRapportLigneAstreintes)
                                  .SelectMany(z => z.ListCodePrimeSortiesAstreintes)
                                  .Select(rl => rl.RapportLigneAstreinteId).ToList();
            var RapportLigneAstreinteList = Context.RapportLigneAstreintes.Where(x => RapportLigneAstreinteIdList != null
                                                                                   && RapportLigneAstreinteIdList.Any()
                                                                                   && RapportLigneAstreinteIdList.Contains(x.RapportLigneAstreinteId)).ToList();
            foreach (var item in rapportList)
            {
                if (item.ListRapportLigneAstreintes.Any())
                    foreach (var astreintLigne in item.ListRapportLigneAstreintes)
                    {
                        if (astreintLigne.ListCodePrimeSortiesAstreintes.Any())
                        {
                            foreach (var items in astreintLigne.ListCodePrimeSortiesAstreintes)
                            {
                                astreintLigne.AstreinteCode = astraintesList?.FirstOrDefault(x => items.CodeAstreinteId.Equals(x.CodeAstreinteId))?.Code;
                                var selectedRapportLigneAstreint = RapportLigneAstreinteList?.FirstOrDefault(x => x.RapportLigneAstreinteId == items.RapportLigneAstreinteId);
                                astreintLigne.HeureAstreinte = selectedRapportLigneAstreint != null ? (selectedRapportLigneAstreint.DateFinAstreinte - selectedRapportLigneAstreint.DateDebutAstreinte).TotalHours : 0;
                            }
                        }
                    }
            }

            return rapportList;
        }

        /// <summary>
        /// Sauvegarde les modifications apportée à un rapport ligne
        /// </summary>
        /// <param name="rapportLigne">Rapport ligne à modifier</param>
        public async Task UpdateRangeRapportLigneAsync(IEnumerable<RapportLigneEnt> rapportLigne)
        {
            Context.RapportLignes.UpdateRange(rapportLigne);
        }

        private IQueryable<PersonnelRapportSummaryEnt> CalculPersonnelPointageSummaryTotalHours(IQueryable<RapportLigneEnt> rapportPersonnelQuery)
        {
            return from q in rapportPersonnelQuery
                   group q by new { q.PersonnelId, q.CiId } into groupResult
                   select new PersonnelRapportSummaryEnt
                   {
                       PersonnelId = groupResult.Key.PersonnelId.Value,
                       CiId = groupResult.Key.CiId,
                       TotalHeuresNormale = groupResult.Sum(s => s.HeureNormale),
                       TotalHeuresAbsence = groupResult.Sum(s => s.HeureAbsence),
                       TotalHeuresMajorations = groupResult.Sum(s => s.ListRapportLigneMajorations.Any() ?
                                                s.ListRapportLigneMajorations.Where(m => m.CodeMajoration.IsHeureNuit).Sum(m => m.HeureMajoration) : 0),

                       TotalHeuresNormalesSup = groupResult.Sum(s => s.HeureNormale) + groupResult.Sum(s => s.HeureAbsence) +
                       groupResult.Sum(s => s.ListRapportLigneMajorations.Any() ? s.ListRapportLigneMajorations.Where(m => m.CodeMajoration.IsHeureNuit).Sum(m => m.HeureMajoration) : 0) - MaxHeurePoinatageFES > 0 ?
                       groupResult.Sum(s => s.HeureNormale) + groupResult.Sum(s => s.HeureAbsence) +
                       groupResult.Sum(s => s.ListRapportLigneMajorations.Any() ? s.ListRapportLigneMajorations.Where(m => m.CodeMajoration.IsHeureNuit).Sum(m => m.HeureMajoration) : 0) - MaxHeurePoinatageFES : 0
                   };
        }

        /// <summary>
        /// Modifie l'identifiant du contrat interimaire d'une liste de rappoer ligne
        /// </summary>
        /// <param name="rapportLignesIds">Liste des identifiants rapport ligne</param>
        /// <param name="contratInterimaireId">Identifiant contrat interimaire</param>
        public void UpdateContratId(List<int> rapportLignesIds, int contratInterimaireId)
        {
            List<RapportLigneEnt> linesToUpdate = Context.RapportLignes.Where(x => rapportLignesIds.Contains(x.RapportLigneId))?.ToList();
            if (linesToUpdate != null && linesToUpdate.Any())
            {
                foreach (var l in linesToUpdate)
                {
                    l.ContratId = contratInterimaireId;
                }

                Context.UpdateRange(linesToUpdate);
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Récupére une liste de rapport ligne parune liste d'ID
        /// </summary>
        /// <param name="idsList">La liste des ids</param>
        /// <returns></returns>
        public IEnumerable<RapportLigneEnt> GetRapportLignesByIds(List<int> idsList)
        {
            return Context.RapportLignes.Where(x => idsList.Contains(x.RapportLigneId)).AsNoTracking().ToList();
        }

        public List<RapportLigneEnt> GetRapportLignesToUpdateValorisationFromListBaremeStorm(List<int> ressourcesIds, List<int> ciIds, List<CiDernierePeriodeComptableNonCloturee> ciDernierePeriodeComptableNonCloturees)
        {
            if (ciIds == null)
                return new List<RapportLigneEnt>();

            //recup materiels associés aux ressources des baremes modifiés
            var materielsIds = this.Context.Materiels.Where(x => ressourcesIds.Contains(x.RessourceId)).Select(x => x.MaterielId).ToList();

            DateTime periodeDebut = ciDernierePeriodeComptableNonCloturees.Min(x => x.DernierePeriodeComptableNonCloturee).GetPeriode();

            List<RapportLigneDatePointageInfo> allRapportLignesInfosOnCids = this.Context.RapportLignes
                                                .Where(x => ciIds.Contains(x.CiId) && x.DatePointage >= periodeDebut)
                                                .Where(x => !x.DateSuppression.HasValue)
                                                .Where(x => !x.Cloture)
                                                .Select(x => new RapportLigneDatePointageInfo
                                                {
                                                    CiId = x.CiId,
                                                    RapportLigneId = x.RapportLigneId,
                                                    DatePointage = x.DatePointage
                                                })
                                                .AsNoTracking()
                                                .ToList();

            List<int> rapportLigneIdsNotClosed = GetPointagesOnPeriodeNotClosed(allRapportLignesInfosOnCids, ciDernierePeriodeComptableNonCloturees);

            List<RapportLigneDatePointageInfo> rapportLignesOfMateriels = GetPointagesMateriels(rapportLigneIdsNotClosed, materielsIds);

            List<int> rapportLignesToSelect = rapportLignesOfMateriels.Select(x => x.RapportLigneId).Distinct().ToList();

            var rapportLignes = this.Context.RapportLignes.Where(x => rapportLignesToSelect.Contains(x.RapportLigneId))
                .Include(o => o.Ci.Organisation)
                .Include(o => o.Ci.EtablissementComptable.Societe)
                .Include(o => o.Ci.Societe)
                .Include(o => o.Personnel.Societe.Organisation)
                .Include(o => o.Personnel.Ressource)
                .Include(o => o.Personnel.EtablissementPaie)
                .Include(o => o.Personnel.EtablissementRattachement)
                .Include(o => o.CodeAbsence)
                .Include(o => o.CodeMajoration)
                .Include(o => o.ListRapportLigneMajorations).ThenInclude(x => x.CodeMajoration)
                .Include(o => o.CodeDeplacement).Include(o => o.CodeZoneDeplacement)
                .Include(o => o.Materiel)
                .Include(o => o.ListRapportLignePrimes)
                    .ThenInclude(ooo => ooo.Prime)
                .Include(o => o.ListRapportLigneTaches)
                    .ThenInclude(ooo => ooo.Tache)
                .Include(r => r.Ci.Societe.TypeSociete)
                .Include(r => r.Ci.Societe.AssocieSeps)
                .ThenInclude(a => a.TypeParticipationSep)
                .Include(r => r.Materiel.CommandeLignes)
                .ThenInclude(c => c.Unite)
                .AsNoTracking()
                .ToList();

            return rapportLignes;

        }

        private List<RapportLigneDatePointageInfo> GetPointagesMateriels(List<int> rapportLigneIdsNotClosed, List<int> materielsIds)
        {
            return this.Context.RapportLignes
                                .Where(x => rapportLigneIdsNotClosed.Contains(x.RapportLigneId))
                                .Where(x => materielsIds.Contains(x.MaterielId.Value))
                                .Where(x => x.MaterielId.HasValue && !x.Materiel.MaterielLocation)
                                .Select(x => new RapportLigneDatePointageInfo { CiId = x.CiId, RapportLigneId = x.RapportLigneId, DatePointage = x.DatePointage })
                                .ToList();
        }

        private List<int> GetPointagesOnPeriodeNotClosed(List<RapportLigneDatePointageInfo> rapportLigneInfos,
                                                         List<CiDernierePeriodeComptableNonCloturee> ciDernierePeriodeComptableNonCloturees)
        {
            var result = new List<int>();

            foreach (var rapportLigneInfo in rapportLigneInfos)
            {
                var ciOfRapportLigne = rapportLigneInfo.CiId;

                var dernierePeriodeComptableNonCloturee = ciDernierePeriodeComptableNonCloturees.Where(x => x.CiId == ciOfRapportLigne).First();

                var periodeDebutNotClosedForCi = dernierePeriodeComptableNonCloturee.DernierePeriodeComptableNonCloturee;

                var isRapportLigneNotClosed = rapportLigneInfo.DatePointage >= periodeDebutNotClosedForCi;

                if (isRapportLigneNotClosed)
                {
                    result.Add(rapportLigneInfo.RapportLigneId);
                }
            }

            return result;
        }

        public RapportLigneEnt GetPointageByPersonnelIdAndDatePointage(int personnelId, int ciId, DateTime date)
        {
            return Context.RapportLignes
                          .Where(o => o.PersonnelId == personnelId &&
                                      o.CiId == ciId &&
                                      o.DatePointage.Year == date.Year &&
                                      o.DatePointage.Month == date.Month &&
                                      o.DatePointage.Day == date.Day &&
                                      !o.DateSuppression.HasValue).FirstOrDefault();
        }

    }
}
