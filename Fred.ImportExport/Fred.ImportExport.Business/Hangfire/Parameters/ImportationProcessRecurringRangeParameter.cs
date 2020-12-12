using Fred.ImportExport.Entities.ImportExport;
using System;

namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class ImportationProcessRecurringRangeParameter
    {
        public string SocieteCode { get; set; }

        public DateTime? DateDebutComptable { get; set; }

        public DateTime? DateFinComptable { get; set; }

        public string CodeEtablissement { get; set; }

        public string FluxCode { get; set; }

        public string GroupCode { get; set; }
    }
}
