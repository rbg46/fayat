using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RapportPrime;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fred.DataAccess.RapportPrime
{
    public class RapportPrimeLigneRepository : FredRepository<RapportPrimeLigneEnt>, IRapportPrimeLigneRepository
    {
        private readonly FredDbContext context;

        public RapportPrimeLigneRepository(FredDbContext context, ILogManager logMgr) : base(context)
        {
            this.context = context;
        }

        public IQueryable<RapportPrimeLigneEnt> GetDefaultQuery()
        {
            return context.RapportPrimeLigne.Include(o => o.Ci.Organisation)
                          .Include(o => o.Ci.EtablissementComptable.Societe)
                          .Include(o => o.Ci.Societe)
                          .Include(o => o.Personnel.Societe)
                          .Include(o => o.Personnel.Ressource)
                          .Include(o => o.Personnel.EtablissementPaie)
                          .Include(o => o.Personnel.EtablissementRattachement)
                          .Include(o => o.RapportPrime)
                          .Include(o => o.ListPrimes).ThenInclude(ooo => ooo.Prime)
                          .Include(o => o.ListAstreintes).ThenInclude(ooo => ooo.Astreinte)
                          .Where(o => !o.DateSuppression.HasValue);
        }

        public IEnumerable<RapportPrimeLigneEnt> GetRapportPrimeLigneVerrouillesByUserId(int userid, int annee, int mois)
        {
            IQueryable<RapportPrimeLigneEnt> qryRapportPrimeLignes = GetDefaultQuery().Include(o => o.RapportPrime);

            IEnumerable<RapportPrimeLigneEnt> rapportLignePrimeVerrouillesByUserId = qryRapportPrimeLignes
              .Where(p => (p.RapportPrime != null && p.RapportPrime.DateRapportPrime.Year == annee && p.RapportPrime.DateRapportPrime.Month == mois)
                          && (p.AuteurVerrouId != null && p.AuteurVerrouId == userid))
              .Distinct();

            return rapportLignePrimeVerrouillesByUserId;
        }

        public IEnumerable<RapportPrimeLigneEnt> SearchRapportPrimeLigneWithFilter(Func<RapportPrimeLigneEnt, bool> predicateWhere)
        {
            IQueryable<RapportPrimeLigneEnt> queryableRapportPrimeLignes = GetDefaultQuery();
            IEnumerable<RapportPrimeLigneEnt> rapportPrimeLigne = queryableRapportPrimeLignes
                                      .Where(predicateWhere)
                                      .OrderBy(x => x.RapportPrime.DateRapportPrime);

            List<RapportPrimeLigneEnt> returnValue = rapportPrimeLigne.ToList();

            return returnValue;
        }

        public async Task<int> GetRapportLignePrimeIdAsync(int primeId)
        {
            return await context.RapportLignePrimes
                .Where(s => s.PrimeId == primeId)
                .Select(s => s.RapportLignePrimeId)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(RapportPrimeLigneEnt rapportPrimeLigne)
        {
            await context.RapportPrimeLigne.AddAsync(rapportPrimeLigne);
        }

        public async Task<List<RapportPrimeLigneEnt>> GetListAsync(List<int> ids)
        {
            return await context.RapportPrimeLigne
                .Where(r => ids.Contains(r.RapportPrimeLigneId))
                .ToListAsync();
        }

        public async Task<List<RapportPrimeLigneEnt>> GetListWithLinkedPropertiesAsync(IEnumerable<int> ids)
        {
            return await context.RapportPrimeLigne
                .Where(r => ids.Contains(r.RapportPrimeLigneId))
                .Include(r => r.ListAstreintes)
                .Include(r => r.ListPrimes)
                .ToListAsync();
        }

        public void UpdateRapportPrimeLigne(RapportPrimeLigneEnt lineToUpdate)
        {
            context.RapportPrimeLigne.Update(lineToUpdate);
        }
    }
}
