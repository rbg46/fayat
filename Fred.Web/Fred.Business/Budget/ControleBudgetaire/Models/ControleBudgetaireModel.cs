using System.Collections.Generic;
using Fred.Business.Budget.Helpers;

namespace Fred.Business.Budget.ControleBudgetaire.Models
{
    /// <summary>
    /// Model du controle budgétaire
    /// </summary>
    public class ControleBudgetaireModel
    {
        /// <summary>
        /// Représente les données à afficher sur l'écran du controle budgétaire
        /// </summary>
        public IEnumerable<AxeTreeModel> Tree { get; set; }

        /// <summary>
        /// Id du budget dont on récupère le controle budgétaire
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        /// Décrit l'état du controle budgétaire, celui ci sera en lecture seule si le code represente l'état validé (EA pour en application)
        /// </summary>
        public string CodeEtat { get; set; }

        /// <summary>
        /// Un controle budgetaire Vérouillé est plus restrictif qu'un controle budgétaire en lecture seule
        /// Car un controle budgétaire vérouillé ne peut presque jamais être dévérouillé.
        /// e.g un controle budgétaire validé existe dans le futur
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Un controle budgétaire Readonly est dans cet état si il n'est pas brouillon
        /// </summary>
        public bool Readonly { get; set; }

        /// <summary>
        /// True si l'avancement est validé pour la période correspondant à ce model
        /// </summary>
        public bool AvancementValide { get; set; }


        /// <summary>
        /// True si le CI est cloturé sur cette période 
        /// </summary>
        public bool PeriodeCloturee { get; set; }

        /// <summary>
        /// Periode au format YYYYMM de ce controle budgétaire
        /// </summary>
        public int Periode { get; set; }

        /// <summary>
        /// Version du Budget
        /// </summary>
        public string BudgetVersion { get; set; }

        /// <summary>
        /// Date d'application du budget
        /// </summary>
        public string DateBudget { get; set; }

    }
}
