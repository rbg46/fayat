using System.Collections.Generic;

namespace Fred.Web.Models.Rapport
{
    public class VerrouillageRapportResponse
    {
        // Pour FES
        public List<int> PartialLockedReportIds { get; set; }

        // Pour RZB
        public List<int> NotLockableRaportsIds { get; set; }
    }
}
