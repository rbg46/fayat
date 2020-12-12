using System;

namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class ImportationProcessRangeParameter
    {
        public int UserId { get; set; }
        public int SocieteId { get; set; }
        public int CiId { get; set; }
        public DateTime DateDebutComptable { get; set; }
        public DateTime DateFinComptable { get; set; }
        public string FluxCode { get; set; }
    }
}