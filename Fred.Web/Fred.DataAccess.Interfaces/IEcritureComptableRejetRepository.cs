using System.Collections.Generic;
using Fred.Entities.EcritureComptable;

namespace Fred.DataAccess.Interfaces
{
    public interface IEcritureComptableRejetRepository : IFredRepository<EcritureComptableRejetEnt>
    {
        void Insert(List<EcritureComptableRejetEnt> rejetEnts);
    }
}
