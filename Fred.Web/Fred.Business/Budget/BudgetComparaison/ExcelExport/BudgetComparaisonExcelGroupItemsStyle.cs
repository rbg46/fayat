using Fred.Framework.Reporting.SyncFusion.Excel;
using Syncfusion.XlsIO;


namespace Fred.Business.Budget.BudgetComparaison.ExcelExport
{

    /// <summary>
    /// Représente les styles Excel des items d'un groupe.
    /// </summary>
    public class BudgetComparaisonExcelGroupItemsStyle
    {
        private const string NumberFormat = "#,##0.00";

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="itemBaseStyle">Le style de base des items du groupe.</param>
        public BudgetComparaisonExcelGroupItemsStyle(ExcelStyle itemBaseStyle)
        {
            Quantite = new BudgetComparaisonExcelGroupItemStyle(itemBaseStyle, range =>
            {
                range.NumberFormat = NumberFormat;
                range.HorizontalAlignment = ExcelHAlign.HAlignRight;
            });
            Unite = new BudgetComparaisonExcelGroupItemStyle(itemBaseStyle, range =>
            {
                range.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            });
            PrixUnitaire = Quantite;
            Montant = Quantite;
        }

        /// <summary>
        /// Le style de la colonne quantité.
        /// </summary>
        public BudgetComparaisonExcelGroupItemStyle Quantite { get; }

        /// <summary>
        /// Le style de la colonne unité.
        /// </summary>
        public BudgetComparaisonExcelGroupItemStyle Unite { get; }

        /// <summary>
        /// Le style de la colonne prix unitaire.
        /// </summary>
        public BudgetComparaisonExcelGroupItemStyle PrixUnitaire { get; }

        /// <summary>
        /// Le style de la colonne montant.
        /// </summary>
        public BudgetComparaisonExcelGroupItemStyle Montant { get; }
    }
}
