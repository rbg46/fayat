using System;

namespace Fred.Web.Shared.Models.Budget.Liste
{
    /// <summary>
    /// Définit le modèle d'objet à retourner après la suppression d'un budget
    /// </summary>
    public class BudgetSuppressionSuccessModel
    {
        /// <summary>
        /// Date de suppression du budget, la composante horaire n'est pas utilisée
        /// </summary>
        public DateTime DateSuppression { get; set; }
    }
}
