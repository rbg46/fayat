using Fred.Framework.Reporting.SyncFusion.Excel;
using Syncfusion.XlsIO;

namespace Fred.Business.Budget.BudgetComparaison.ExcelExport
{
    /// <summary>
    /// Représente les positions des colonnes d'un groupe dans le fichier Excel.
    /// </summary>
    public class BudgetComparaisonExcelGroupColumns : ExcelColumns
    {
        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="worksheet">La page.</param>
        /// <param name="start">L'index de début du groupe.</param>
        /// <param name="end">L'index de fin du groupe.</param>
        /// <param name="quantite">L'index de la colonne quantité ou zéro si non utilisé.</param>
        /// <param name="unite">L'index de la colonne unité ou zéro si non utilisé.</param>
        /// <param name="prixUnitaire">L'index de la colonne prix unitaire ou zéro si non utilisé.</param>
        /// <param name="montant">L'index de la colonne montant ou zéro si non utilisé.</param>
        public BudgetComparaisonExcelGroupColumns(IWorksheet worksheet, int start, int end, int quantite, int unite, int prixUnitaire, int montant)
            : base(worksheet, start, end)
        {
            Quantite = new ExcelColumn(worksheet, quantite);
            Unite = new ExcelColumn(worksheet, unite);
            PrixUnitaire = new ExcelColumn(worksheet, prixUnitaire);
            Montant = new ExcelColumn(worksheet, montant);
        }

        /// <summary>
        /// La colonne quantité.
        /// </summary>
        public ExcelColumn Quantite { get; }

        /// <summary>
        /// La colonne unité.
        /// </summary>
        public ExcelColumn Unite { get; }

        /// <summary>
        /// La colonne prix unitaire.
        /// </summary>
        public ExcelColumn PrixUnitaire { get; }

        /// <summary>
        /// La colonne montant.
        /// </summary>
        public ExcelColumn Montant { get; }
    }
}
