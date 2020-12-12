using Fred.Entities.Rapport;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Analyzers
{
    public class PrimeResult
    {
        public RapportLignePrimeEnt RapportLignePrime { get; internal set; }
        public RapportLigneEnt RapportLigne { get; internal set; }
        public string Code { get; internal set; }
        public string Libelle { get; internal set; }
        public string Unite { get; internal set; }
        public string Quantite { get; internal set; }
    }
}
