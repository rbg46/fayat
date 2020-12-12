using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.RapportPrime.Update
{
    public class RapportPrimeUpdateModel
    {
        public DateTime DateRapportPrime { get; set; }
        public List<RapportPrimeLigneUpdateModel> LinesToCreate { get; set; }
        public List<RapportPrimeLigneUpdateModel> LinesToUpdate { get; set; }
        public List<int> LinesToDelete { get; set; }
    }
}
