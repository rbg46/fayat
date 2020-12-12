using Fred.Entities.ValidationPointage;

namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class ControleVracJobWithEntityParameter
    {
        public ControlePointageEnt ControlePointageEnt { get; set; }
        public int UtilisateurId { get; set; }
        public PointageFiltre Filtre { get; set; }
        public string BackgroundJobId { get; set; }
    }
}