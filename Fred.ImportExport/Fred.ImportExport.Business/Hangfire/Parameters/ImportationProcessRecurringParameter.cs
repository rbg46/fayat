using System;

namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class ImportationProcessRecurringParameter
    {
        public string SocieteCode { get; set; }

        public DateTime? DateComptable { get; set; }

        public string CodeEtablissement { get; set; }

        public string FluxCode { get; set; }

        public string GroupCode { get; set; }
    }
}
