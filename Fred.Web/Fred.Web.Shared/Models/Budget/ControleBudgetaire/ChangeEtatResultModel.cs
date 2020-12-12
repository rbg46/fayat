namespace Fred.Web.Shared.Models.Budget.ControleBudgetaire
{
    /// <summary>
    /// Représente le résultat de la fonction permettant de tester si le passage d'un etat à un autre est possible
    /// </summary>
    public class ChangeEtatResultModel
    {
        /// <summary>
        /// True si l'avancement est validé
        /// </summary>
        public bool AvancementValide { get; set; } = true;

        /// <summary>
        /// True si le CI est cloturé
        /// </summary>
        public bool PeriodeComptableCloturee { get; set; } = true;

        /// <summary>
        /// True si l'ID du budget associé au controle budgétaire a changé
        /// </summary>
        public bool BudgetIdHasChanged { get; set; } = false;

        /// <summary>
        /// True si le passage est possible depuis l'état précedent 
        /// e.g passer d'un état brouillon à l'état validé est impossible
        /// e.g passer d'un état brouillon à l'état a valider est possible
        /// </summary>
        public bool EtatPrecedentOkay { get; set; } = true;

        /// <summary>
        /// Variable raccourcie utilisable pour savoir si toutes les conditions sont remplies
        /// </summary>
        public bool AllOkay => AvancementValide && PeriodeComptableCloturee && EtatPrecedentOkay;

        /// <summary>
        /// Représente l'état de destination 
        /// </summary>
        public BudgetEtatModel NouvelEtat { get; set; }
    }



}
