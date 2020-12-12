using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.EcritureComptable;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fred.DataAccess.EcritureComptable
{
    public class EcritureComptableRepository : FredRepository<EcritureComptableEnt>, IEcritureComptableRepository
    {
        private readonly FredDbContext context;

        public EcritureComptableRepository(ILogManager logManager, IUnitOfWork uow, FredDbContext context)
          : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<EcritureComptableEnt>> GetAsync(int ciId, MonthLimits monthLimits)
        {
            return await context.EcritureComptables
                .Include(x => x.Nature)
                .Where(ec => ec.CiId == ciId && ec.DateComptable.Value >= monthLimits.StartDate && ec.DateComptable.Value <= monthLimits.EndDate)
                .AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<EcritureComptableEnt>> GetByCiIdAndLabelAsync(IEnumerable<int> ciIds, IEnumerable<string> labels)
        {
            return await context.EcritureComptables.Where(ec => ciIds.Contains(ec.CiId) && labels.Contains(ec.Libelle)).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<EcritureComptableEnt>> GetByCommandeIdsAsync(List<int> commandeIds)
        {
            return await context.EcritureComptables.Where(q => q.CommandeId.HasValue && commandeIds.Contains(q.CommandeId.Value)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<EcritureComptableEnt>> GetByFamilleOdIdsAsync(List<int> familleOperationDiverseIds)
        {
            return await context.EcritureComptables.Where(q => familleOperationDiverseIds.Contains(q.FamilleOperationDiverseId)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<EcritureComptableEnt>> GetAllAsync(List<int> ciIds, DateTime dateComptable)
        {
            MonthLimits monthLimitsStart = dateComptable.GetLimitsOfMonth();
            MonthLimits monthLimitsEnd = dateComptable.GetLimitsOfMonth();
            return await context.EcritureComptables.Where(ec => ciIds.Contains(ec.CiId) && ec.DateComptable.Value >= monthLimitsStart.StartDate && ec.DateComptable.Value <= monthLimitsEnd.EndDate).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<EcritureComptableEnt>> GetAllAsync(int ciId, DateTime dateComptableDebut, DateTime dateComptableFin)
        {
            MonthLimits monthLimitsStart = dateComptableDebut.GetLimitsOfMonth();
            MonthLimits monthLimitsEnd = dateComptableFin.GetLimitsOfMonth();
            return await context.EcritureComptables.Where(ec => ec.CiId == ciId && ec.DateComptable.Value >= monthLimitsStart.StartDate && ec.DateComptable.Value <= monthLimitsEnd.EndDate).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<EcritureComptableEnt>> GetListOfNumeroPiecesAsync(List<int> cisOfSociete, MonthLimits monthLimits)
        {
            return await context.EcritureComptables.Where(ec => monthLimits.StartDate <= ec.DateComptable && ec.DateComptable <= monthLimits.EndDate && cisOfSociete.Contains(ec.CiId)).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<EcritureComptableEnt>> GetByCiIdsAndPeriodAsync(List<int> cisOfSociete, MonthLimits monthLimits)
        {
            return await context.EcritureComptables
                .Include(ec => ec.CI)
                .Include(ec => ec.Nature)
                .Include(ec => ec.FamilleOperationDiverse)
                .Where(ec =>
                    cisOfSociete.Contains(ec.CiId)
                    && ec.DateComptable >= monthLimits.StartDate && ec.DateComptable <= monthLimits.EndDate)
                .AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public void InsertListByTransaction(IEnumerable<EcritureComptableEnt> ecritureComptablesToInsert)
        {
            using (IDbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.EcritureComptables.AddRange(ecritureComptablesToInsert);
                    context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    throw new FredRepositoryException(e.Message, e);
                }
            }
        }

        public IReadOnlyList<EcritureComptableEnt> GetByNumerosFacturesSAP(List<string> numeroFacturesSAP)
        {
            return context.EcritureComptables.Where(ec => numeroFacturesSAP.Contains(ec.NumeroFactureSAP)).ToList();
        }

        public async Task<decimal> GetEcritureComptableCumulSumAsync(int ecritureComptableId)
        {
            decimal sum = await context.EcritureComptablesCumul.Where(ecc => ecc.EcritureComptableId == ecritureComptableId)
                                                              .SumAsync(x => x.Montant)
                                                              .ConfigureAwait(false);
            return sum;
        }

        public EcritureComptableEnt GetById(int ecritureComptableId)
        {
            return context.EcritureComptables
                .Include(f => f.FamilleOperationDiverse)
                .Include(n => n.Nature)
                .Where(ec => ec.EcritureComptableId == ecritureComptableId)
                .AsNoTracking()
                .FirstOrDefault();
        }
    }
}
