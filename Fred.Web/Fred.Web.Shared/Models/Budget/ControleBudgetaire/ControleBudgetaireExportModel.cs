using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Budget.ControleBudgetaire
{
    /// <summary>
    /// Représente les Budgets à afficher dans le template excel
    /// </summary>
    public class ControleBudgetaireExportModel
    {
        /// <summary>
        /// Identifiant du CI
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Code et libellé de CI formatté
        /// </summary>
        public string CodeLibelleCI { get; set; }

        /// <summary>
        /// Total de la recette
        /// </summary>
        public decimal? TotalRecette { get; set; }

        /// <summary>
        /// Total avancement facture (cumulé)
        /// </summary>
        public decimal? TotalAvancementFacture { get; set; }

        /// <summary>
        /// Total avancement facture (cumulé)
        /// </summary>
        public decimal TotalAvancementFacturePeriode { get; set; }

        /// <summary>
        /// Total PFA
        /// </summary>
        public decimal TotalPFA { get; set; }

        /// <summary>
        /// Titre du Budget
        /// </summary>
        public string TitreBudget { get; set; }

        /// <summary>
        /// Titre Global
        /// </summary>
        public string TitreGlobal { get; set; }

        /// <summary>
        /// Liste des valeurs
        /// </summary>
        public List<ControleBudgetaireExportModelValeurs> Valeurs { get; set; }

        public decimal FraisGenerauxBudget { get; set; }
        public decimal FraisGenerauxRecette { get; set; }
        public decimal FraisGenerauxRecetteCumul { get; set; }
        public decimal FraisGenerauxPfa { get; set; }
        public string TitreTauxFraisGeneraux { get; set; }

    }
}
