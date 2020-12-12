using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.DatesClotureComptable;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.DatesClotureComptable;

namespace Fred.Business.DatesClotureComptable
{
    /// <summary>
    ///   Gestionnaire des DatesClotureComptable.
    /// </summary>
    public class DatesClotureComptableManager : Manager<DatesClotureComptableEnt, IDatesClotureComptableRepository>, IDatesClotureComptableManager
    {
        private readonly IUtilisateurManager utilisateurManager;


        public DatesClotureComptableManager(IUnitOfWork uow, IDatesClotureComptableRepository datesClotureComptableRepository, IUtilisateurManager utilisateurManager)
          : base(uow, datesClotureComptableRepository)
        {
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        ///   Retourne une liste de DatesClotureComptableEnt par ci et année
        /// </summary>
        /// <param name="ciId">Le ci</param>
        /// <param name="year">Une année</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        public IEnumerable<DatesClotureComptableEnt> GetCIListDatesClotureComptableByIdAndYear(int ciId, int year)
        {
            return Repository.GetCIListDatesClotureComptableByIdAndYear(ciId, year).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<DatesClotureComptableEnt> GetListDatesClotureComptableByCiGreaterThanPeriode(int ciId, int periodeDebut)
        {
            return Repository.GetListDatesClotureComptableByCiGreaterThanPeriode(ciId, periodeDebut).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<DatesClotureComptableEnt> GetListDatesClotureComptableByCiGreaterThanPeriode(int ciId, int month, int year)
        {
            return Repository.GetListDatesClotureComptableByCiGreaterThanPeriode(ciId, month, year).ToList();
        }

        /// <summary>
        ///   Retourne une liste de DatesClotureComptableEnt par CI, année et mois
        /// </summary>
        /// <param name="ciId">Un CI</param>
        /// <param name="year">Une année</param>
        /// <param name="month">Un mois</param>
        /// <returns>Renvoi un calendrier</returns>
        public DatesClotureComptableEnt Get(int ciId, int year, int month)
        {
            return Repository.Get(ciId, year, month);
        }

        /// <summary>
        ///   Renvoi vrai si aujourd'hui est dans la periode de clôture
        /// </summary>
        /// <param name="ciId">Le CI</param>
        /// <param name="year">L'année laquelle on souhaite faire le test de la cloture comptable</param>
        /// <param name="month">Le mois avec lequel on souhaite faire le test de la cloture comptable</param>
        /// <returns>Renvoi un booléen</returns>
        public bool IsTodayInPeriodeCloture(int ciId, int year, int month)
        {
            return IsPeriodClosed(ciId, year, month);
        }

        /// <summary>
        /// Permet de savoir si une periode est cloturée pour un CI.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="year">year</param>
        /// <param name="month">month</param>
        /// <param name="today">permet de passé la date 'aujourd'hui' qui sera comparée, utile pour les tests unitaires.</param>
        /// <returns>true si la periode est cloturée</returns>
        public bool IsPeriodClosed(int ciId, int year, int month, DateTime? today = null)
        {
            DatesClotureComptableEnt datesClotureComptable = Repository.Get(ciId, year, month);

            return DatesClotureComptableIsClosed(datesClotureComptable, today);
        }

        /// <summary>
        /// Permet de savoir si un CI a au moins une periode de cloturée dans un intervalle de temps
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <param name="today">permet de passé la date 'aujourd'hui' qui sera comparée, utile pour les tests unitaires.</param>
        /// <returns>true si la periode est cloturée</returns>
        public List<DatesClotureComptableEnt> GetClosedDatesClotureComptable(int ciId, DateTime startDate, DateTime endDate, DateTime? today = null)
        {
            List<DatesClotureComptableEnt> datesClotureComptable = Repository.Get(ciId, startDate, endDate);

            return GetClosedDatesClotureComptable(datesClotureComptable, today);
        }

        /// <summary>
        ///   Renvoi une liste de DatesClotureComptableEnt pour calculer si la date est cloturée
        /// </summary>
        /// <param name="ciIds">Les CiIds</param>
        /// <param name="year">L'année laquelle on souhaite faire le test de la cloture comptable</param>
        /// <param name="month">Le mois avec lequel on souhaite faire le test de la cloture comptable</param>
        /// <returns>Une liste de DatesClotureComptableEnt</returns>
        public IEnumerable<DatesClotureComptableEnt> GetDatesClotureComptableForCiIds(List<int> ciIds, int year, int month)
        {
            var datesClotureComptables = Repository.Get(ciIds, year, month);

            return datesClotureComptables;
        }

        /// <summary>
        ///   Renvoie un DatesClotureComptableEnt pour calculer si la date est cloturée
        /// </summary>
        /// <param name="ciId"> CiId</param>
        /// <param name="year">L'année laquelle on souhaite faire le test de la cloture comptable</param>
        /// <param name="month">Le mois avec lequel on souhaite faire le test de la cloture comptable</param>
        /// <returns>Une liste de DatesClotureComptableEnt</returns>
        public DatesClotureComptableEnt GetDatesClotureComptableForCiId(int ciId, int year, int month)
        {
            var datesClotureComptable = Repository.Get(ciId, year, month);

            return datesClotureComptable;
        }

        /// <summary>
        /// Retourne une liste de <see cref="DatesClotureComptableEnt"/> pour une liste de <see cref="DateClotureComptableWithOptionModel"/>
        /// </summary>
        /// <param name="dateClotureComptableWithOptionModels">Liste de <see cref="DateClotureComptableWithOptionModel"/></param>
        /// <returns>Liste de <see cref="DatesClotureComptableEnt"/></returns>
        public List<DatesClotureComptableEnt> GetDatesClotureComptableForCiId(List<DateClotureComptableWithOptionModel> dateClotureComptableWithOptionModels)
        {
            List<DatesClotureComptableEnt> list = new List<DatesClotureComptableEnt>();

            foreach (DateClotureComptableWithOptionModel item in dateClotureComptableWithOptionModels)
            {
                list.Add(Repository.Get(item.CiId, item.Annee, item.Mois));
            }

            return list;
        }

        /// <summary>
        /// Permet de savoir si une periode est cloturée pour un CI a partir d'une liste de DatesClotureComptableEnt
        /// </summary>
        /// <param name="datesClotureComptables">datesClotureComptables</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="year">year</param>
        /// <param name="month">month</param>       
        /// <returns>true si la periode est cloturée</returns>
        public bool IsPeriodClosed(IEnumerable<DatesClotureComptableEnt> datesClotureComptables, int ciId, int year, int month)
        {
            var datesClotureComptableForCiAndPeriod = datesClotureComptables.FirstOrDefault(dcc => dcc.CiId == ciId && dcc.Annee == year && dcc.Mois == month);

            return DatesClotureComptableIsClosed(datesClotureComptableForCiAndPeriod);
        }

        private List<DatesClotureComptableEnt> GetClosedDatesClotureComptable(List<DatesClotureComptableEnt> datesClotureComptables, DateTime? today = null)
        {
            List<DatesClotureComptableEnt> listdatesClotureComptablesClosed = new List<DatesClotureComptableEnt>();
            foreach (var datesClotureComptable in datesClotureComptables)
            {
                if (datesClotureComptable?.DateCloture.HasValue == true)
                {
                    DateTime currentMonthClosureDate = new DateTime(datesClotureComptable.DateCloture.Value.Year, datesClotureComptable.DateCloture.Value.Month, datesClotureComptable.DateCloture.Value.Day);
                    DateTime now = today ?? DateTime.Today;
                    if (now >= currentMonthClosureDate)
                    {
                        listdatesClotureComptablesClosed.Add(datesClotureComptable);
                    }
                }
            }
            return listdatesClotureComptablesClosed.OrderBy(x => x.Annee).ThenBy(x => x.Mois).ToList();
        }

        private bool DatesClotureComptableIsClosed(DatesClotureComptableEnt datesClotureComptable, DateTime? today = null)
        {
            if (datesClotureComptable?.DateCloture.HasValue == true)
            {
                DateTime currentMonthClosureDate = new DateTime(datesClotureComptable.DateCloture.Value.Year, datesClotureComptable.DateCloture.Value.Month, datesClotureComptable.DateCloture.Value.Day);
                DateTime now = today ?? DateTime.Today;
                return now >= currentMonthClosureDate;
            }

            return false;
        }

        /// <summary>
        ///   Détermine si le CI est bloqué en réception pour une période donnée
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="year">Année</param>
        /// <param name="month">Mois</param>
        /// <returns>Vrai si le CI est bloqué en réception, sinon faux</returns>
        public bool IsBlockedInReception(int ciId, int year, int month)
        {
            var dcc = Repository.Get(ciId, year, month);
            return dcc != null && (dcc.DateCloture.HasValue || dcc.DateTransfertFAR.HasValue);
        }

        /// <summary>
        ///   Récupère la période non bloquée en réception pour un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="currentDate">Date courante</param>
        /// <returns>Période non bloquée</returns>
        public DateTime GetNextUnblockedInReceptionPeriod(int ciId, DateTime currentDate)
        {
            while (IsBlockedInReception(ciId, currentDate.Year, currentDate.Month))
            {
                currentDate = currentDate.AddMonths(1);
            }
            return new DateTime(currentDate.Year, currentDate.Month, 1);
        }

        /// <summary>
        /// Retourne une liste de PeriodeClotureEnt du mois precedent, courrant et futur dela journee courrante
        /// Permet de savoir si ces periode sont cloturées.
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <returns>Une liste de PeriodeClotureEnt</returns>
        public IEnumerable<PeriodeClotureEnt> GetPreviousCurrentAndNextMonths(int ciId)
        {
            var result = new List<PeriodeClotureEnt>();
            var now = DateTime.Today;
            DatesClotureComptableEnt previousMonth = null;
            DatesClotureComptableEnt currentMonth = null;
            DatesClotureComptableEnt nextMonth = null;
            int yearOfpreviousMonth = 0;
            int monthOfpreviousMonth = 0;
            int yearOfNextMonth = 0;
            int monthOfNextMonth = 0;

            if (now.Month != 1)
            {
                yearOfpreviousMonth = now.Year;
                monthOfpreviousMonth = now.Month - 1;
            }
            else
            {
                yearOfpreviousMonth = now.Year - 1;
                monthOfpreviousMonth = 12;
            }

            if (now.Month != 12)
            {
                yearOfNextMonth = now.Year;
                monthOfNextMonth = now.Month + 1;
            }
            else
            {
                yearOfNextMonth = now.Year + 1;
                monthOfNextMonth = 1;
            }

            previousMonth = Repository.GetCIDatesClotureComptableByIdAndYearAndMonthOrDefault(ciId, yearOfpreviousMonth, monthOfpreviousMonth);
            currentMonth = Repository.GetCIDatesClotureComptableByIdAndYearAndMonthOrDefault(ciId, now.Year, now.Month);
            nextMonth = Repository.GetCIDatesClotureComptableByIdAndYearAndMonthOrDefault(ciId, yearOfNextMonth, monthOfNextMonth);
            result.Add(MapToPeriodeClotureEnt(previousMonth));
            result.Add(MapToPeriodeClotureEnt(currentMonth));
            result.Add(MapToPeriodeClotureEnt(nextMonth));

            return result;
        }

        /// <summary>
        /// transforme une DatesClotureComptableEnt en PeriodeClotureEnt
        /// </summary>
        /// <param name="datesClotureComptableEnt">datesClotureComptableEnt</param>
        /// <returns>PeriodeClotureEnt</returns>
        private PeriodeClotureEnt MapToPeriodeClotureEnt(DatesClotureComptableEnt datesClotureComptableEnt)
        {
            return new PeriodeClotureEnt
            {
                Annee = datesClotureComptableEnt.Annee,
                Mois = datesClotureComptableEnt.Mois,
                IsClosed = GetDatesClotureComptableEntIsClosed(datesClotureComptableEnt)
            };
        }

        /// <summary>
        /// GetDatesClotureComptableEntIsClosed
        /// </summary>
        /// <param name="datesClotureComptableEnt">DatesClotureComptableEnt</param>
        /// <returns>true ou false</returns>
        private bool GetDatesClotureComptableEntIsClosed(DatesClotureComptableEnt datesClotureComptableEnt)
        {
            if (datesClotureComptableEnt?.DateCloture.HasValue == true)
            {
                int year = datesClotureComptableEnt.DateCloture.Value.Year;
                int month = datesClotureComptableEnt.DateCloture.Value.Month;
                int day = datesClotureComptableEnt.DateCloture.Value.Day;

                DateTime currentMonthClosureDate = new DateTime(year, month, day);
                return DateTime.Today >= currentMonthClosureDate;
            }

            return false;
        }

        /// <summary>
        /// Recupere l'annee, le mois precedant et suivant
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="year">year</param>
        /// <returns>List de DatesClotureComptableEnt</returns>
        public IEnumerable<DatesClotureComptableEnt> GetYearAndPreviousNextMonths(int ciId, int year)
        {
            return Repository.GetYearAndPreviousNextMonths(ciId, year).ToList();
        }

        /// <summary>
        ///   Ajoute un DatesClotureComptableEnt en base
        /// </summary>
        /// <param name="dcc">Une date clotures comptable</param>
        /// <returns>Renvoi l'id technique</returns>
        public DatesClotureComptableEnt CreateDatesClotureComptable(DatesClotureComptableEnt dcc)
        {
            var currentUserId = this.utilisateurManager.GetContextUtilisateurId();
            // Vérifier si la date cloture est déjà créé, si oui retourner l'existante
            // Cette verification est necessaire pour éviter la duplication de date cloture comptable
            DatesClotureComptableEnt existingDCC = Repository.Get(dcc.CiId, dcc.Annee, dcc.Mois);

            if (existingDCC == null)
            {
                return Repository.CreateDatesClotureComptable(dcc, currentUserId);
            }
            else
            {
                return existingDCC;
            }
        }

        /// <summary>
        ///   Ajoute un DatesClotureComptableEnt en base
        /// </summary>
        /// <param name="dcc">Une date clotures comptable</param>
        /// <returns>Renvoi l'id technique</returns>
        public DatesClotureComptableEnt ModifyDatesClotureComptable(DatesClotureComptableEnt dcc)
        {
            var currentUserId = this.utilisateurManager.GetContextUtilisateurId();
            return Repository.ModifyDatesClotureComptable(dcc, currentUserId);
        }

        /// <inheritdoc />
        public void BulkInsert(List<DatesClotureComptableEnt> datesClotureComptables)
        {
            try
            {
                Repository.InsertInMass(datesClotureComptables);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne l'ensemble des dates de clôture d'un CI.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI.</param>
        /// <returns>L'ensemble des dates de clôture du CI.</returns>
        public IEnumerable<DatesClotureComptableEnt> Get(int ciId)
        {
            return Repository.Get(ciId);
        }

        /// <summary>
        /// Retourne l'ensemble des dates de clôture.
        /// </summary>
        /// <returns>L'ensemble des dates de clôture.</returns>
        public IEnumerable<DatesClotureComptableEnt> GetAllSync()
        {
            return Repository.GetAllSync();
        }

        /// <summary>
        /// Retourne la dernière date de cloture pour un CI
        /// </summary>
        /// <param name="ciId">Identifiant unique d'un CI</param>
        /// <returns>La dernière date de cloture pour un ci</returns>
        public DateTime GetLastDateClotureByCiID(int ciId)
        {
            try
            {
                return Repository.GetLastDateClotureByCiID(ciId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public DateTime? GetDernierePeriodeComptableCloturee(int ciId)
        {
            DateTime? lastAccountingDateClose = null;

            DatesClotureComptableEnt derniereClotureComptableEnt = Get(ciId)
             .Where(dc => !dc.Historique && dc.DateCloture.HasValue)
             .OrderByDescending(dc => (dc.Annee * 100) + dc.Mois)
             .FirstOrDefault();

            if (derniereClotureComptableEnt != null)
            {
                try
                {
                    lastAccountingDateClose = new DateTime(derniereClotureComptableEnt.Annee, derniereClotureComptableEnt.Mois, 1);
                }
                catch (ArgumentOutOfRangeException)
                {
                    lastAccountingDateClose = null;
                }
            }
            return lastAccountingDateClose;
        }
    }
}
