using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.RepartitionEcart;
using Fred.Business.Utilisateur;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CloturesPeriodes;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Utilisateur;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.DatesClotureComptable
{
    public class CloturesPeriodesManager : Manager<DatesClotureComptableEnt, IDatesClotureComptableRepository>, ICloturesPeriodesManager
    {
        private readonly IDatesClotureComptableRepository datesClotureComptableRepository;
        private readonly IUnitOfWork uow;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IRepartitionEcartManager repartitionEcartManager;
        private readonly IValorisationManager valorisationManager;
        private readonly ICIRepository ciRepository;

        public CloturesPeriodesManager(
            IUnitOfWork uow,
            IDatesClotureComptableRepository datesClotureComptableRepository,
            IUtilisateurManager utilisateurManager,
            IRepartitionEcartManager repartitionEcartManager,
            IValorisationManager valorisationManager,
            ICIRepository ciRepository)
        : base(uow, datesClotureComptableRepository)
        {
            this.uow = uow;
            this.utilisateurManager = utilisateurManager;
            this.datesClotureComptableRepository = datesClotureComptableRepository;
            this.repartitionEcartManager = repartitionEcartManager;
            this.valorisationManager = valorisationManager;
            this.ciRepository = ciRepository;
        }

        /// <summary>
        /// Récupération des résultats de la recherche en fonction du filtre
        /// </summary>
        /// <param name="searchCloturesPeriodesForCiEnt">Recherche des dates de clôture comptable</param>
        /// <returns>Les résultats de la recherche en fonction du filtre</returns>
        public PlageCisDatesClotureComptableDto SearchFilter(SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt)
        {
            try
            {
                IQueryable<CiDateClotureComptableDto> queryDatesClotureComptable = GetDatesClotureComptableFiltrable(searchCloturesPeriodesForCiEnt);
                List<CiDateClotureComptableDto> results = queryDatesClotureComptable.OrderBy(a => a.CiId).ToList();

                return new PlageCisDatesClotureComptableDto
                {
                    Filter = searchCloturesPeriodesForCiEnt,
                    Date = DateTime.Now,
                    Items = results.Select(x => x).ToList()
                };
            }
            catch (DataException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de clôturer uniquement les CI sélectionnés
        /// </summary>
        /// <param name="date">date de Transfert</param>
        /// <param name="annee">annee</param>
        /// <param name="mois">mois</param>
        /// <param name="searchCloturesPeriodesForCiEnt">Filtre multi-critères de la date clôture comptable</param>
        /// <param name="identifiantsSelected">La liste des identifiants sélectionnés de centre imputation</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        public async Task<List<DatesClotureComptableEnt>> CloturerSeulementDepensesSelectionneesAsync(DateTime date, int annee, int mois, SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt, List<int> identifiantsSelected)
        {
            try
            {
                IQueryable<CiDateClotureComptableNavigableEnt> queryDatesClotureComptable = QueryDatesClotureComptableWithoutDateClotureOnlyCiJoints(searchCloturesPeriodesForCiEnt, identifiantsSelected);
                LockValorisation(annee, mois, identifiantsSelected);
                return await CloturerQueryDatesClotureComptableAsync(queryDatesClotureComptable, date, annee, mois).ConfigureAwait(false);
            }
            catch (DataException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        private void LockValorisation(int annee, int mois, List<int> identifiantsSelected)
        {
            valorisationManager.UpdateVerrouPeriodeValorisation(identifiantsSelected, annee, mois, true);
        }

        /// <summary>
        /// Permet de déclôturer uniquement les CI sélectionnés
        /// </summary>
        /// <param name="date">date de Transfert Date</param>
        /// <param name="annee">annee</param>
        /// <param name="mois">mois</param>
        /// <param name="searchCloturesPeriodesForCiEnt">Filtre multi-critères de la date clôture comptable</param>
        /// <param name="identifiantsSelected">La liste des identifiants sélectionnés de centre imputation</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        public async Task<IEnumerable<DatesClotureComptableEnt>> DecloturerSeulementDepensesSelectionneesAsync(DateTime date, int annee, int mois, SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt, List<int> identifiantsSelected)
        {
            try
            {
                IQueryable<CiDateClotureComptableNavigableEnt> queryDatesClotureComptable = QueryDatesClotureComptableWithDateClotureOnlyCiJoints(searchCloturesPeriodesForCiEnt, identifiantsSelected);
                return await DecloturerQueryDatesClotureComptableAsync(queryDatesClotureComptable, date, annee, mois).ConfigureAwait(false);
            }
            catch (DataException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de clôturer toutes les peridodeCloturerToutesDepensesSaufSelectionnees
        /// </summary>
        /// <param name="date">date de clôture</param>
        /// <param name="annee">annee</param>
        /// <param name="mois">mois</param>
        /// <param name="searchCloturesPeriodesForCiEnt">Recherche des dates de clôture comptable</param>
        /// <param name="identifiantsSelected">La liste des identifiants sélectionnés de centre imputation</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        public async Task<IEnumerable<DatesClotureComptableEnt>> CloturerToutesDepensesSaufSelectionneesAsync(DateTime date, int annee, int mois, SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt, List<int> identifiantsSelected)
        {
            try
            {
                IQueryable<CiDateClotureComptableNavigableEnt> queryDatesClotureComptable = QueryDatesClotureComptableExceptCiJoints(searchCloturesPeriodesForCiEnt, identifiantsSelected);
                LockValorisation(annee, mois, identifiantsSelected);
                List<DatesClotureComptableEnt> datesClotureComptables = await CloturerQueryDatesClotureComptableAsync(queryDatesClotureComptable, date, annee, mois).ConfigureAwait(false);
                //CCA  : C'est moche de faire ça mais je n'ai pas le temps de faire mieux
                //Bonne pratique :  renvoyer un model et non une entité
                datesClotureComptables.ForEach(x => x.CI.DatesClotureComptables.ForEach(q => q.CI = null));
                return datesClotureComptables;
            }
            catch (DataException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// CloturerQueryDatesClotureComptable
        /// </summary>
        /// <param name="queryDatesClotureComptable">queryDatesClotureComptable</param>
        /// <param name="date">date</param>
        /// <param name="annee">annee</param>
        /// <param name="mois">mois</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        private async Task<List<DatesClotureComptableEnt>> CloturerQueryDatesClotureComptableAsync(IQueryable<CiDateClotureComptableNavigableEnt> queryDatesClotureComptable, DateTime date, int annee, int mois)
        {
            IQueryable<CiDateClotureComptableNavigableEnt> updateQueryableDatesClotureComptable = queryDatesClotureComptable.Where(x => x.DatesClotureComptable != null);
            // monter en mémoire une liste des dates comptables pour l'insertion des nouvelles dates de clôture (donc si déja clôturé, on écarte)
            List<CiDateClotureComptableNavigableEnt> datesComptableAndCis = queryDatesClotureComptable.Where(x => x.DatesClotureComptable == null || x.DatesClotureComptable.DateCloture == null).ToList();
            if (datesComptableAndCis.Count > 0)
            {
                int currentUserId = utilisateurManager.GetContextUtilisateurId();

                int datesHistorisees = UpdateHistorisationDatesComptableAndCisWithDateTimeNow(date, updateQueryableDatesClotureComptable.Select(x => x.DatesClotureComptable), currentUserId);
                ProcessDateClotureComptable(date, annee, mois, datesComptableAndCis, currentUserId, datesHistorisees);
                datesComptableAndCis.ForEach(datesComptableAndCi => datesComptableAndCi.DatesClotureComptable.DateCloture = date);

                AddRangeDatesComptableAndCis(datesComptableAndCis.Select(x => x.DatesClotureComptable));

                Save();

                await repartitionEcartManager.ClotureAsync(datesComptableAndCis.Select(q => q.Ci.CiId).Distinct().ToList(), new DateTime(annee, mois, 15)).ConfigureAwait(false);
            }
            return datesComptableAndCis.Select(x => x.DatesClotureComptable).ToList();
        }

        /// <summary>
        /// CloturerQueryDatesClotureComptable
        /// </summary>
        /// <param name="queryDatesClotureComptable">queryDatesClotureComptable</param>
        /// <param name="date">date</param>
        /// <param name="annee">annee</param>
        /// <param name="mois">mois</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        private async Task<IEnumerable<DatesClotureComptableEnt>> DecloturerQueryDatesClotureComptableAsync(IQueryable<CiDateClotureComptableNavigableEnt> queryDatesClotureComptable, DateTime date, int annee, int mois)
        {
            IQueryable<CiDateClotureComptableNavigableEnt> updateQueryableDatesClotureComptable = queryDatesClotureComptable.Where(x => x.DatesClotureComptable != null);
            // monter en mémoire une liste des dates comptables pour l'insertion des nouvelles dates de clôture (donc si déja déclôturé, on écarte)
            List<CiDateClotureComptableNavigableEnt> datesComptableAndCis = queryDatesClotureComptable.Where(x => x.DatesClotureComptable == null || x.DatesClotureComptable.DateCloture != null).ToList();
            int currentUserId = utilisateurManager.GetContextUtilisateurId();
            await repartitionEcartManager.DeClotureAsync(datesComptableAndCis.Select(q => q.Ci.CiId).ToList(), new DateTime(annee, mois, 15)).ConfigureAwait(false);

            int datesHistorisees = UpdateDatesComptableAndCisDateClotureWithDateTimeNull(date, updateQueryableDatesClotureComptable.Select(x => x.DatesClotureComptable), currentUserId);
            ProcessDateClotureComptable(date, annee, mois, datesComptableAndCis, currentUserId, datesHistorisees);
            datesComptableAndCis.ForEach(datesComptableAndCi => datesComptableAndCi.DatesClotureComptable.DateCloture = null);

            AddRangeDatesComptableAndCis(datesComptableAndCis.Select(x => x.DatesClotureComptable));

            Save();

            return datesComptableAndCis.Select(x => x.DatesClotureComptable);
        }

        private static void ProcessDateClotureComptable(DateTime date, int annee, int mois, List<CiDateClotureComptableNavigableEnt> datesComptableAndCis, int currentUserId, int datesHistorisees)
        {
            foreach (CiDateClotureComptableNavigableEnt datesComptableAndCi in datesComptableAndCis)
            {
                bool isNew = datesComptableAndCi.DatesClotureComptable == null;
                DateTime? dateTransfertFar = datesComptableAndCi.DatesClotureComptable?.DateTransfertFAR;

                datesComptableAndCi.DatesClotureComptable = new DatesClotureComptableEnt();
                if (currentUserId != 0)
                {
                    datesComptableAndCi.DatesClotureComptable.AuteurModificationId = currentUserId;
                }

                datesComptableAndCi.DatesClotureComptable.DateModification = date;
                datesComptableAndCi.DatesClotureComptable.Annee = annee;
                datesComptableAndCi.DatesClotureComptable.DateTransfertFAR = dateTransfertFar;
                datesComptableAndCi.DatesClotureComptable.Mois = mois;
                if (isNew)
                {
                    if (datesComptableAndCi.DatesClotureComptable.CI != null)
                    {
                        throw new FredBusinessException("abnormal CloturerQueryDatesClotureComptable : CI must be null");
                    }

                    if (currentUserId != 0)
                    {
                        datesComptableAndCi.DatesClotureComptable.AuteurCreationId = currentUserId;
                    }

                    datesComptableAndCi.DatesClotureComptable.DateCreation = date;
                    datesComptableAndCi.DatesClotureComptable.CiId = datesComptableAndCi.Ci.CiId;
                }
                else
                {
                    datesComptableAndCi.DatesClotureComptable.CiId = datesComptableAndCi.Ci.CiId;
                }
            }
            if (datesHistorisees > datesComptableAndCis.Count)
            {
                throw new FredBusinessException($"{datesComptableAndCis.Count} != {datesHistorisees} : le nombre de dates insérées ne correspond pas au nombre de dates historisées");
            }
        }

        /// <summary>
        /// InsertDateComptableAndCi
        /// </summary>
        /// <param name="datesComptableAndCis">datesComptableAndCis</param>
        private void AddRangeDatesComptableAndCis(IEnumerable<DatesClotureComptableEnt> datesComptableAndCis)
        {
            Repository.AddRange(datesComptableAndCis);
        }

        /// <summary>
        /// UpdateHistorisationDatesComptableAndCisWithDateTimeNow
        /// </summary>
        /// <param name="date">date</param>
        /// <param name="updateQueryableDatesClotureComptable">updateQueryableDatesClotureComptable</param>
        /// <param name="currentUserId">currentUserId</param>
        /// <returns>Nombre de dates historisées</returns>
        private int UpdateHistorisationDatesComptableAndCisWithDateTimeNow(DateTime date, IQueryable<DatesClotureComptableEnt> updateQueryableDatesClotureComptable, int currentUserId)
        {
            List<DatesClotureComptableEnt> reponseEnMemoire = updateQueryableDatesClotureComptable.ToList();
            int count = 0;
            foreach (DatesClotureComptableEnt r in reponseEnMemoire)
            {
                if (r.Historique)
                {
                    // déjà historisé, on continue
                    continue;
                }
                if (r.DateCloture != null)
                {
                    // déjà clôturé, on continue
                    continue;
                }

                if (currentUserId != 0)
                {
                    r.AuteurModificationId = currentUserId;
                }
                r.DateModification = date;
                r.DateCloture = date;
                r.Historique = true;
                Repository.Update(r);
                count++;
            }
            return count;
        }

        /// <summary>
        /// UpdateHistorisationDatesComptableAndCis 
        /// </summary>
        /// <param name="date">date</param>
        /// <param name="updateQueryableDatesClotureComptable">updateQueryableDatesClotureComptable</param>
        /// <param name="currentUserId">currentUserId</param>
        /// <returns>Nombre de DatesClotureComptableEnt historisées</returns>
        private int UpdateDatesComptableAndCisDateClotureWithDateTimeNull(DateTime date, IQueryable<DatesClotureComptableEnt> updateQueryableDatesClotureComptable, int currentUserId)
        {
            int count = 0;
            foreach (DatesClotureComptableEnt r in updateQueryableDatesClotureComptable.ToList())
            {
                if (r.Historique)
                {
                    // déjà historisé, on continue
                    continue;
                }
                if (r.DateCloture == null)
                {
                    // déjà déclôturé, on continue
                    continue;
                }

                if (currentUserId != 0)
                {
                    r.AuteurModificationId = currentUserId;
                }

                r.DateModification = date;
                r.DateCloture = null;
                r.Historique = true;
                Repository.Update(r);
                count++;
            }
            return count;
        }

        /// <summary>
        /// Retourner la liste des dates de clôture comptable
        /// </summary>
        /// <param name="searchCloturesPeriodesForCiEnt">filtre</param>
        /// <param name="identifiantsCiJoints">Sur tous sauf sur les identifiants ci-joints centre imputation</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        private IQueryable<CiDateClotureComptableNavigableEnt> QueryDatesClotureComptableExceptCiJoints(SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt, List<int> identifiantsCiJoints)
        {
            IQueryable<CiDateClotureComptableNavigableEnt> queryDatesClotureComptable = GetDatesClotureComptableNavigable(searchCloturesPeriodesForCiEnt);
            IQueryable<CiDateClotureComptableNavigableEnt> test = from dccCi in queryDatesClotureComptable
                                                                  join ici in identifiantsCiJoints on dccCi.Ci.CiId equals ici into icis
                                                                  from i in icis.DefaultIfEmpty()
                                                                  select dccCi;
            return test;
        }

        /// <summary>
        /// Retourner la liste des dates de clôture comptable qui ont une date de cloture et qui correspondent à une liste de CI
        /// </summary>
        /// <param name="searchCloturesPeriodesForCiEnt">filtre</param>
        /// <param name="identifiantsCiJoints">Sur tous sauf sur les identifiants ci-joints centre imputation</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        private IQueryable<CiDateClotureComptableNavigableEnt> QueryDatesClotureComptableWithDateClotureOnlyCiJoints(SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt, List<int> identifiantsCiJoints)
        {
            IQueryable<CiDateClotureComptableNavigableEnt> queryDatesClotureComptable = GetDatesClotureComptableNavigable(searchCloturesPeriodesForCiEnt);
            return from dccCi in queryDatesClotureComptable
                   join ici in identifiantsCiJoints on dccCi.Ci.CiId equals ici into icis
                   from i in icis.DefaultIfEmpty()
                   where i != 0 && (dccCi.DatesClotureComptable != null && dccCi.DatesClotureComptable.DateCloture != null)
                   select dccCi;
        }

        /// <summary>
        /// Retourner la liste des dates de clôture comptable qui n'ont pas de date de cloture et qui correspondent à une liste de CI
        /// </summary>
        /// <param name="searchCloturesPeriodesForCiEnt">filtre</param>
        /// <param name="identifiantsCiJoints">Sur tous sauf sur les identifiants ci-joints centre imputation</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        private IQueryable<CiDateClotureComptableNavigableEnt> QueryDatesClotureComptableWithoutDateClotureOnlyCiJoints(SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt, List<int> identifiantsCiJoints)
        {
            IQueryable<CiDateClotureComptableNavigableEnt> queryDatesClotureComptable = GetDatesClotureComptableNavigable(searchCloturesPeriodesForCiEnt);
            return from dccCi in queryDatesClotureComptable
                   join ici in identifiantsCiJoints on dccCi.Ci.CiId equals ici into icis
                   from i in icis.DefaultIfEmpty()
                   where i != 0 && (dccCi.DatesClotureComptable == null || dccCi.DatesClotureComptable.DateCloture == null)
                   select dccCi;
        }

        /// <summary>
        /// Retourner la liste des dates de clôture comptable
        /// </summary>
        /// <param name="searchCloturesPeriodesForCiEnt">filtre</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        private IQueryable<CiDateClotureComptableNavigableEnt> GetDatesClotureComptableNavigable(SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt)
        {
            UtilisateurEnt currentUser = utilisateurManager.GetContextUtilisateur();
            List<int> userCiIds = utilisateurManager.GetAllCIbyUser(currentUser.UtilisateurId).ToList();
            DateTime date = new DateTime(searchCloturesPeriodesForCiEnt.Year, searchCloturesPeriodesForCiEnt.Month, 15);
            MonthLimits monthLimits = date.GetLimitsOfMonth();
            bool noMonthLimits = searchCloturesPeriodesForCiEnt.DejaTermine == "true";

            var results = (from ci in ciRepository.Query().Get().Include(o => o.Organisation)
                           join d in Repository.Get().Where(dci => dci.Annee == searchCloturesPeriodesForCiEnt.Year && dci.Mois == searchCloturesPeriodesForCiEnt.Month && !dci.Historique)
                           on ci.CiId equals d.CiId into dcis
                           from dci in dcis.DefaultIfEmpty()
                           where ci.ChantierFRED && (ci.DateFermeture >= monthLimits.StartDate || ci.DateFermeture == null || noMonthLimits)
                           select new { ci, dci });
            return results
                           .Select(x => new CiDateClotureComptableNavigableEnt
                           {
                               Ci = x.ci,
                               DatesClotureComptable = x.dci,
                           }).Where(searchCloturesPeriodesForCiEnt.FiltreNavigableParCentreImputation())
                           .Where(searchCloturesPeriodesForCiEnt.FiltreNavigableParOrganisation())
                           .Where(searchCloturesPeriodesForCiEnt.FiltreNavigableParTransfertFar())
                           .Where(searchCloturesPeriodesForCiEnt.FiltreNavigableParClotureDepenses())
                           .Where(x => userCiIds.Contains(x.Ci.CiId));
        }

        /// <summary>
        /// Retourner la liste des dates de clôture comptable
        /// </summary>
        /// <param name="searchCloturesPeriodesForCiEnt">filtre</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        private IQueryable<CiDateClotureComptableDto> GetDatesClotureComptableFiltrable(SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt)
        {
            UtilisateurEnt currentUser = utilisateurManager.GetContextUtilisateur();
            List<int> userCiIds = utilisateurManager.GetAllCIbyUser(currentUser.UtilisateurId).ToList();
            DateTime date = new DateTime(searchCloturesPeriodesForCiEnt.Year, searchCloturesPeriodesForCiEnt.Month, 15);
            MonthLimits monthLimits = date.GetLimitsOfMonth();
            bool noMonthLimits = searchCloturesPeriodesForCiEnt.DejaTermine == "true";

            return (from ci in ciRepository.Query().Get().Include(o => o.Organisation)
                    join d in Repository.Get().Where(dci => dci.Annee == searchCloturesPeriodesForCiEnt.Year
                                                                                                                && ((searchCloturesPeriodesForCiEnt.DejaTermine == "true" && dci.Mois >= searchCloturesPeriodesForCiEnt.Month) || dci.Mois == searchCloturesPeriodesForCiEnt.Month)
                                                                                                                && !dci.Historique)
                    on ci.CiId equals d.CiId into dcis
                    from dci in dcis.DefaultIfEmpty()
                    where ci.ChantierFRED && (ci.DateFermeture >= monthLimits.StartDate || ci.DateFermeture == null || noMonthLimits)
                    select new CiDateClotureComptableDto
                    {
                        CiId = ci.CiId,
                        Code = ci.Code,
                        DateOuverture = dci.CI.DateOuverture,
                        DateFermeture = dci.CI.DateFermeture,
                        DateCloture = dci.DateCloture,
                        DatesClotureComptableId = dci.DatesClotureComptableId,
                        DateTransfertFAR = dci.DateTransfertFAR,
                        Libelle = ci.Libelle,
                        OrganisationId = ci.Organisation.OrganisationId,
                        PereId = ci.Organisation.PereId,
                        TypeOrganisationId = ci.Organisation.TypeOrganisationId,
                        Societe = new SocieteDto { Code = ci.Societe.Code }
                    }).Where(searchCloturesPeriodesForCiEnt.FiltreParCentreImputation())
                             .Where(searchCloturesPeriodesForCiEnt.FiltreParOrganisation())
                             .Where(searchCloturesPeriodesForCiEnt.FiltreParTransfertFar())
                             .Where(searchCloturesPeriodesForCiEnt.FiltreParClotureDepenses())
                             .Where(x => userCiIds.Contains(x.CiId));
        }
    }
}