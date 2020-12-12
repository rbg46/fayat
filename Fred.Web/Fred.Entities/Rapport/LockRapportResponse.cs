using System.Collections.Generic;

namespace Fred.Entities.Rapport
{
    public class LockRapportResponse
    {
        public List<RapportEnt> LockedRapports { get; set; }
        public IEnumerable<int> PartialLockedReport { get; set; }
    }
}
