using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Budget.Avancement
{
    public class AvancementSaveModel
    {
        public int BudgetId { get; set; }

        public int CiId { get; set; }

        public int DeviseId { get; set; }

        public int Periode { get; set; }

        public ICollection<AvancementTacheSaveModel> ListTaches { get; set; }

        public ICollection<AvancementTache4SaveModel> ListAvancementT4 { get; set; }
    }

    public class AvancementTacheSaveModel
    {
        public int TacheId { get; set; }

        public string CommentaireAvancement { get; set; }
    }


    public class AvancementTache4SaveModel
    {
        /// <summary>
        /// Identifiant d.
        /// </summary>
        public int BudgetT4Id { get; set; }

        public int TypeAvancement { get; set; }

        public decimal? AvancementPourcent { get; set; }

        public decimal? AvancementQte { get; set; }

        public decimal Quantite { get; set; }

        public decimal? DAD { get; set; }
    }
}
