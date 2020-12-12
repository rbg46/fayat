using System;
using System.Globalization;
using DateTimeExtensions;
using DateTimeExtensions.WorkingDays;

namespace Fred.Framework.DateTimeExtend
{
    /// <summary>
    /// Classe d'extension de DateTime
    /// </summary>
    public static class DateTimeExt
    {
        /// <summary>
        /// Retourne les limites d'un mois
        /// </summary>
        /// <param name="month">date reference</param>
        /// <returns>MonthLimits</returns>
        public static MonthLimits GetLimitsOfMonth(this DateTime month)
        {
            DateTime firstDayOfMonth = GetFirstDayOfMonth(month);
            DateTime lastDayOfMonth = GetLastDayOfMonth(month);

            return new MonthLimits()
            {
                StartDate = firstDayOfMonth,
                EndDate = lastDayOfMonth,
            };
        }

        public static DateTime GetFirstDayOfMonth(this DateTime month)
        {
            return new DateTime(month.Year, month.Month, 1, 0, 0, 0);
        }

        public static DateTime GetLastDayOfMonth(this DateTime month)
        {
            return GetFirstDayOfMonth(month).AddMonths(1).AddSeconds(-1);
        }

        /// <summary>
        ///   Teste si la date passée en paramètre est un jour ouvré
        /// </summary>
        /// <param name="date">Date à tester</param>
        /// <param name="culture">culture exemple :fr-FR</param>
        /// <returns>Vrai si jour ouvré, sinon faux</returns>
        public static bool IsBusinessDay(this DateTime date, string culture = "fr-FR")
        {
            var cultureInfo = new CultureInfo(culture);
            IWorkingDayCultureInfo cultureInfos = new WorkingDayCultureInfo(cultureInfo.Name);
            return date.IsWorkingDay(cultureInfos);
        }

        /// <summary>
        /// Retounrne la différence en heure entre la premiére et la deuxiéme heure
        /// </summary>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>Différence en heure entre startDate et endDate</returns>
        public static double GetHourDifference(this DateTime startDate, DateTime endDate)
        {
            TimeSpan total = startDate - endDate;
            return total.TotalHours;
        }
    }
}
