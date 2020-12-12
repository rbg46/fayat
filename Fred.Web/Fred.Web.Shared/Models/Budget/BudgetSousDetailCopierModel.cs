using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Budget
{
    /// <summary>
    /// Modèle de copie de sous-détail.
    /// </summary>
    public class BudgetSousDetailCopier
    {
        /// <summary>
        /// Modèle de copie de sous-détail.
        /// </summary>
        public class Model
        {
            /// <summary>
            /// Identifiant du budget cible.
            /// </summary>
            public int BudgetCibleId { get; set; }

            /// <summary>
            /// Identifiant du CI source.
            /// </summary>
            public int CiSourceId { get; set; }

            /// <summary>
            /// Items à copier.
            /// </summary>
            public List<ItemModel> Items { get; set; }
        }

        /// <summary>
        /// Modèle d'un item à copier.
        /// </summary>
        public class ItemModel
        {
            /// <summary>
            /// Identifiant du budget T4 source.
            /// </summary>
            public int BudgetT4SourceId { get; set; }

            /// <summary>
            /// Budget T4 cible.
            /// </summary>
            public BudgetSousDetailSave.Tache4Model BudgetT4Cible { get; set; }
        }

        /// <summary>
        /// Modèle du résultat de la copie.
        /// </summary>
        public class ResultModel
        {
            /// <summary>
            /// Liste des identifiants des budget T4 créés.
            /// </summary>
            public List<int> BudgetT4sIdCreated { get; private set; } = new List<int>();
        }
    }
}
