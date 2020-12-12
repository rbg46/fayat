using DateTimeExtensions;
using DateTimeExtensions.WorkingDays;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Fred.Framework.DateTimeExtend
{
    /// <summary>
    /// Manager de DateTimeExtend
    /// </summary>
    public class DateTimeExtendManager : IDateTimeExtendManager
    {
        /// <summary>
        /// Teste la date passé en paramètre, et évalue s'il s'agit d'un jour férié
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>Retourne vrai si la date est un jour férié</returns>
        public bool IsHoliday(DateTime date)
        {
            var cultureInfo = new CultureInfo("fr-FR");
            IWorkingDayCultureInfo cultureInfos = new WorkingDayCultureInfo(cultureInfo.Name);
            return date.IsHoliday(cultureInfos);
        }

        /// <summary>
        ///   Teste si la date passée en paramètre est un jour ouvré
        /// </summary>
        /// <param name="date">Date à tester</param>
        /// <returns>Vrai si jour ouvré, sinon faux</returns>
        public bool IsBusinessDay(DateTime date)
        {
            var cultureInfo = new CultureInfo("fr-FR");
            IWorkingDayCultureInfo cultureInfos = new WorkingDayCultureInfo(cultureInfo.Name);
            return date.IsWorkingDay(cultureInfos);
        }

        /// <summary>
        ///   Teste si la date passée en paramètre est un jour de weekend
        /// </summary>
        /// <param name="date">Date à tester</param>
        /// <returns>Vrai si jour de weekend, sinon faux</returns>
        public bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        ///   Obtient le nombre de jours non ouvrés dans une période
        /// </summary>
        /// <param name="start">Date de début</param>
        /// <param name="end">Date de fin</param>
        /// <returns>Nombre de jours non ouvrés</returns>
        public int CountHolidaysAndWeekends(DateTime start, DateTime end)
        {
            int cpt = 0;

            for (var date = start; date.Date <= end.Date; date = date.AddDays(1))
            {
                if (!IsBusinessDay(date))
                {
                    cpt++;
                }
            }

            return cpt;
        }

        /// <summary>
        ///   Récupère la liste des jours fériés d'une année ou de l'année en cours et l'année suivante
        /// </summary>
        /// <param name="year">Année</param>
        /// <returns>Liste des jours fériés</returns>
        public IEnumerable<DateTime> GetHolidays(int? year)
        {
            List<DateTime> holidays = new List<DateTime>();
            DateTime start, end;

            if (year.HasValue)
            {
                start = new DateTime(year.Value, 1, 1);
                end = new DateTime(year.Value, 12, 31);

                for (var date = start; date.Date <= end.Date; date = date.AddDays(1))
                {
                    if (!IsBusinessDay(date) && !IsWeekend(date))
                    {
                        holidays.Add(date);
                    }
                }
            }
            else
            {
                start = new DateTime(DateTime.Now.Year, 1, 1);
                end = new DateTime(DateTime.Now.AddYears(1).Year, 12, 31);

                for (var date = start; date.Date <= end.Date; date = date.AddDays(1))
                {
                    if (!IsBusinessDay(date) && !IsWeekend(date))
                    {
                        holidays.Add(date);
                    }
                }
            }

            return holidays;
        }

        /// <summary>
        /// Calcul les jours fériés et de weekend pour une période et une culture passé en paramètre
        /// </summary>
        /// <param name="periode">Période mensuel</param>
        /// <param name="cultureInfo">Culture</param>
        /// <param name="isWeekPeriode">isWeekPeriode</param>
        /// <returns>Retourne la liste des jours de la période passé en paramètres</returns>
        public List<DateTimeExtend> TestHoliday(DateTime periode, CultureInfo cultureInfo, bool isWeekPeriode)
        {
            IWorkingDayCultureInfo cultureInfos = new WorkingDayCultureInfo(cultureInfo.Name);
            var listDaysExtend = new List<DateTimeExtend>();
            List<DateTime> dates = (isWeekPeriode) ? DaysInWeek(periode) : GetDates(periode.Year, periode.Month);
            foreach (DateTime date in dates)
            {
                var dateExtend = new DateTimeExtend();
                dateExtend.Date = date;
                dateExtend.IsHoliday = date.IsHoliday(cultureInfos);
                dateExtend.IsWeekend = !date.IsWorkingDay(cultureInfos);
                listDaysExtend.Add(dateExtend);
            }

            return listDaysExtend;
        }

        /// <inheritdoc />
        public DateTime GetFirstDayOnPeriod(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                       .Select(day => new DateTime(year, month, day))
                       .First();
        }

        /// <inheritdoc />
        public DateTime GetLastDayOnPeriod(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                       .Select(day => new DateTime(year, month, day))
                       .Last();
        }
        private List<DateTime> GetDates(int year, int month)
        {

            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                             .Select(day => new DateTime(year, month, day))
                             .ToList();
        }

        private List<DateTime> DaysInWeek(DateTime periode)
        {
            DateTime today = periode;
            int currentDayOfWeek = (int)today.DayOfWeek;
            DateTime sunday = today.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);
            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();
            return dates;
        }

        /// <summary>
        /// Retourner la liste des jours ouvrés entre 2 dates 
        /// </summary>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <param name="isHolidayCheck">True si on considére la vérification des jours fériés assuré par la méthode IsHoliday. False par défaut</param>
        /// <returns>List of DateTime</returns>
        public IEnumerable<DateTime> GetWorkingDays(DateTime startDate, DateTime endDate, bool isHolidayCheck = false)
        {
            List<DateTime> result = new List<DateTime>();
            DateTimeExtendManager dateTimeExtendManager = new DateTimeExtendManager();

            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                bool isWorkingDay;
                if (!isHolidayCheck)
                {
                    isWorkingDay = !dateTimeExtendManager.IsWeekend(date);
                }
                else
                {
                    isWorkingDay = !dateTimeExtendManager.IsWeekend(date) && !dateTimeExtendManager.IsHoliday(date);
                }

                if (isWorkingDay)
                {
                    result.Add(date.Date);
                }
            }

            return result;
        }
    }
}
