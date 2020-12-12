using Fred.Entities.EcritureComptable;
using Fred.Web.Shared.Models.Valorisation;

namespace Fred.Web.Shared.Models.EcritureComptable
{
    public class EcritureComptableValorisationModel
    {
        public EcritureComptableFtpDto Ecriture { get; set; }
        public ValorisationEcritureComptableODModel Valorisation { get; set; }
        public decimal Montant { get; set; }
    }
}
