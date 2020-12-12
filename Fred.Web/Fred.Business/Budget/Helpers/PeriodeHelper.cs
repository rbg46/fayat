using System;
using System.Globalization;
using System.Linq;

namespace Fred.Business.Budget.Helpers
{
    /// <summary>
    /// Helper pour les période au format entier.
    /// </summary>
    public static class PeriodeHelper
    {
        /// <summary>
        /// Retourne la période courante
        /// </summary>
        /// <returns>La période courante</returns>
        public static int GetPeriode()
        {
            var today = DateTime.Today;
            return (today.Year * 100) + today.Month;
        }

        /// <summary>
        /// Retourne la période courante
        /// </summary>
        /// <param name="periodeDateTime">Période au format DateTime</param>
        /// <returns>La période courante</returns>
        public static int GetPeriode(DateTime periodeDateTime)
        {
            return (periodeDateTime.Year * 100) + periodeDateTime.Month;
        }

        /// <summary>
        /// Retourne la période précédente
        /// </summary>
        /// <param name="periode">Période courante</param>
        /// <returns>La période précédente</returns>
        public static int? GetNextPeriod(int periode)
        {
            if (periode.ToString().Length == 6)
            {
                var month = periode % 100;
                if (month == 12)
                {
                    return periode + 89;
                }
                else
                {
                    return periode + 1;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retourne la période précédente
        /// </summary>
        /// <param name="periode">Période courante</param>
        /// <returns>La période précédente</returns>
        public static int? GetPreviousPeriod(int periode)
        {
            if (periode.ToString().Length == 6)
            {
                var month = periode % 100;
                if (month == 1)
                {
                    return periode - 89;
                }
                else
                {
                    return periode - 1;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retourne un objet DateTime à partir de la période passée paramètre.
        /// Le jour de ce datetime sera le dernier jour du mois de la période. La composante horaire 
        /// du date time est laissée vide
        /// </summary>
        /// <param name="periode">Periode au format YYYYMM</param>
        /// <returns>un objet datetime, null si la periode n'est au bon format</returns>
        public static DateTime? ToFirstDayOfMonthDateTime(int periode)
        {
            if (periode.ToString().Length == 6)
            {
                int month = periode % 100;
                int year = periode / 100;

                return new DateTime(year, month, 1);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retourne un objet DateTime à partir de la période passée paramètre.
        /// Le jour de ce datetime sera le dernier jour du mois de la période. La composante horaire 
        /// du date time est laissée vide
        /// </summary>
        /// <param name="periode">Periode au format YYYYMM</param>
        /// <returns>un objet datetime, null si la periode n'est au bon format</returns>
        public static DateTime? ToLastDayOfMonthDateTime(int periode)
        {
            if (periode.ToString().Length == 6)
            {
                int month = periode % 100;
                int year = periode / 100;

                int day = DateTime.DaysInMonth(year, month);

                return new DateTime(year, month, day);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retourne une periode au format Mois Année : le mois est représenté par son nom (Janvier, Févrirer...) et l'année est sous forme de chiffre (2017,2018)
        /// </summary>
        /// <param name="periode">une peridoe au format YYYYMM</param>
        /// <returns>une chaine de caractère, null si la période donnée n'est pas au bon format</returns>
        public static string GetLiteralPeriode(int periode)
        {
            if (periode.ToString().Length == 6)
            {
                var dateTime = ToFirstDayOfMonthDateTime(periode).Value;
                var month = dateTime.ToString("MMMM", CultureInfo.GetCultureInfo("fr-FR"));
                return month + " " + dateTime.Year.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Cette fonction retourne true si la date est dans la période passée en paramètre
        /// La composante jour de la date est ignorée
        /// </summary>
        /// <example>
        /// IsDateTimeInPeriode( new DateTime(2019,01,01), 201901) -> retourne true
        /// IsDateTimeInPeriode( new DateTime(2019,02,01), 201901) -> retourne false
        /// </example>
        /// <param name="date">Date à examiner, le jour est ignoré</param>
        /// <param name="yyyymmPeriode">période à analyser : doit être au format YYYYMM</param>
        /// <returns>un booleen</returns>
        public static bool IsDateTimeInPeriode(DateTime date, int yyyymmPeriode)
        {
            var month = yyyymmPeriode % 100;
            var year = yyyymmPeriode / 100;

            return date.Year == year && date.Month == month;
        }

        /// <summary>
        /// Retourne la période au format Mois (litéral) et année (ex: Juin 2018)
        /// </summary>
        /// <param name="periode">La période concernée au format 201806.</param>
        /// <returns>La période formatée.</returns>
        public static string FormatCulture(int periode)
        {
            int month = periode % 100;
            int year = periode / 100;

            // Récupère le nom du mois en fonction de la culture
            var monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);

            // Met la première lettre en majuscule
            monthName = monthName.First().ToString().ToUpper() + monthName.Substring(1);

            // Formate la date
            var stringPeriode = monthName + " " + year;
            return stringPeriode;
        }

        /// <summary>
        /// Retourne une période formatée MM/YYYY
        /// </summary>
        /// <param name="periode">periode numérique</param>
        /// <returns>période formatée</returns>
        public static string FormatPeriode(int periode)
        {
            var month = periode % 100;
            var year = periode / 100;

            return $"{month.ToString().PadLeft(2, '0')}/{year.ToString()}";
        }
    }
}
