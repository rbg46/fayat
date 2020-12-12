using Fred.Entities.Rapport;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Analyzers
{
    public class PointageTypeEtudeResult
    {
        public RapportLigneTacheEnt RapportLigneTache { get; internal set; }
        public RapportLigneEnt RapportLigne { get; internal set; }
        public string Code { get; internal set; }
        public string Quantite { get; internal set; }
        public bool IsFistResult { get; internal set; }
    }
}
