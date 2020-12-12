using System.Collections.Generic;

namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class ExportByRapportsParameter
    {
        public List<int> RapportIds { get; set; }
        public string BackgroundJobId { get; set; }
    }
}