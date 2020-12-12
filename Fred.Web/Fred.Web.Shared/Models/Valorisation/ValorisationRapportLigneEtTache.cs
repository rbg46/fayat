using Fred.Entities.Rapport;

namespace Fred.Web.Shared.Models.Valorisation
{
    public class ValorisationRapportLigneEtTache
    {
        public RapportLigneEnt RapportLigne { get; set; }
        public RapportLigneTacheEnt RapportLigneTache { get; set; }
    }
}
