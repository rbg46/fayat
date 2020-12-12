using System;

namespace Fred.Common.Tests.EntityFramework
{
    /// <summary>
    /// Liste des méthodes remplacant celle de DbFucntions
    /// </summary>
    public static class FakeDbFunctions
    {
        /// <summary>
        /// Permet d'obtinir une date sans les heures
        /// </summary>
        /// <param name="dateValue">date à traiter</param>
        /// <returns>date sans les heures</returns>
        public static DateTime? TruncateTime(DateTime? dateValue)
        {
            if (dateValue == null)
                throw new ArgumentNullException("dateValue");
            return dateValue.Value.Date;
        }

        /// <summary>
        /// Ajoute des jours à la date
        /// </summary>
        /// <param name="dateValue">date à traiter</param>
        /// <param name="addValue">nombre de jours à ajouter</param>
        /// <returns></returns>
        public static DateTime? AddDays(DateTime? dateValue, int? addValue)
        {
            if (dateValue == null)
                throw new ArgumentNullException("dateValue");
            return dateValue.Value.AddDays(addValue ?? 0);
        }
    }
}
