using Fred.Entities.ValidationPointage;

namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class ControleVracJobParameter
    {
        public int CtrlPointageId { get; set; }
        public int LotPointageId { get; set; }
        public int UtilisateurId { get; set; }
        public PointageFiltre Filtre { get; set; }
        public string BackgroundJobId { get; set; }
    }
}