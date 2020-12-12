using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Budget
{
    public class BudgetSousDetailSave
    {
        #region Model

        public class Model
        {
            public int BudgetId { get; set; }

            public int BudgetT4Id { get; set; }

            public Tache4Model BudgetT4 { get; set; }

            public List<ItemModel> ItemsChanged { get; set; }

            public List<int> ItemsDeletedId { get; set; }
        }

        #endregion
        #region Tache4Model

        public class Tache4Model
        {
            public int TacheId { get; set; }

            public decimal? MontantT4 { get; set; }

            public decimal? PU { get; set; }

            public decimal? QuantiteDeBase { get; set; }

            public decimal? QuantiteARealiser { get; set; }

            public int? UniteId { get; set; }

            public int VueSD { get; set; }

            public int? TypeAvancement { get; set; }
        }

        #endregion
        #region ItemModel

        public class ItemModel
        {
            public int ViewId { get; set; }

            public int BudgetSousDetailId { get; set; }

            public int RessourceId { get; set; }

            public decimal? QuantiteSD { get; set; }

            public string QuantiteSDFormule { get; set; }

            public decimal? Quantite { get; set; }

            public string QuantiteFormule { get; set; }

            public decimal? PrixUnitaire { get; set; }

            public decimal? Montant { get; set; }

            public string Commentaire { get; set; }

            public int? UniteId { get; set; }
        }

        #endregion
        #region ResultModel

        public class ResultModel : ErreurResultModel
        {
            public int? BudgetT4CreatedId { get; set; }

            public List<ItemCreatedModel> ItemsCreated { get; set; }
        }

        #endregion
        #region ItemCreatedModel

        public class ItemCreatedModel
        {
            public int ViewId { get; private set; }

            public int BudgetSousDetailId { get; private set; }

            public ItemCreatedModel(int viewId, int budgetSousDetailId)
            {
                ViewId = viewId;
                BudgetSousDetailId = budgetSousDetailId;
            }
        }

        #endregion
    }
}
