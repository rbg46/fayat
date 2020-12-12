namespace Fred.Web.Shared.Models.Budget.Recette
{
    /// <summary>
    /// Modèle de sauvegarde pour un avancement recette
    /// </summary>
    public class AvancementRecetteSaveModel
    {
        /// <summary>
        /// Identifiant de l'avancement recette
        /// </summary>
        public int AvancementRecetteId { get; set; }

        /// <summary>
        /// Identifiant de la recette
        /// </summary>
        public int BudgetRecetteId { get; set; }

        /// <summary>
        /// Période comptable
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
        /// Définit le montant marché PFA.
        /// </summary>
        public decimal MontantMarchePFA { get; set; }

        /// <summary>
        /// Définit le montant des avenants PFA.
        /// </summary>
        public decimal MontantAvenantsPFA { get; set; }

        /// <summary>
        /// Définit montant de la somme à valoir PFA.
        /// </summary>
        public decimal SommeAValoirPFA { get; set; }

        /// <summary>
        /// Définit montant des travaux supplémentaires PFA.
        /// </summary>
        public decimal TravauxSupplementairesPFA { get; set; }

        /// <summary>
        /// Définit montant de la révision PFA.
        /// </summary>
        public decimal RevisionPFA { get; set; }

        /// <summary>
        /// Définit le montant des autres recettes PFA.
        /// </summary>
        public decimal AutresRecettesPFA { get; set; }

        /// <summary>
        /// Définit le montant des pénalités et retenues PFA.
        /// </summary>
        public decimal PenalitesEtRetenuesPFA { get; set; }

        /// <summary>
        /// Correctif
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
