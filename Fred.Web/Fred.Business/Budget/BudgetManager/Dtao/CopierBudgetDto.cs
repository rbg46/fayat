using System.Collections.Generic;
using Fred.Web.Shared.Models.Budget;

namespace Fred.Business.Budget.BudgetManager.Dtao
{
    /// <summary>
    /// DTO de la copie de budget.
    /// </summary>
    public class CopierBudgetDto
    {
        /// <summary>
        /// Représente la requète de la copie.
        /// </summary>
        public class Request
        {
            /// <summary>
            /// Identifiant de la devise.
            /// </summary>
            public int DeviseId { get; set; }

            /// <summary>
            /// Identifiant du budget cible.
            /// </summary>
            public int BudgetCibleId { get; set; }

            /// <summary>
            /// Identifiant du CI du budget cible.
            /// </summary>
            public int BudgetCibleCiId { get; set; }

            /// <summary>
            /// Identifiant du CI du budget source.
            /// </summary>
            public int BudgetSourceCiId { get; set; }

            /// <summary>
            /// Révision de budget source.
            /// </summary>
            public string BudgetSourceRevision { get; set; }

            /// <summary>
            /// Identifiant de l'organisation de la bibliothèque des prix.
            /// </summary>
            public int? BibliothequePrixOrganisationId { get; set; }

            /// <summary>
            /// Si aucune bibliothèque des prix n'est indiquée ou si elle n'existe pas, indique s'il faut copier les
            /// composantes du budget source ou mettre les valeurs par défaut.
            /// Si une bibliothèque des prix est indiquée, la valeur sera null.
            /// </summary>
            public bool? ComposantesDuBudgetSource { get; set; }

            /// <summary>
            /// Indique si la copie concerne uniquement les lignes vides ou si elle doit se faire intégralement.
            /// </summary>
            public bool OnlyLignesVides { get; set; }

            /// <summary>
            /// Indique si les ressources spécifiques doivent également être copiées.
            /// </summary>
            public bool IncludeRessourceSpecifiques { get; set; }
        }

        /// <summary>
        /// Représente le résultat de la copie.
        /// </summary>
        public class Result : ErreurResultModel
        {
            /// <summary>
            /// Les identifiants des tâches 4 qui ont été copiées.
            /// </summary>
            public List<int> Tache4IdCopies { get; set; } = new List<int>();
        }
    }
}
