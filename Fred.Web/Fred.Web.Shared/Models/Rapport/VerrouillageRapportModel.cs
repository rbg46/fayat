using System.Collections.Generic;

namespace Fred.Web.Models.Rapport
{
    public class VerrouillageRapportModel
    {
        public List<RapportModel> RapportList { get; set; }
        public SearchRapportModel Filter { get; set; }
    }
}
