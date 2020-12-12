using Fred.Entities.EcritureComptable;
using Fred.Web.Shared.Models.Valorisation;

namespace Fred.Web.Shared.Models.EcritureComptable
{
    public class EcritureComptableValorisationPointageFayatTpModel
    {
        public ValorisationEcritureComptableODModel Valorisation { get; set; }

        public EcritureComptableFtpDto Pointage { get; set; }
    }
}
