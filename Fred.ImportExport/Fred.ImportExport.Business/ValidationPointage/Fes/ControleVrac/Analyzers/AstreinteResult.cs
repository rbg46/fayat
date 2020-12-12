using System.Diagnostics;
using Fred.Entities.Rapport;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Analyzers
{
    [DebuggerDisplay("RapportLigne = {RapportLigne?.RapportLigneId} HasAstreinte = {HasAstreinte} Code = {Code} Quantite = {Quantite} ")]
    public class AstreinteResult
    {
        public bool HasAstreinte { get; set; }
        public bool HasSortieAstreinte { get; set; }
        public string Quantite { get; internal set; }
        public RapportLigneEnt RapportLigne { get; internal set; }
        public string Code { get; internal set; }
    }
}
