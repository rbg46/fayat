using Fred.Web.Models.Rapport;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    public class AstreintePointageHebdoCell : PointageCell
    {
        public bool HasAstreinte { get; set; }
        public int AstreinteId { get; set; }
        public List<RapportLigneAstreinteModel> ListRapportLigneAstreintes { get; set; }
    }
}
