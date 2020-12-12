namespace Fred.Entities.Budget.Recette
{
    /// <summary>
    ///   Représente un avancement
    /// </summary>
    public class AvancementRecetteEnt
    {
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
        public BudgetRecetteEnt BudgetRecette { get; set; }

        /// <summary>
        /// Définit la période de l'avancement. 
        /// Format YYYYMM
        /// </summary>
        public int Periode { get; set; }

        /// <summary>
        /// Définit le montant marché.
        /// </summary>
        public decimal MontantMarche { get; set; }

        /// <summary>
        /// Définit le montant des avenants.
        /// </summary>
        public decimal MontantAvenants { get; set; }

        /// <summary>
        /// Définit montant de la somme à valoir.
        /// </summary>
        public decimal SommeAValoir { get; set; }

        /// <summary>
        /// Définit montant des travaux supplémentaires.
        /// </summary>
        public decimal TravauxSupplementaires { get; set; }

        /// <summary>
        /// Définit montant de la révision.
        /// </summary>
        public decimal Revision { get; set; }

        /// <summary>
        /// Définit le montant des autres recettes.
        /// </summary>
        public decimal AutresRecettes { get; set; }

        /// <summary>
        /// Définit le montant des pénalités et retenues.
        /// </summary>
        public decimal PenalitesEtRetenues { get; set; }

        /// <summary>
        /// Définit le montant marché.
        /// </summary>
        public decimal MontantMarchePFA { get; set; }

        /// <summary>
        /// Définit le montant des avenants.
        /// </summary>
        public decimal MontantAvenantsPFA { get; set; }

        /// <summary>
        /// Définit montant de la somme à valoir.
        /// </summary>
        public decimal SommeAValoirPFA { get; set; }

        /// <summary>
        /// Définit montant des travaux supplémentaires.
        /// </summary>
        public decimal TravauxSupplementairesPFA { get; set; }

        /// <summary>
        /// Définit montant de la révision.
        /// </summary>
        public decimal RevisionPFA { get; set; }

        /// <summary>
        /// Définit le montant des autres recettes.
        /// </summary>
        public decimal AutresRecettesPFA { get; set; }

        /// <summary>
        /// Définit le montant des pénalités et retenues.
        /// </summary>
        public decimal PenalitesEtRetenuesPFA { get; set; }

        /// <summary>
        /// Définit le montant de la correction.
        /// </summary>
        public decimal Correctif { get; set; }

        /// <summary>
        /// Taux de frais généraux
        /// </summary>
        public decimal TauxFraisGeneraux { get; set; }

        /// <summary>
        /// Ajustement de frais généraux
        /// </summary>
        public decimal AjustementFraisGeneraux { get; set; }

        /// <summary>
        /// Taux de frais généraux PFA
        /// </summary>
        public decimal TauxFraisGenerauxPFA { get; set; }

        /// <summary>
        /// Ajustement de frais généraux PFA
        /// </summary>
        public decimal AjustementFraisGenerauxPFA { get; set; }

        /// <summary>
        /// Avancement Taux de frais généraux
        /// </summary>
        public decimal AvancementTauxFraisGeneraux { get; set; }

        /// <summary>
        /// Avancement Ajustement de frais généraux
        /// </summary>
        public decimal AvancementAjustementFraisGeneraux { get; set; }
    }
}
