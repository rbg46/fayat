using System;

namespace Fred.Framework.Extensions
{
	/// <summary>
	///   Extension du type <see cref="DateTime" />.
	/// </summary>
	public static class DateTimeExtensions
    {
	    /// <summary>
	    /// Retourne la période d'une date.
	    /// La date retournée sera le 1er jour du mois et de l'année de la date passée en paramètre.
	    /// </summary>
	    /// <param name="date">Date concernée</param>
	    /// <returns>La période de la date</returns>
	    public static DateTime GetPeriode(this DateTime date)
	    {
	      return new DateTime(date.Year, date.Month, 1);
	    }

        /// <summary>
        /// Transforme un intervalle de datetime en intervalle de date
        /// </summary>
        /// <param name="dateFrom">Date de début d'intervalle</param>
        /// <param name="dateTo">Date de fin d'intervalle</param>
	    public static void TransformDateTimeRangeToDateRange(ref DateTime? dateFrom, ref DateTime? dateTo)
	    {
		    if (dateFrom.HasValue)
		    {
			    dateFrom = dateFrom.Value.ToStartDate();
		    }
		    if (dateTo.HasValue)
		    {
			    dateTo = dateTo.Value.ToEndDate();
		    }
	    }

        /// <summary>
        /// Renvoie le datetime en tant que date de début de période (sans la partie time)
        /// </summary>
        /// <param name="startDateTime">Date de début</param>
        /// <returns>DateTime</returns>
	    public static DateTime ToStartDate(this DateTime startDateTime)
	    {
		    return startDateTime.Date;
	    }

        /// <summary>
        /// Renvoie le datetime en tant que date de fin de période (sans la partie time)
        /// </summary>
        /// <param name="endDateTime">Date de fin</param>
        /// <returns>DateTime</returns>
	    public static DateTime ToEndDate(this DateTime endDateTime)
	    {
		    return endDateTime.Date;
	    }
	    /// <summary>
	    /// Indique si la période de la date est identique à une autre.
	    /// </summary>
	    /// <param name="date">Date concernée</param>
	    /// <param name="other">Date à comparer</param>
	    /// <returns>True si la période de la date est identique à l'autre, sinon false</returns>
	    public static bool IsSamePeriode(this DateTime date, DateTime other)
	    {
	      return date.Year == other.Year && date.Month == other.Month;
	    }

	    /// <summary>
	    /// Indique si la période de la date est antérieure à une autre.
	    /// </summary>
	    /// <param name="date">Date concernée</param>
	    /// <param name="other">Date à comparer</param>
	    /// <returns>True si la période de la date est antérieure à l'autre, sinon false</returns>
	    public static bool IsOlderPeriode(this DateTime date, DateTime other)
	    {
	      return date.Year < other.Year || (date.Year == other.Year && date.Month < other.Month);
	    }

	    /// <summary>
	    /// Indique si la période de la date est antérieure ou identique à une autre.
	    /// </summary>
	    /// <param name="date">Date concernée</param>
	    /// <param name="other">Date à comparer</param>
	    /// <returns>True si la période de la date est antérieure ou identique à l'autre, sinon false</returns>
	    public static bool IsOlderOrSamePeriode(this DateTime date, DateTime other)
	    {
	      return date.IsOlderPeriode(other) || date.IsSamePeriode(other);
	    }

	    /// <summary>
	    /// Retourne la période suivante d'une date.
	    /// La date retournée sera le 1er jour du mois et de l'année suivant la période passée en paramètre.
	    /// </summary>
	    /// <param name="date">Date concernée</param>
	    /// <returns>La période de la date</returns>
	    public static DateTime GetNextPeriode(this DateTime date)
	    {
	      return date.GetPeriode().AddMonths(1);
	    }

	    /// <summary>
	    /// Retourne la période suivante d'une date.
	    /// La date retournée sera le 1er jour du mois et de l'année suivant la période passée en paramètre.
	    /// </summary>
	    /// <param name="date">Date concernée</param>
	    /// <returns>La période de la date</returns>
	    public static DateTime GetPreviousPeriode(this DateTime date)
	    {
	      return date.GetPeriode().AddMonths(-1);
	    }
    }
}
