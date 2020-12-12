using Fred.Entities.Budget.Recette;

namespace Fred.Web.Shared.Models.Budget.Recette
{
    /// <summary>
    /// Représente un modèle d'avancement de recette.
    /// </summary>
    public class AvancementRecetteLoadModel
    {
        #region AvancementRecette
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un avancement.
        /// </summary>
        public int AvancementRecetteId { get; set; } = 0;

        /// <summary>
        /// List des erreurs
        /// </summary>
        public string Erreurs { get; set; } = string.Empty;

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

        /// <summary>
        /// Taux de frais généraux
        /// </summary>
        public decimal TauxFraisGeneraux { get; set; } = 0;

        /// <summary>
        /// Ajustement de frais généraux
        /// </summary>
        public decimal AjustementFraisGeneraux { get; set; } = 0;

        /// <summary>
        /// Avancement Taux de frais généraux
        /// </summary>
        public decimal AvancementTauxFraisGeneraux { get; set; } = 0;

        /// <summary>
        /// Avancement Ajustement de frais généraux
        /// </summary>
        public decimal AvancementAjustementFraisGeneraux { get; set; } = 0;

        /// <summary>
        /// Retourne le total avancement calculé
        /// </summary>
        public decimal TotalAvancementRecette
        {
            get { return MontantMarche + MontantAvenants + SommeAValoir + TravauxSupplementaires + Revision + AutresRecettes + PenalitesEtRetenues; }
        }

        /// <summary>
        /// Retourne le total Facture calculé
        /// </summary>
        public decimal TotalAvancementFacture
        {
            get { return TotalAvancementRecette + Correctif; }
        }

        /// <summary>
        /// Retourne le total PFA calculé
        /// </summary>
        public decimal TotalPFA
        {
            get { return MontantMarchePFA + MontantAvenantsPFA + SommeAValoirPFA + TravauxSupplementairesPFA + RevisionPFA + AutresRecettesPFA + PenalitesEtRetenuesPFA; }

        }

        /// <summary>
        /// Retourne le total Facture calculé
        /// </summary>
        public decimal TotalAvancementFacturePeriode
        {
            get { return TotalAvancementFacture - TotalAvancementPreviousFacture; }
        }

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

        /// <summary>
        /// Définit le total des recettes.
        /// </summary>
        public decimal TotalAvancementPreviousRecette
        {
            get { return MontantMarchePrevious + MontantAvenantsPrevious + SommeAValoirPrevious + TravauxSupplementairesPrevious + RevisionPrevious + AutresRecettesPrevious + PenalitesEtRetenuesPrevious; }
        }

        /// <summary>
        /// Définit le total des recettes.
        /// </summary>
        public decimal TotalAvancementPreviousFacture
        {
            get { return TotalAvancementPreviousRecette + CorrectifPrevious; }
        }

        /// <summary>
        /// Avancement Taux de frais généraux
        /// </summary>
        public decimal AvancementTauxFraisGenerauxPrevious { get; set; } = 0;

        /// <summary>
        /// Avancement Ajustement de frais généraux
        /// </summary>
        public decimal AvancementAjustementFraisGenerauxPrevious { get; set; } = 0;
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

        /// <summary>
        /// Définit le total des recettes.
        /// </summary>
        public decimal TotalPFARecette
        {
            get { return MontantMarchePFA + MontantAvenantsPFA + SommeAValoirPFA + TravauxSupplementairesPFA + RevisionPFA + AutresRecettesPFA + PenalitesEtRetenuesPFA; }
        }

        /// <summary>
        /// Taux de frais généraux PFA
        /// </summary>
        public decimal TauxFraisGenerauxPFA { get; set; } = 0;

        /// <summary>
        /// Ajustement de frais généraux PFA
        /// </summary>
        public decimal AjustementFraisGenerauxPFA { get; set; } = 0;
        #endregion

        /// <summary>
        /// Définit le montant de la correction.
        /// </summary>
        public decimal Correctif { get; set; } = 0;

        /// <summary>
        /// Définit le montant de la correction.
        /// </summary>
        public decimal CorrectifPrevious { get; set; } = 0;

        /// <summary>
        /// Constructeur.
        /// </summary>
        public AvancementRecetteLoadModel(BudgetRecetteEnt recette, AvancementRecetteEnt avancement, AvancementRecetteEnt avancementPrevious)
        {
            if (recette != null)
            {
                LoadRecette(recette);
                LoadAvancementRecette(avancement, avancementPrevious);
            }
            else
            {
                Erreurs = "Aucune recette n'est défini pour le budget en application";
            }
        }

        public AvancementRecetteLoadModel(AvancementRecetteEnt avancement)
        {
            LoadCurrentAvancement(avancement);
        }


