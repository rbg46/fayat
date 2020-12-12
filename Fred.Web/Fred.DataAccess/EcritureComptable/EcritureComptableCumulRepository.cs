using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.EcritureComptable;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fred.DataAccess.EcritureComptable
{
    public class EcritureComptableCumulRepository : FredRepository<EcritureComptableCumulEnt>, IEcritureComptableCumulRepository
    {
        public EcritureComptableCumulRepository(FredDbContext context)
          : base(context)
        {
        }

        public async Task<IReadOnlyList<EcritureComptableCumulEnt>> GetEcritureComptableCumulByCiIdAndPartNumberAsync(IEnumerable<int> ciIds, IEnumerable<string> partNumbers)
        {
            return await Context.EcritureComptablesCumul.Where(q => ciIds.Distinct().Contains(q.CiId) && partNumbers.Distinct().Contains(q.NumeroPiece)).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public void InsertListByTransaction(ICollection<EcritureComptableCumulEnt> ecritureComptableCumul)
        {
            using (var dbcontext = new FredDbContext())
            {
                using (IDbContextTransaction dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        // disable detection of changes
                        Context.ChangeTracker.AutoDetectChangesEnabled = false;

                        // Ajout des ecritures comptables          
                        Context.EcritureComptablesCumul.AddRange(ecritureComptableCumul);

                        Context.SaveChanges();
                        dbContextTransaction.Commit();

                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new FredRepositoryException(e.Message, e);
                    }
                    finally
                    {
                        // re-enable detection of changes
                        Context.ChangeTracker.AutoDetectChangesEnabled = true;
                    }
                }
            }
        }
    }
}
