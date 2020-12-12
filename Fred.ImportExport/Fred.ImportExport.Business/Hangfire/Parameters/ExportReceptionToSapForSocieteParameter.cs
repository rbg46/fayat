using System.Collections.Generic;

namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class ExportReceptionToSapForSocieteParameter
    {
        public int SocieteId { get; set; }
        public List<int> ReceptionIds { get; set; }

        public string JobId { get; set; }
    }
}
