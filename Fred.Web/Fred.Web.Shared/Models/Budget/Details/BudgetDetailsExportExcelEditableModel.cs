namespace Fred.Web.Shared.Models.Budget.Details
{

    /// <summary>
    /// Représente le model utilisé dans l'export excel editable.
    /// </summary>
    public class BudgetDetailsExportExcelEditableModel
    {
        public string CodeTache { get; set; }
        public string LibelleTache { get; set; }
        public decimal? MontantTache { get; set; }
        public string UniteTache { get; set; }
        public decimal? QuantiteTache { get; set; }
        public decimal? PUTache { get; set; }
        public string Chapitre { get; set; }
        public string Ressource { get; set; }
        public string Commentaire { get; set; }
        public string Unite { get; set; }
        public decimal? QuantiteRessourceT4 { get; set; }
        public decimal? PURessourceT4 { get; set; }
        public decimal? MontantRessourceT4 { get; set; }
        public decimal? QuantiteRessourceSD { get; set; }
        public decimal? PURessourceSD { get; set; }
        public decimal? MontantRessourceSD { get; set; }
    }
}
