namespace Fred.Web.Shared.Models.Budget.Recette
{
    public class BudgetRecetteModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un budget.
        /// </summary>
        public int BudgetRecetteId { get; set; }

        /// <summary>
        /// Définit le montant marché.
        /// </summary>
        public decimal? MontantMarche { get; set; }

        /// <summary>
        /// Définit le montant des avenants.
        /// </summary>
        public decimal? MontantAvenants { get; set; }

        /// <summary>
        /// Définit montant de la somme à valoir.
        /// </summary>
        public decimal? SommeAValoir { get; set; }

        /// <summary>
        /// Définit montant des travaux supplémentaires.
        /// </summary>
        public decimal? TravauxSupplementaires { get; set; }

        /// <summary>
        /// Définit montant de la révision.
        /// </summary>
        public decimal? Revision { get; set; }

        /// <summary>
        /// Définit le montant des autres recettes.
        /// </summary>
        public decimal? AutresRecettes { get; set; }

        /// <summary>
        /// Définit le montant des pénalités et retenues.
        /// </summary>
        public decimal? PenalitesEtRetenues { get; set; }

        public decimal? TotalRecette
        {
            get
            {
                return MontantMarche + MontantAvenants + SommeAValoir + TravauxSupplementaires + Revision + AutresRecettes + PenalitesEtRetenues;
            }
        }
    }
}
