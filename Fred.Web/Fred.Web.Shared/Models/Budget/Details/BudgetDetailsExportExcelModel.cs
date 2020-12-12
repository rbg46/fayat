namespace Fred.Web.Shared.Models.Budget.Details
{
    /// <summary>
    /// Représente le model de l'excel détaillant le budget pour l'export analyse.
    /// Actuellement un template de l'export excel n'est pas capable de gérer des sous propriété 
    /// c'est à dire qu'il est impossible dans le template de dire A.Prop1.SousProp1.
    /// Cette objet mets donc tout à plat, mais certaine propriétés seront nulles pour avoir le résultat suivant 
    /// 
    ///    T1 T2 T3 T4 [Qte] [Unite] [Pu]€ [Montant]€ 
    ///    [NULL] [NULL] [NULL] [NULL] [NULL] [NULL] R1 R2 R3 [QteRessource] ....
    ///    [NULL] [NULL] [NULL] [NULL] [NULL] [NULL] R1 R2 R3 [QteRessource] ....
    /// </summary>
    public class BudgetDetailsExportExcelModel
    {
        public string CiCodeLibelle { get; set; }
        public string CodeLibelleT1 { get; set; }
        public string CodeLibelleT2 { get; set; }
        public string CodeLibelleT3 { get; set; }
        public string CodeLibelleT4 { get; set; }

        public decimal? QuantiteT4 { get; set; }
        public string UniteT4 { get; set; }

        public decimal? PuT4 { get; set; }
        public decimal? MontantT4 { get; set; }

        public string LibelleChapitre { get; set; }
        public string LibelleSousChapitre { get; set; }
        public string LibelleRessource { get; set; }

        public decimal? QuantiteRessourceT4 { get; set; }

        public string UniteRessourceT4 { get; set; }

        public decimal? PuRessourceT4 { get; set; }

        public decimal? MontantRessourceT4 { get; set; }

        public decimal? QuantiteBaseRessourceSd { get; set; }

        public string UniteBaseRessourceSd { get; set; }
        public decimal? QuantiteRessourceSd { get; set; }
        public string UniteRessourceSd { get; set; }
        public decimal? PuRessourceSd { get; set; }
        public decimal? MontantRessourceSd { get; set; }
        public string Commentaire { get; set; }
    }
}
