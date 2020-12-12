using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Budget
{
    /// <summary>
    /// Modèles de création de plusieurs tâches 4.
    /// </summary>
    public class CreateTaches4
    {
        #region Model

        /// <summary>
        /// Modèles de création de plusieurs tâches 4.
        /// </summary>
        public class Model
        {
            /// <summary>
            /// Identifiant du CI.
            /// </summary>
            public int CiId { get; set; }

            /// <summary>
            /// Identifiant de la tâche parente de niveau 3.
            /// </summary>
            public int Tache3Id { get; set; }

            /// <summary>
            /// Liste des tâches 4 à créer.
            /// </summary>
            public List<Tache4Model> Taches4 { get; set; }
        }

        #endregion
        #region Tache4Model

        /// <summary>
        /// Représente une tâche 4 a créer.
        /// </summary>
        public class Tache4Model
        {
            /// <summary>
            /// Code de la nouvelle tâche.
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Libellé de la nouvelle tâche.
            /// </summary>
            public string Libelle { get; set; }
        }

        #endregion
        #region ResultModel

        /// <summary>
        /// Représente le résultat de la création des tâches.
        /// </summary>
        public class ResultModel : ErreurResultModel
        {
            /// <summary>
            /// Erreurs trouvées ou null si pas d'erreur.
            /// </summary>
            public List<string> Erreurs { get; set; }

            /// <summary>
            /// Identifiant des tâches 4 créées ou null en cas d'erreur.
            /// </summary>
            public List<int> Taches4CreatedIds { get; set; }
        }

        #endregion

    }
}
