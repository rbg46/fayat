using System;

namespace Fred.Web.Shared.Models.Budget
{
    /// <summary>
    /// Représente l'historique de copie d'un budget.
    /// </summary>
    public class BudgetCopyHistoModel
    {
        /// <summary>
        /// Le CI du budget source.
        /// </summary>
        public BudgetCopyHistoCIModel BudgetSourceCI { get; set; }

        /// <summary>
        /// La version du budget source.
        /// </summary>
        public string BudgetSourceVersion { get; set; }

        /// <summary>
        /// Le CI de la bibliothèque des prix source.
        /// </summary>
        public BudgetCopyHistoCIModel BibliothequePrixSourceCI { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la copie.
        /// </summary>
        public DateTime? DateCopy { get; set; }
    }

    /// <summary>
    /// Représente un CI utilisé lors d'une copie de budget.
    /// </summary>
    public class BudgetCopyHistoCIModel
    {
        /// <summary>
        /// Le code du CI.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Le libellé du CI.
        /// </summary>
        public string Libelle { get; set; }
    }
}
