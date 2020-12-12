using System;
using Fred.Entities.ValidationPointage;

namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class RemonteeVracJobParameter
    {
        public int RemonteeVracId { get; set; }
        public DateTime Periode { get; set; }
        public int UtilisateurId { get; set; }
        public PointageFiltre Filtre { get; set; }
        public string BackgroundJobId { get; set; }
    }
}