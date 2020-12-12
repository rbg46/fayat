using System.Collections.Generic;

namespace Fred.Entities.Budget
{
    /// <summary>
    /// Représente un controle budgétaire
    /// </summary>
    public class ControleBudgetaireEnt
    {
        /// <summary>
        /// Id du budget auquel est rattaché ce controle budgétaire
        /// </summary>
        public int ControleBudgetaireId { get; set; }

        /// <summary>
        /// Periode d'application du controle budgétaire
        /// </summary>
        public int Periode { get; set; }

        /// <summary>
        /// Budget auquel est rattaché ce controle budgétaire
        /// </summary>
        public BudgetEnt Budget { get; set; }

        /// <summary>
        /// Id de l'état de controle budgétaire
        /// </summary>
        public int ControleBudgetaireEtatId { get; set; }

        /// <summary>
        /// Etat du controle budgétaire
        /// </summary>
        public BudgetEtatEnt ControleBudgetaireEtat { get; set; }

        /// <summary>
        /// Valeurs de ce controle budgétaire
        /// </summary>
        public ICollection<ControleBudgetaireValeursEnt> Valeurs { get; set; }
    }
}
