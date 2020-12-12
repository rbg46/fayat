using Fred.Entities.Rapport;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Analyzers
{
    public class MajorationResult
    {
        public RapportLigneMajorationEnt RapportLigneMajoration { get; internal set; }
        public string Code { get; internal set; }
        public string Quantite { get; internal set; }
        public RapportLigneEnt RapportLigne { get; internal set; }
    }
}
