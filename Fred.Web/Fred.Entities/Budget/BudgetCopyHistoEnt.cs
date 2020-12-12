using System;
using Fred.Entities.CI;

namespace Fred.Entities.Budget
{
    /// <summary>
    /// Représente l'historique de copie d'un budget.
    /// </summary>
    public class BudgetCopyHistoEnt
    {
        private DateTime? dateCopy;

        /// <summary>
        /// L'identifiant de l'historique.
        /// </summary>
        public int BudgetCopyHistoId { get; set; }

        /// <summary>
        /// L'identifiant du budget concerné.
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        /// Le budget concerné.
        /// </summary>
        public BudgetEnt Budget { get; set; }

        /// <summary>
        /// L'identifiant du CI du budget source.
        /// </summary>
        public int BudgetSourceCIId { get; set; }

        /// <summary>
        /// Le CI du budget source.
        /// </summary>
        public CIEnt BudgetSourceCI { get; set; }

        /// <summary>
        /// La version du budget source.
        /// </summary>
        public string BudgetSourceVersion { get; set; }

        /// <summary>
        /// L'identifiant du CI de la bibliothèque des prix source.
        /// </summary>
        public int? BibliothequePrixSourceCIId { get; set; }

        /// <summary>
        /// Le CI de la bibliothèque des prix source.
        /// </summary>
        public CIEnt BibliothequePrixSourceCI { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la copie.
        /// </summary>
        public DateTime? DateCopy
        {
            get { return (dateCopy.HasValue) ? DateTime.SpecifyKind(dateCopy.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateCopy = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }
    }
}
