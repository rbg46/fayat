namespace Fred.Business.Budget.BudgetManager.Services
{
    public class GeneralExpense
    {
        public decimal Pourcentage { get; set; }
        public decimal Budget { get; set; }
        public decimal Recette { get; set; }
        public decimal RecetteCumul { get; set; }
        public decimal Pfa { get; set; }
        public decimal RecetteCumulPrevious { get; set; }
        public decimal Prod { get; set; }
    }
}