namespace Fred.Web.Shared.Models.Budget.Recette
{
    public class AvancementRecetteModel
    {
        #region AvancementRecette
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un avancement.
        /// </summary>
        public int AvancementRecetteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du CI auquel se avancement appartient
        /// </summary>
        public int BudgetRecetteId { get; set; }

        /// <summary>
        ///   Obtient ou définit le CI auquel ce avancement appartient
        /// </summary>
        public BudgetRecetteModel BudgetRecette { get; set; }

        /// <summary>
        /// Définit la période de l'avancement. 
        /// Format YYYYMM
        /// </summary>
        public int Periode { get; set; }

        /// <summary>
        /// Définit le montant marché.
        /// </summary>
        public decimal MontantMarche { get; set; } = 0;

        /// <summary>
        /// Définit le montant des avenants.
        /// </summary>
        public decimal MontantAvenants { get; set; } = 0;

        /// <summary>
        /// Définit montant de la somme à valoir.
        /// </summary>
        public decimal SommeAValoir { get; set; } = 0;

        /// <summary>
        /// Définit montant des travaux supplémentaires.
        /// </summary>
        public decimal TravauxSupplementaires { get; set; } = 0;

        /// <summary>
        /// Définit montant de la révision.
        /// </summary>
        public decimal Revision { get; set; } = 0;

        /// <summary>
        /// Définit le montant des autres recettes.
        /// </summary>
        public decimal AutresRecettes { get; set; } = 0;

        /// <summary>
        /// Définit le montant des pénalités et retenues.
        /// </summary>
        public decimal PenalitesEtRetenues { get; set; } = 0;
        #endregion

        #region avancementRecettePrevious
        /// <summary>
        /// Définit le montant marché.
        /// </summary>
        public decimal MontantMarchePrevious { get; set; } = 0;

        /// <summary>
        /// Définit le montant des avenants.
        /// </summary>
        public decimal MontantAvenantsPrevious { get; set; } = 0;

        /// <summary>
        /// Définit montant de la somme à valoir.
        /// </summary>
        public decimal SommeAValoirPrevious { get; set; } = 0;

        /// <summary>
        /// Définit montant des travaux supplémentaires.
        /// </summary>
        public decimal TravauxSupplementairesPrevious { get; set; } = 0;

        /// <summary>
        /// Définit montant de la révision.
        /// </summary>
        public decimal RevisionPrevious { get; set; } = 0;

        /// <summary>
        /// Définit le montant des autres recettes.
        /// </summary>
        public decimal AutresRecettesPrevious { get; set; } = 0;

        /// <summary>
        /// Définit le montant des pénalités et retenues.
        /// </summary>
        public decimal PenalitesEtRetenuesPrevious { get; set; } = 0;
        #endregion

        #region PFA
        /// <summary>
        /// Définit le montant marché.
        /// </summary>
        public decimal MontantMarchePFA { get; set; } = 0;

        /// <summary>
        /// Définit le montant des avenants.
        /// </summary>
        public decimal MontantAvenantsPFA { get; set; } = 0;

        /// <summary>
        /// Définit montant de la somme à valoir.
        /// </summary>
        public decimal SommeAValoirPFA { get; set; } = 0;

        /// <summary>
        /// Définit montant des travaux supplémentaires.
        /// </summary>
        public decimal TravauxSupplementairesPFA { get; set; } = 0;

        /// <summary>
        /// Définit montant de la révision.
        /// </summary>
        public decimal RevisionPFA { get; set; } = 0;

        /// <summary>
        /// Définit le montant des autres recettes.
        /// </summary>
        public decimal AutresRecettesPFA { get; set; } = 0;

        /// <summary>
        /// Définit le montant des pénalités et retenues.
        /// </summary>
        public decimal PenalitesEtRetenuesPFA { get; set; } = 0;
        #endregion

        /// <summary>
        /// Définit le montant de la correction.
        /// </summary>
        public decimal Correction { get; set; } = 0;
    }
}
