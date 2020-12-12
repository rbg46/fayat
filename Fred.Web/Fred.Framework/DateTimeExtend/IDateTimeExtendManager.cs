using System;
using System.Collections.Generic;
using System.Globalization;

namespace Fred.Framework.DateTimeExtend
{
    /// <summary>
    ///   La classe IDateTimeExtendManager sert à lire une valeur dans le fichier de configuration
    /// </summary>
    public interface IDateTimeExtendManager
    {
        /// <summary>
        /// Teste la date passé en paramètre, et évalue s'il s'agit d'un jour férié
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>Retourne vrai si la date est un jour férié</returns>
        bool IsHoliday(DateTime date);

        /// <summary>Obtient une valeur dans le fichier de configuration </summary>    
        /// <param name="periode">p</param>
        /// <param name="cultureInfo">c</param>
        /// <param name="isWeekPeriode">isWekPeriode</param>
        /// <returns>Liste de date</returns>
        List<DateTimeExtend> TestHoliday(DateTime periode, CultureInfo cultureInfo, bool isWeekPeriode);

        /// <summary>
        /// Retourne le premier jour de la période
        /// </summary>
        /// <param name="year">Année</param>
        /// <param name="month">Mois</param>
        /// <returns>Premier jour du mois</returns>
        DateTime GetFirstDayOnPeriod(int year, int month);

        /// <summary>
        /// Retourne le dernier jour de la période
        /// </summary>
        /// <param name="year">Année</param>
        /// <param name="month">Mois</param>
        /// <returns>Dernier jour du mois</returns>
        DateTime GetLastDayOnPeriod(int year, int month);

        /// <summary>
        ///   Teste si la date passée en paramètre est un jour ouvré (!samedi, !dimanche, !férié)
        /// </summary>
        /// <param name="date">Date à tester</param>
        /// <returns>Vrai si jour ouvré, sinon faux</returns>
        bool IsBusinessDay(DateTime date);

        /// <summary>
        ///   Obtient le nombre de jours non ouvrés dans une période
        /// </summary>
        /// <param name="start">Date de début</param>
        /// <param name="end">Date de fin</param>
        /// <returns>Nombre de jours non ouvrés</returns>
        int CountHolidaysAndWeekends(DateTime start, DateTime end);

        /// <summary>
        ///   Teste si la date passée en paramètre est un jour de weekend
        /// </summary>
        /// <param name="date">Date à tester</param>
        /// <returns>Vrai si jour de weekend, sinon faux</returns>
        bool IsWeekend(DateTime date);

        /// <summary>
        ///   Récupère la liste des jours fériés d'une année ou de l'année en cours et l'année suivante
        /// </summary>
        /// <param name="year">Année</param>
        /// <returns>Liste des jours fériés</returns>
        IEnumerable<DateTime> GetHolidays(int? year);

        /// <summary>
        /// Retourner la liste des jours ouvrés entre 2 dates 
        /// </summary>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <param name="isHolidayCheck">True si on considére la vérification des jours fériés assuré par la méthode IsHoliday. False par défaut</param>
        /// <returns>List of DateTime</returns>
        IEnumerable<DateTime> GetWorkingDays(DateTime startDate, DateTime endDate, bool isHolidayCheck = false);
    }
}
