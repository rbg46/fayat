using Fred.Entities.EcritureComptable;
using Fred.Framework.Models;
using Fred.Web.Shared.Models.EcritureComptable;
using System.Collections.Generic;

namespace Fred.Business.EcritureComptable
{
    public interface IEcritureComptableRejetManager : IManager<EcritureComptableRejetEnt>
    {
        void AddRejet(List<EcritureComptableRejetModel> ecritureComptableRejetModel);

        void AddRejets(List<EcritureComptableFayatTpRejetModel> rejets);

        void TreatRejet(IEnumerable<Result<EcritureComptableRetreiveResult>> ecritureComptableDto);
    }
}
