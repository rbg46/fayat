using System;

namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class RemonteePrimesJobParameter
    {
        public DateTime Periode { get; set; }
        public int UtilisateurId { get; set; }
        public string BackgroundJobId { get; set; }
    }
}