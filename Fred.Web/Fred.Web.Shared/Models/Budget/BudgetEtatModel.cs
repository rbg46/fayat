
namespace Fred.Web.Shared.Models.Budget
{
    /// <summary>
    /// 
    /// </summary>
    public class BudgetEtatModel
    {
        public BudgetEtatModel(string code, string libelle)
        {
            this.Code = code;
            this.Libelle = libelle;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Libelle { get; set; }


    }
}
