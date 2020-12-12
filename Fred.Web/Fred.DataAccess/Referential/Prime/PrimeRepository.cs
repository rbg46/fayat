using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using static Fred.Entities.Constantes;

namespace Fred.DataAccess.Referential.Prime
{
    public class PrimeRepository : FredRepository<PrimeEnt>, IPrimeRepository
    {
        private readonly ILogManager logManager;

        public PrimeRepository(FredDbContext context, ILogManager logManager)
            : base(context)
        {
            this.logManager = logManager;
        }

        public async Task<bool> IsPrimeUsedAsync(int primeId, string dependanceParameter)
        {
            var result = new SqlParameter("@resu", SqlDbType.Int);
            result.Direction = ParameterDirection.Output;

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@origTableName", "FRED_PRIME"),
                new SqlParameter("@exclusion", string.Empty),
                new SqlParameter("@dependance", dependanceParameter),
                new SqlParameter("@origineId", primeId),
                new SqlParameter("@delimiter", '|'),
                result
            };

            await Context.Database.ExecuteSqlCommandAsync("VerificationDeDependance @origTableName, @exclusion, @dependance, @origineId, @delimiter, @resu OUT", parameters);

            return (int)result.Value != 0;
        }

        public IEnumerable<PrimeEnt> GetPrimesList()
        {
            foreach (PrimeEnt prime in Context.Primes)
            {
                yield return prime;
            }
        }

        public IEnumerable<PrimeEnt> GetPrimesListSync()
        {
            return Context.Primes.AsNoTracking();
        }

        public IEnumerable<PrimeEnt> GetActivesPrimesList()
        {
            foreach (PrimeEnt prime in Context.Primes.Where(p => p.Actif))
            {
                yield return prime;
            }
        }

        public IEnumerable<PrimeEnt> GetPrimesListForCi(int ciId)
        {
            CIEnt ci = Context.CIs
                .Include(c => c.Societe.TypeSociete)
                .Where(x => x.CiId == ciId)
                .Include(x => x.EtablissementComptable)
                .Single();

            if (ci.Societe.TypeSociete.Code == TypeSociete.Sep)
            {
                return GetPrimesListForCiSep(ci);
            }

            return GetPrimesListForCiHorsSep(ci);
        }

        public IEnumerable<PrimeEnt> SearchPrimesAllowedByCi(string text, int societeId, int ciId)
        {
            var query = from p in Context.Primes
                        join ciPrime in Context.CIPrimes on p.PrimeId equals ciPrime.PrimeId into gj
                        from subCiPrime in gj.DefaultIfEmpty()
                        where p.Actif
                        where p.SocieteId.Equals(societeId)
                        where p.Publique || subCiPrime.CiId == ciId
                        where string.IsNullOrEmpty(text) || p.Code.Contains(text) || p.Libelle.Contains(text)
                        select p;

            return query;
        }

        public async Task<PrimeEnt> GetPrimeByIdAsync(int primeId)
        {
            return await Context.Primes.FindAsync(primeId);
        }

        public async Task<bool> IsPrimeExistAsync(string code, int? idCourant = null, int? societeId = null)
        {
            var filteredPrimes = Context.Primes.Where(p => p.Code == code);

            if (idCourant != null && societeId != null)
            {
                return await filteredPrimes
                    .Where(p => p.SocieteId.HasValue
                        && p.SocieteId.Value == societeId
                        && (idCourant == 0 || p.PrimeId != idCourant))
                    .AnyAsync();
            }

            return await filteredPrimes.AnyAsync();
        }

        public async Task AddPrimeAsync(PrimeEnt primeEnt)
        {
            await Context.Primes.AddAsync(primeEnt);
        }

        public async Task UpdatePrimeAsync(PrimeEnt primeEnt)
        {
            Context.Update(primeEnt);
        }

        public async Task DeletePrimeAsync(PrimeEnt prime)
        {
            Context.Primes.Remove(prime);
        }

        public async Task<IEnumerable<PrimeEnt>> SearchPrimeAllWithSearchPrimeTextAsync(int societeId, string text, SearchPrimeEnt filters)
        {
            return await Context.Primes
                .Where(p => p.SocieteId == societeId
                    && p.IsPrimeAstreinte == null
                    && (!filters.Actif || p.Actif)
                    && (string.IsNullOrEmpty(text)
                        || filters.Code && p.Code.Contains(text)
                        || filters.Libelle && p.Libelle.Contains(text)))
                .OrderBy(s => s.Code)
                .ToListAsync();
        }

        public async Task<IEnumerable<PrimeEnt>> SearchPrimeAllWithFiltersAsync(int societeId, Expression<Func<PrimeEnt, bool>> predicate)
        {
            return await Context.Primes
                .Where(p => p.SocieteId == societeId
                    && p.IsPrimeAstreinte == null)
                .Where(predicate)
                .OrderBy(s => s.Code)
                .ToListAsync();
        }

        public IEnumerable<PrimeEnt> SearchPrimeWithFilters(Expression<Func<PrimeEnt, bool>> predicate)
        {
            return Context.Primes.Where(s => s.Actif && s.IsPrimeAstreinte == null).Where(predicate).OrderBy(s => s.Code);
        }

