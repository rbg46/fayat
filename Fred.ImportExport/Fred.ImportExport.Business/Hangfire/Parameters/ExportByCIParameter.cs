using System.Collections.Generic;

namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class ExportByCIParameter
    {
        public List<int> ListCiId { get; set; }
        public string BackgroundJobId { get; set; }
        public int UtilisateurId { get; set; }
    }
}