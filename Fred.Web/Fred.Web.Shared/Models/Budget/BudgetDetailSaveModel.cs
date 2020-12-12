using System.Collections.Generic;
using Fred.Entities.Budget;

namespace Fred.Web.Shared.Models.Budget
{
    /// <summary>
    /// Modèles d'enregistrement d'un détail d'un budget.
    /// </summary>
    public class BudgetDetailSave
    {
        #region Model

        /// <summary>
        /// Modèle d'enregistrement d'un détail d'un budget.
        /// </summary>
        public class Model
        {
            /// <summary>
            /// Identifiant du budget.
            /// </summary>
            public int BudgetId { get; set; }

            /// <summary>
            /// Liste des tâches de niveau 1 à 3 à enregistrer.
            /// </summary>
            public List<Tache1To3Model> Tache1To3s { get; set; }

            /// <summary>
            /// Liste des tâches de niveau 4 à enregistrer.
            /// </summary>
            public List<Tache4Model> Taches4 { get; set; }

            /// <summary>
            /// Liste des identifiants des budget T4 à supprimer.
            /// </summary>
            public List<int> BudgetT4sDeleted { get; set; }
        }

        #endregion
        #region Tache1To3Model

        /// <summary>
        /// Modèle d'enregistrement d'une tâche de niveau 1 à 3 à enregistrer.
        /// </summary>
        public class Tache1To3Model : TacheBaseModel
        { }

        #endregion
        #region Tache4Model

        /// <summary>
        /// Modèle d'enregistrement d'une tâche de niveau 4 à enregistrer.
        /// </summary>
        public class Tache4Model : TacheBaseModel
        {
            /// <summary>
            /// Le montant de la tâche 4.
            /// </summary>
            public decimal? MontantT4 { get; set; }

            /// <summary>
            /// Le montant du sous-détail.
            /// </summary>
            public decimal? MontantSD { get; set; }

            /// <summary>
            /// Le prix unitaire.
            /// </summary>
            public decimal? PU { get; set; }

            /// <summary>
            /// La quantité de base.
            /// </summary>
            public decimal? QuantiteDeBase { get; set; }

            /// <summary>
            /// La quantité à réaliser.
            /// </summary>
            public decimal? QuantiteARealiser { get; set; }

            /// <summary>
            /// Le type d'avancement.
            /// </summary>
            public int? TypeAvancement { get; set; }

            /// <summary>
            /// Identifiant de la tâche de niveau 3 parente.
            /// </summary>
            public int Tache3Id { get; set; }
        }

        #endregion
        #region Tache4Model

        /// <summary>
        /// Modèle d'enregistrement de base pour une tâche.
        /// </summary>
        public abstract class TacheBaseModel
        {
            /// <summary>
            /// Identifiant de la tâche.
            /// </summary>
            public int TacheId { get; set; }

            /// <summary>
            /// Commentaire.
            /// </summary>
            public string Commentaire { get; set; }
        }

        #endregion
        #region ResultModel

        /// <summary>
        /// Représente le résultat de l'enregistrement d'un détail de budget.
        /// </summary>
        public class ResultModel : ErreurResultModel
        {
            /// <summary>
            /// Représente le résultat de création des budgets tâche.
            /// </summary>
            public List<BudgetTacheCreatedModel> BudgetTachesCreated { get; set; }

            /// <summary>
            /// Représente le résultat de création des budgets T4.
            /// </summary>
            public List<BudgetT4CreatedModel> BudgetT4sCreated { get; set; }
        }

        #endregion
        #region BudgetTacheCreatedModel

        /// <summary>
        /// Représente le résultat de création d'un budget T4.
        /// </summary>
        public class BudgetTacheCreatedModel
        {
            /// <summary>
            /// Identifiant de la tâche.
            /// </summary>
            public int TacheId { get; private set; }

            /// <summary>
            /// Identifiant du budget T4.
            /// </summary>
            public int BudgetTacheId { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="budgetT4Ent">Entité concernée.</param>
            public BudgetTacheCreatedModel(BudgetTacheEnt budgetTacheEnt)
            {
                TacheId = budgetTacheEnt.TacheId;
                BudgetTacheId = budgetTacheEnt.BudgetTacheId;
            }
        }

        #endregion
        #region BudgetT4CreatedModel

        /// <summary>
        /// Représente le résultat de création d'un budget T4.
        /// </summary>
        public class BudgetT4CreatedModel
        {
            /// <summary>
            /// Identifiant de la tâche.
            /// </summary>
            public int TacheId { get; private set; }

            /// <summary>
            /// Identifiant du budget T4.
            /// </summary>
            public int BudgetT4Id { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="budgetT4Ent">Entité concernée.</param>
            public BudgetT4CreatedModel(BudgetT4Ent budgetT4Ent)
            {
                TacheId = budgetT4Ent.T4Id;
                BudgetT4Id = budgetT4Ent.BudgetT4Id;
            }
        }

        #endregion
    }
}
