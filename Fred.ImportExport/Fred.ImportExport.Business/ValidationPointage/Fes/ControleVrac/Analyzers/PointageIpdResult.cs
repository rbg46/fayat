using Fred.Entities.Rapport;
using Fred.Entities.Referential;
namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Analyzers
{
    public class PointageIpdResult
    {
        public RapportLigneEnt RapportLigne { get; set; }

        public bool IsBigestIpd { get; set; }

    }
}