        private void LoadAvancementRecette(AvancementRecetteEnt avancement, AvancementRecetteEnt avancementPrevious)
        {
            if (avancement != null)
            {
                LoadCurrentAvancement(avancement);
                LoadPFA(avancement);
            }
            else if (avancementPrevious != null)
            {
                avancementPrevious.AvancementRecetteId = 0;
                LoadCurrentAvancement(avancementPrevious);
                LoadPFA(avancementPrevious);
            }

            if (avancementPrevious != null)
            {
                LoadPreviousAvancement(avancementPrevious);
            }

        }

        private void LoadRecette(BudgetRecetteEnt recette)
        {
            BudgetRecette = new BudgetRecetteModel();
            BudgetRecette.BudgetRecetteId = recette.BudgetRecetteId;
            BudgetRecette.MontantMarche = recette.MontantMarche ?? 0;
            BudgetRecette.MontantAvenants = recette.MontantAvenants ?? 0;
            BudgetRecette.SommeAValoir = recette.SommeAValoir ?? 0;
            BudgetRecette.TravauxSupplementaires = recette.TravauxSupplementaires ?? 0;
            BudgetRecette.Revision = recette.Revision ?? 0;
            BudgetRecette.AutresRecettes = recette.AutresRecettes ?? 0;
            BudgetRecette.PenalitesEtRetenues = recette.PenalitesEtRetenues ?? 0;
            MontantMarchePFA = recette.MontantMarche ?? 0;
            MontantAvenantsPFA = recette.MontantAvenants ?? 0;
            SommeAValoirPFA = recette.SommeAValoir ?? 0;
            TravauxSupplementairesPFA = recette.TravauxSupplementaires ?? 0;
            RevisionPFA = recette.Revision ?? 0;
            AutresRecettesPFA = recette.AutresRecettes ?? 0;
            PenalitesEtRetenuesPFA = recette.PenalitesEtRetenues ?? 0;
        }

        private void LoadCurrentAvancement(AvancementRecetteEnt avancement)
        {
            AvancementRecetteId = avancement.AvancementRecetteId;
            MontantMarche = avancement.MontantMarche;
            MontantAvenants = avancement.MontantAvenants;
            MontantAvenantsPFA = avancement.MontantAvenantsPFA;
            SommeAValoir = avancement.SommeAValoir;
            TravauxSupplementaires = avancement.TravauxSupplementaires;
            Revision = avancement.Revision;
            AutresRecettes = avancement.AutresRecettes;
            PenalitesEtRetenues = avancement.PenalitesEtRetenues;
            TauxFraisGeneraux = avancement.TauxFraisGeneraux;
            TauxFraisGenerauxPFA = avancement.TauxFraisGenerauxPFA;
            AvancementTauxFraisGeneraux = avancement.AvancementTauxFraisGeneraux;
            AjustementFraisGeneraux = avancement.AjustementFraisGeneraux;
            AjustementFraisGenerauxPFA = avancement.AjustementFraisGenerauxPFA;
            AvancementAjustementFraisGeneraux = avancement.AvancementAjustementFraisGeneraux;
            Correctif = avancement.Correctif;
            Periode = avancement.Periode;
        }

        private void LoadPreviousAvancement(AvancementRecetteEnt avancementPrevious)
        {
            MontantMarchePrevious = avancementPrevious.MontantMarche;
            MontantAvenantsPrevious = avancementPrevious.MontantAvenants;
            SommeAValoirPrevious = avancementPrevious.SommeAValoir;
            TravauxSupplementairesPrevious = avancementPrevious.TravauxSupplementaires;
            RevisionPrevious = avancementPrevious.Revision;
            AutresRecettesPrevious = avancementPrevious.AutresRecettes;
            PenalitesEtRetenuesPrevious = avancementPrevious.PenalitesEtRetenues;
            AvancementTauxFraisGenerauxPrevious = avancementPrevious.AvancementTauxFraisGeneraux;
            AvancementAjustementFraisGenerauxPrevious = avancementPrevious.AvancementAjustementFraisGeneraux;
            CorrectifPrevious = avancementPrevious.Correctif;
        }

        private void LoadPFA(AvancementRecetteEnt avancement)
        {
            MontantMarchePFA = avancement.MontantMarchePFA;
            MontantAvenantsPFA = avancement.MontantAvenantsPFA;
            SommeAValoirPFA = avancement.SommeAValoirPFA;
            TravauxSupplementairesPFA = avancement.TravauxSupplementairesPFA;
            RevisionPFA = avancement.RevisionPFA;
            AutresRecettesPFA = avancement.AutresRecettesPFA;
            PenalitesEtRetenuesPFA = avancement.PenalitesEtRetenuesPFA;
        }
    }
}
