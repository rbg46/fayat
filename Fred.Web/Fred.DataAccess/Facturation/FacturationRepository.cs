using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Facturation;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fred.DataAccess.Facturation
{
    public class FacturationRepository : FredRepository<FacturationEnt>, IFacturationRepository
    {
        public FacturationRepository(FredDbContext context)
          : base(context)
        {
        }

        public void InsertInMass(IEnumerable<FacturationEnt> facturations)
        {
            using (IDbContextTransaction dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Facturations.AddRange(facturations);
                    Context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    throw new FredRepositoryException(e.Message, e);
                }
            }
        }

        public List<int?> GetIdDepenseAchatWithFacture()
        {
            return Context.Facturations.Select(x => x.DepenseAchatReceptionId).Distinct().ToList();
        }

        public IReadOnlyList<FacturationEnt> GetExistingNumeroFactureSap(List<string> numFacturesSap)
        {
            return Context.Facturations.Where(f => numFacturesSap.Contains(f.NumeroFactureSAP)).AsNoTracking().ToList();
        }

        public IEnumerable<FacturationEnt> GetExistingNumeroFactureSap(int receptionId, decimal montantHt, DateTime dateSaisie)
        {
            return Context.Facturations.Where(x => x.DepenseAchatReceptionId.Value == receptionId && x.MontantHT == montantHt && x.DateSaisie == dateSaisie).AsNoTracking();
        }
    }
}