        public IEnumerable<PrimeEnt> GetPrimes(int? societeId, DateTime lastModification)
        {
            var result = Context.Primes.Where(x => x.SocieteId == societeId);

            if (lastModification != default(DateTime))
            {
                result.Where(t => t.DateModification >= lastModification);
            }

            return result.ToList();
        }

        public IEnumerable<CIPrimeEnt> GetCiPrimeList()
        {
            return Context.CIPrimes.ToList();
        }

        public List<int> GetPrimesIdsByCiId(int ciId)
        {
            return Context.CIPrimes
                          .Where(p => p.CiId == ciId)
                          .Select(p => p.PrimeId)
                          .ToList();
        }

        public CIPrimeEnt GetCIPrimeByPrimeIdAndCiId(int primeId, int ciId)
        {
            return Context.CIPrimes.Where(c => c.PrimeId == primeId && c.CiId == ciId).AsNoTracking().FirstOrDefault();
        }

        public void AddCiPrimes(CIPrimeEnt ciPrime)
        {
            try
            {
                Context.CIPrimes.Add(ciPrime);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        public void DeleteCIPrimeById(int primeId, int ciId)
        {
            CIPrimeEnt ciPrime = Context.CIPrimes.FirstOrDefault(cm => cm.PrimeId == primeId && cm.CiId == ciId);

            if (ciPrime == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.CIPrimes.Remove(ciPrime);

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        public IEnumerable<PrimeEnt> SearchByGroupe(int groupeId, int societeId, bool? isEtamIac, string text, int page, int pageSize, bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null)
        {
            return Context.Primes.Where(p => p.Actif)
               .Where(p => p.GroupeId != null && p.GroupeId == groupeId && p.SocieteId == societeId)
               .Where(p => !isCadre.HasValue || (isCadre.HasValue && p.IsCadre == isCadre.HasValue))
               .Where(p => !isOuvrier.HasValue || (isOuvrier.HasValue && p.IsOuvrier == isOuvrier.HasValue))
               .Where(p => !isEtam.HasValue || (isEtam.HasValue && p.IsETAM == isEtam.HasValue))
               .Where(p => string.IsNullOrEmpty(text) || p.Code.Contains(text) || p.Libelle.Contains(text))
               .Where(p => p.IsPrimeAstreinte == null)
               .Where(p => p.TargetPersonnel == (isEtamIac == true ? TargetPersonnel.EtamIac : TargetPersonnel.Ouvrier) || p.TargetPersonnel == TargetPersonnel.All)
               .OrderBy(p => p.Code)
               .Skip((page - 1) * pageSize)
               .Take(pageSize);
        }

        public IEnumerable<PrimeEnt> SearchByGroupeAndCiId(int groupeId, int societeId, bool? isEtamIac, string text, int page, int pageSize, int ciId, bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null)
        {
            List<PrimeEnt> primes = new List<PrimeEnt>();
            List<PrimeEnt> privatePrimes = new List<PrimeEnt>();

            // Get all private primes for current CI
            privatePrimes = GetPrivateBonusForCurrentCi(groupeId, societeId, isEtamIac, text, page, pageSize, ciId).ToList();

            // Get all public primes
            primes = GetPublicBonus(groupeId, societeId, text, page, pageSize).ToList();

            //Add private (if exist) to the other primes
            if (privatePrimes?.Count > 0)
            {
                primes.AddRange(privatePrimes);
            }

            return primes.Where(p => !isCadre.HasValue || (isCadre.HasValue && p.IsCadre == isCadre.HasValue))
               .Where(p => !isOuvrier.HasValue || (isOuvrier.HasValue && p.IsOuvrier == isOuvrier.HasValue))
               .Where(p => !isEtam.HasValue || (isEtam.HasValue && p.IsETAM == isEtam.HasValue));
        }

        public IQueryable<PrimeEnt> SearchLightSEP(string text, SocieteEnt societe, int? ciId, Expression<Func<PrimeEnt, bool>> predicate)
        {
            List<int> listPrivatePrimeId = null;

            if (ciId.HasValue)
            {
                listPrivatePrimeId = GetPrivatePrimeIdListByCiId(ciId.Value);
            }

            var listSocietePartenaireId = societe.AssocieSeps.Select(a => a.SocieteAssocieeId);
            var listPrimeId = Context.Primes
                .Where(p => p.Actif
                            && !p.IsPrimeAstreinte.HasValue
                            && p.Publique
                            && p.SocieteId.HasValue
                            && listSocietePartenaireId.Contains(p.SocieteId.Value))
                .Select(p => p.PrimeId)
                .ToList();
            listPrimeId.AddRange(listPrivatePrimeId);

            IQueryable<PrimeEnt> query = Context.Primes
                .Include(x => x.Societe)
                .Where(p => listPrimeId.Contains(p.PrimeId)
                            && (string.IsNullOrEmpty(text)
                            || p.Code.Contains(text)
                            || p.Libelle.Contains(text)))
                .Where(predicate);

            return query;
        }

        private List<int> GetPrivatePrimeIdListByCiId(int ciId)
        {
            return Context.CIPrimes
                .Where(cp => cp.CiId == ciId && cp.Prime.Actif && !cp.Prime.Publique && !cp.Prime.IsPrimeAstreinte.HasValue)
                .Select(cp => cp.PrimeId)
                .ToList();
        }

        public IQueryable<PrimeEnt> SearchLightBaseAsync(string text, int? societeId, int? ciId, Expression<Func<PrimeEnt, bool>> predicate)
        {
            IQueryable<PrimeEnt> query = from p in Context.Primes
                                         join ciPrime in Context.CIPrimes on p.PrimeId equals ciPrime.PrimeId into gj
                                         from subCiPrime in gj.DefaultIfEmpty()
                                         where p.Actif
                                         where societeId.HasValue && p.SocieteId == societeId && p.IsPrimeAstreinte == null
                                         where p.Publique || (ciId.HasValue && gj.Any() && subCiPrime.CiId == ciId)
                                         where string.IsNullOrEmpty(text) || p.Code.Contains(text) || p.Libelle.Contains(text)
                                         select p;

            return query.Where(predicate);
        }

        private IEnumerable<PrimeEnt> GetPrimesListForCiHorsSep(CIEnt ci)
        {
            int societeId = ci.EtablissementComptable?.SocieteId ?? ci.SocieteId.Value;

            var query = Context.Primes
                .Include(s => s.Societe)
                .Where(p => p.Actif && p.SocieteId == societeId)
                .OrderBy(p => p.Code)
                .Select(p => new
                {
                    p.PrimeId,
                    p.Code,
                    p.Libelle,
                    p.Publique,
                    p.Actif,
                    p.Societe,
                    IsLinkedToCI = p.CIPrimes.Where(cip => cip.CiId == ci.CiId).Select(cip => cip.PrimeId).Contains(p.PrimeId)
                }).ToList();

            return query.Select(x => new PrimeEnt
            {
                PrimeId = x.PrimeId,
                Code = x.Code,
                Libelle = x.Libelle,
                Publique = x.Publique,
                Actif = x.Actif,
                Societe = x.Societe,
                IsLinkedToCI = x.IsLinkedToCI
            });
        }

        private IEnumerable<PrimeEnt> GetPrimesListForCiSep(CIEnt ci)
        {
            int societeMandataireId = ci.EtablissementComptable?.SocieteId ?? ci.SocieteId.Value;

            var listSocieteId = new List<int>();
            listSocieteId.Add(societeMandataireId);
            listSocieteId.AddRange(Context.AssocieSeps
                .Where(a => a.SocieteId == societeMandataireId)
                .Select(a => a.SocieteAssocieeId)
                .ToList());

            var query = Context.Primes
                .Include(s => s.Societe)
                .Where(p => p.Actif && p.SocieteId.HasValue && listSocieteId.Contains(p.SocieteId.Value))
                .OrderBy(p => p.Code)
                .Select(p => new
                {
                    p.PrimeId,
                    p.Code,
                    p.Libelle,
                    p.Publique,
                    p.Actif,
                    p.Societe,
                    IsLinkedToCI = p.CIPrimes.Where(cip => cip.CiId == ci.CiId).Select(cip => cip.PrimeId).Contains(p.PrimeId)
                }).ToList();

            return query.Select(x => new PrimeEnt
            {
                PrimeId = x.PrimeId,
                Code = x.Code,
                Libelle = x.Libelle,
                Publique = x.Publique,
                Actif = x.Actif,
                Societe = x.Societe,
                IsLinkedToCI = x.IsLinkedToCI
            });
        }

        private IEnumerable<PrimeEnt> GetPrivateBonusForCurrentCi(int groupeId, int societeId, bool? isEtamIac, string text, int page, int pageSize, int ciId)
        {
            return Context.Primes.Where(p => p.Actif)
                .Where(p => p.GroupeId != null && p.GroupeId == groupeId && p.SocieteId == societeId)
                .Where(p => string.IsNullOrEmpty(text) || p.Code.Contains(text) || p.Libelle.Contains(text))
                .Where(p => p.IsPrimeAstreinte == null)
                .Where(p => p.TargetPersonnel == (isEtamIac == true ? TargetPersonnel.EtamIac : TargetPersonnel.Ouvrier) || p.TargetPersonnel == TargetPersonnel.All)
                .Where(p => p.CIPrimes.Any(x => x.CiId == ciId) && !p.Publique)
                .OrderBy(p => p.Code)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        private IEnumerable<PrimeEnt> GetPublicBonus(int groupeId, int societeId, string text, int page, int pageSize)
        {
            return Context.Primes.Where(p => p.Actif)
               .Where(p => p.GroupeId != null && p.GroupeId == groupeId && p.SocieteId == societeId)
               .Where(p => string.IsNullOrEmpty(text) || p.Code.Contains(text) || p.Libelle.Contains(text))
               .Where(p => p.IsPrimeAstreinte == null)
               .Where(p => !p.CIPrimes.Any() && p.Publique)
               .OrderBy(p => p.Code)
               .Skip((page - 1) * pageSize)
               .Take(pageSize);
        }
    }
}
