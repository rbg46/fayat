using System;

namespace Fred.Web.Models.Personnel
{
    /// <summary>
    /// Représente un membre du personnel
    /// </summary>
    public class DateEntreeSortieModel
    {
        /// <summary>
        /// Obtient ou définit la date d'entrée du personnel.
        /// </summary>
        public DateTime? DateEntree { get; set; }

        /// <summary>
        /// Obtient ou définit la date de sortie du personnel.
        /// </summary>
        public DateTime? DateSortie { get; set; }
    }
}
