using System;

namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class ImportationProcessParameter
    {
        public int UserId { get; set; }
        public int SocieteId { get; set; }
        public int CiId { get; set; }
        public DateTime DateComptable { get; set; }
        public string CodeEtablissement { get; set; }
        public string FluxCode { get; set; }
    }
}