using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.DatesClotureComptable;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Common.Duplication
{
    /// <summary>
    /// Helper sur la duplication des rapport et des pointages
    /// </summary>
    public static class DuplicationTimeHelper
    {
        /// <summary>
        /// Genere les jour pour une periode
        /// </summary>
        /// <param name="startDate">Date de debut</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>Les jours sur la periode</returns>
        public static List<DateTime> GetAllDaysInPeriode(DateTime startDate, DateTime endDate)
        {
            var result = new List<DateTime>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                result.Add(date);
            }
            return result;
        }

        /// <summary>
        /// Extension de IDatesClotureComptableManager, Permet de savoir si le ci pour une date donnée est cloturé.
        /// </summary>
        /// <param name="datesClotureComptableManager">datesClotureComptableManager</param>
        /// <param name="ciId">le ci</param>
        /// <param name="dateClotures">les date de clotures qui servent a savoir si le ci est cloturer</param>
        /// <param name="duplicateDays">les jours concerné par la requettes</param>
        /// <returns>vrai si le ci est cloturé.</returns>
        public static bool HasDatesInClosedMonth(this IDatesClotureComptableManager datesClotureComptableManager, int ciId, IEnumerable<DatesClotureComptableEnt> dateClotures, List<DateTime> duplicateDays)
        {
            foreach (var duplicateDay in duplicateDays)
            {
                //info => 'datesClotureComptableManager.IsPeriodClosed' ne fait pas d'appel a la base
                if (datesClotureComptableManager.IsPeriodClosed(dateClotures, ciId, duplicateDay.Year, duplicateDay.Month))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Filtre une liste de jours sur les jours qui ne sont pas le week end
        /// </summary>
        /// <param name="allDays">les jours a filtrer</param>
        /// <returns>retourne les jours qui ne sont pas des jours de weekend</returns>
        public static List<DateTime> FiltersWeekEnd(List<DateTime> allDays)
        {
            var result = new List<DateTime>();
            foreach (var day in allDays)
            {
                if (!IsWeekEnd(day))
                {
                    result.Add(day);
                }
            }

            return result;
        }

        private static bool IsWeekEnd(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retourne vrai si un des jours est contenu dans un weekend
        /// </summary>
        /// <param name="allDays">Tous les jours a comparer</param>
        /// <returns>Retourne vrai si tous les jours sont contenu dans un weekend</returns>
        public static bool GetIfPeriodIsOnlyOnWeekendOrWeekEnd(List<DateTime> allDays)
        {
            return allDays.All(day => IsWeekEnd(day));
        }


        /// <summary>
        /// Determine l'etat de la duplication pour les contrat interim
        /// l'etat correspond a la difference que provoque le filtre sur les contrat interim et s'il provoque une reduction des jours a dupliquer.
        /// </summary>
        /// <param name="initialDays">jours avant filtre sur contrat interim</param>
        /// <param name="finishDays">jours apres filtre sur contrat interim</param>
        /// <param name="personnel">personnel</param>
        /// <returns>InterimaireDuplicationState</returns>
        public static InterimaireDuplicationState GetInterimaireDuplicationState(List<DateTime> initialDays, List<DateTime> finishDays, PersonnelEnt personnel, List<ContratInterimaireEnt> contratInterimaires)
        {

            if (personnel == null)
            {
                return InterimaireDuplicationState.NoApplicable;
            }

            if (!personnel.IsInterimaire)
            {
                return InterimaireDuplicationState.NoApplicable;
            }

            var startDays = initialDays.Distinct().ToList();

            var endDays = finishDays.Distinct().ToList();


            if (startDays.Count > 0 && startDays.Count == endDays.Count)
            {
                return InterimaireDuplicationState.AllDaysDuplicate;
            }
            List<ZoneDeTravailEnt> ZonesDeTravail = new List<ZoneDeTravailEnt>();
            contratInterimaires?.ForEach(c => ZonesDeTravail.AddRange(c.ZonesDeTravail));
            bool hasDifferentZoneDeTravail = ZonesDeTravail.Select(z => z.EtablissementComptableId).Distinct().Count() > 1;

            if (startDays.Count > 0 && startDays.Count >= endDays.Count && endDays.Count > 0)
            {
                if (hasDifferentZoneDeTravail)
                {
                    return InterimaireDuplicationState.PartialDuplicationInDifferentZoneDeTravail;
                }
                else
                {
                    return InterimaireDuplicationState.PartialDuplicate;
                }
            }

            if (startDays.Count > 0 && endDays.Count == 0)
            {
                if (hasDifferentZoneDeTravail)
                {
                    return InterimaireDuplicationState.AllDuplicationInDifferentZoneDeTravail;
                }
                else
                {
                    return InterimaireDuplicationState.NothingDayDuplicate;
                }
            }

            return InterimaireDuplicationState.AllDaysDuplicate;
        }
    }
}
