namespace Fred.Web.Shared.Models.Budget.ControleBudgetaire
{
    /// <summary>
    /// Représente les valeurs à afficher dans le temlate excel
    /// </summary>
    public class ControleBudgetaireExportModelValeurs
    {

        public string Code { get; set; }

        /// <summary>
        /// Servira d'entête pour la ligne e.g T1--XX Installation 
        /// </summary>
        public string LibelleTacheOuRessource { get; set; }

        /// <summary>
        /// Définit le type de l'axe T1, T2, T3, Chapitre, SousChapitre, Ressource
        /// </summary>
        public string TypeAxe { get; set; }

        /// <summary>
        /// Montant budgété pour cette tache ou ressource
        /// </summary>
        public decimal MontantBudget { get; set; }

        /// <summary>
        /// Le montant du Droit à la dépense pour la periode choisie par l'utilisateur
        /// </summary>
        public decimal DadMoisCourant { get; set; }

        /// <summary>
        /// L'avancement pour la periode choisie par l'utilisateur
        /// La valeur est située entre 0 et 1
        /// </summary>
        public decimal AvancementMoisCourant { get; set; }

        /// <summary>
        /// Toutes les dépenses effectées sur cette tache jusqu'a là periode choisie par l'utilisateur
        /// </summary>
        public decimal DepensesMoisCourant { get; set; }

        /// <summary>
        /// Ecart entre le budget et les dépenses au mois choisi par l'utilisateur
        /// </summary>
        public decimal EcartMoisCourant { get; set; }

        /// <summary>
        /// Le montant du Droit à là dépense calculé jusqu'au mois limite choisi par l'utilisateur
        /// </summary>
        public decimal DadCumule { get; set; }

        /// <summary>
        /// L'avancement pour la periode jusqu'au mois limite choisi par l'utilisateur
        /// La valeur est située entre 0 et 1
        /// </summary>
        public decimal AvancementCumule { get; set; }


        /// <summary>
        /// Toutes les dépenses effectées sur cette tache jusqu'au mois limite choisi par l'utilisateur
        /// </summary>
        public decimal DepensesCumulees { get; set; }

        /// <summary>
        /// Ecart entre le budget et les dépenses jusqu'au mois limite choisi par l'utilisateur
        /// </summary>
        public decimal EcartCumule { get; set; }

        /// <summary>
        /// Le reste à dépenser theorique à la periode choisie par l'utilisateur
        /// </summary>
        public decimal ResteADepenserTheorique { get; set; }

        /// <summary>
        /// Le pourcentage du reste à dépenser theorique à la periode choisie par l'utilisateur
        /// </summary>
        public decimal PourcentageResteADepenserTheorique { get; set; }

        /// <summary>
        /// L'ajustement pour la tache affichée et pour la periode chosiie par l'utilisateur
        /// </summary>
        public decimal Ajustement { get; set; }

        /// <summary>
        /// Prevision fin affaire au mois choisi par l'utilisateur
        /// </summary>
        public decimal PFA { get; set; }

        /// <summary>
        /// Prevision fin affaire du mois précédant
        /// </summary>
        public decimal EcartM1 { get; set; }
    }
}
