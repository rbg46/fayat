using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport
{
    public class ChekingPointingMonthModel

    {
        public string InfoLabelle { get; set; }

        public string InfoCI { get; set; }

        public string EtatMachine { get; set; }

        public Dictionary<int, double> DayWorks { get; set; }
    }
}
