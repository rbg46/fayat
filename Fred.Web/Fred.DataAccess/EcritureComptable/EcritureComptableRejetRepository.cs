using System.Collections.Generic;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.EcritureComptable;
using Fred.EntityFramework;

namespace Fred.DataAccess.EcritureComptable
{

    public class EcritureComptableRejetRepository : FredRepository<EcritureComptableRejetEnt>, IEcritureComptableRejetRepository
    {
        public EcritureComptableRejetRepository(FredDbContext context)
          : base(context)
        {
        }

        public void Insert(List<EcritureComptableRejetEnt> rejetEnts)
        {
            Context.EcritureComptablesRejet.AddRange(rejetEnts);
        }

    }
}
