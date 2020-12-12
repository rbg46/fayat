using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.Web.Shared.Models.Budget
{
    /// <summary>
    /// Représente le model de chargement des tâches 4 non utilisées dans une révision de budget.
    /// </summary>
    public class Tache4Inutilisees
    {
        /// <summary>
        /// Représente le model de chargement des tâches 4 non utilisées dans une révision de budget.
        /// </summary>
        public class Model
        {
            /// <summary>
            /// La révision de budget.
            /// </summary>
            public int BudgetId { get; set; }

            /// <summary>
            /// L'identifiant du CI du budget.
            /// </summary>
            public int CiId { get; set; }

            /// <summary>
            /// Identifiants des tâches 4 déjà utilisées dans la révision de budget.
            /// </summary>
            public List<int> Tache4Ids { get; set; }
        }

        /// <summary>
        /// Représente le résultat de chargement des tâches 4 non utilisées dans une révision de budget.
        /// </summary>
        public class ResultModel : ResultModelBase
        {
            /// <summary>
            /// Liste des tâche 4 non utilisées.
            /// </summary>
            public List<Tache4Model> Taches4 { get; private set; } = new List<Tache4Model>();
        }

        /// <summary>
        /// Représente une tâche 4 non utilisée.
        /// </summary>
        public class Tache4Model : ResultModelBase
        {
            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="tacheEnt">La tâche.</param>
            public Tache4Model(TacheEnt tacheEnt)
            {
                TacheId = tacheEnt.TacheId;
                Code = tacheEnt.Code;
                Libelle = tacheEnt.Libelle;
            }

            /// <summary>
            /// Identifiant de la tâche.
            /// </summary>
            public int TacheId { get; private set; }

            /// <summary>
            /// Code de la tâche.
            /// </summary>
            public string Code { get; private set; }

            /// <summary>
            /// Libellé de la tâche.
            /// </summary>
            public string Libelle { get; private set; }
        }
    }
}
