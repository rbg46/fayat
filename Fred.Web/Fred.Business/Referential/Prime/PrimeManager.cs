using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.Web.Models.Referential;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Gestionnaire des primes
    /// </summary>
    public class PrimeManager : Manager<PrimeEnt, IPrimeRepository>, IPrimeManager
    {
        private readonly IGroupeRepository groupeRepository;
        private readonly ICIPrimeRepository cIPrimeRepository;
        private readonly IRapportPrimeLignePrimeRepository rapportPrimeLignePrimeRepository;
        private readonly IRapportPrimeLigneRepository rapportPrimeLigneRepository;
        private readonly IPointageAnticipePrimeRepository pointageAnticipePrimeRepository;
        private readonly IUnitOfWork uow;
        private readonly IUtilisateurManager utilisateurMgr;
        private readonly ISocieteManager societeManager;
        private readonly ISepService sepService;

        public PrimeManager(
            IUnitOfWork uow,
            IPrimeRepository primeRepo,
            IUtilisateurManager utilisateurMgr,
            ISocieteManager societeManager,
            ISepService sepService,
            IGroupeRepository groupeRepository,
            ICIPrimeRepository cIPrimeRepository,
            IRapportPrimeLignePrimeRepository rapportPrimeLignePrimeRepository,
            IRapportPrimeLigneRepository rapportPrimeLigneRepository,
            IPointageAnticipePrimeRepository pointageAnticipePrimeRepository)
              : base(uow, primeRepo)
        {
            this.groupeRepository = groupeRepository;
            this.cIPrimeRepository = cIPrimeRepository;
            this.rapportPrimeLignePrimeRepository = rapportPrimeLignePrimeRepository;
            this.rapportPrimeLigneRepository = rapportPrimeLigneRepository;
            this.pointageAnticipePrimeRepository = pointageAnticipePrimeRepository;
            this.uow = uow;
            this.utilisateurMgr = utilisateurMgr;
            this.societeManager = societeManager;
            this.sepService = sepService;
        }

        public IEnumerable<PrimeEnt> GetPrimesList()
        {
            return this.Repository.GetPrimesList();
        }

        public IEnumerable<PrimeEnt> GetPrimesListSync()
        {
            return this.Repository.GetPrimesListSync();
        }

        public IEnumerable<PrimeEnt> GetActivesPrimesList()
        {
            return this.Repository.GetActivesPrimesList();
        }

        public PrimeEnt GetNewPrime(int societeId)
        {
            return new PrimeEnt { Actif = true, SocieteId = societeId };
        }

        public async Task<PrimeEnt> GetPrimeByIdAsync(int primeId)
        {
            var prime = await Repository.GetPrimeByIdAsync(primeId);

            if (prime == null)
            {
                throw new FredBusinessNotFoundException("La prime n'existe pas");
            }

            return prime;
        }

        public async Task<bool> IsPrimeExistsByCodeAsync(int idCourant, string code, int societeId)
        {
            return await Repository.IsPrimeExistAsync(code, idCourant, societeId);
        }

        public async Task<PrimeEnt> AddPrimeAsync(PrimeEnt primeEnt)
        {
            if (await Repository.IsPrimeExistAsync(primeEnt.Code, primeEnt.PrimeId, primeEnt.SocieteId))
            {
                throw new FredBusinessException("Une prime avec ce code existe déjà");
            }

            if (primeEnt.SocieteId.HasValue)
            {
                primeEnt.GroupeId = (await groupeRepository.GetGroupebySocieteIdAsync(primeEnt.SocieteId.Value))?.GroupeId;
            }

            await Repository.AddPrimeAsync(primeEnt);
            await uow.SaveAsync();

            return primeEnt;
        }

        public async Task UpdatePrimeAsync(PrimeModel primeModel)
        {
            var prime = await GetPrimeByIdAsync(primeModel.PrimeId);

            prime.Code = primeModel.Code;
            prime.Libelle = primeModel.Libelle;
            prime.PrimeType = primeModel.PrimeType;
            prime.TargetPersonnel = primeModel.TargetPersonnel;
            prime.NombreHeuresMax = primeModel.NombreHeuresMax.GetValueOrDefault();
            prime.SeuilMensuel = primeModel.SeuilMensuel;
            prime.Actif = primeModel.Actif;
            prime.PrimePartenaire = primeModel.PrimePartenaire;
            prime.Publique = primeModel.Publique;
            prime.SocieteId = primeModel.SocieteId;
            prime.IsLinkedToCI = primeModel.IsLinkedToCI;
            prime.GroupeId = primeModel.GroupeId;
            prime.MultiPerDay = primeModel.MultiPerDay;
            prime.IsCadre = primeModel.IsCadre;
            prime.IsOuvrier = primeModel.IsOuvrier;
            prime.IsETAM = primeModel.IsETAM;

            await Repository.UpdatePrimeAsync(prime);
            await uow.SaveAsync();
        }

        public async Task DeletePrimeAsync(int primeId)
        {
            var prime = await GetPrimeByIdAsync(primeId);

            if (await Repository.IsPrimeUsedAsync(prime.PrimeId, await BuildStoredProceduresDependanceParameter(prime.PrimeId)))
            {
                throw new FredBusinessException("Impossible de supprimer la prime, elle est utilisée");
            }

            await Repository.DeletePrimeAsync(prime);
            await uow.SaveAsync();
        }

        public async Task<IEnumerable<PrimeEnt>> SearchPrimeAllWithSearchPrimeTextAsync(int societeId, string text, SearchPrimeEnt filters)
        {
            return await Repository.SearchPrimeAllWithSearchPrimeTextAsync(societeId, text, filters);
        }

        public IEnumerable<PrimeEnt> SearchPrimeWithFilters(string text, SearchPrimeEnt filters)
        {
            return this.Repository.SearchPrimeWithFilters(GetPredicate(text, filters));
        }

        public IEnumerable<PrimeEnt> GetSyncPrimes(DateTime lastModification = default(DateTime))
        {
            var currentUser = this.utilisateurMgr.GetContextUtilisateur();

            return Repository.GetPrimes(currentUser.Personnel.SocieteId, lastModification);
        }

        public IEnumerable<CIPrimeEnt> GetCIPrimeList()
        {
            return this.Repository.GetCiPrimeList();
        }

        public IEnumerable<PrimeEnt> GetPrimesListForCI(int ciId)
        {
            return this.Repository.GetPrimesListForCi(ciId);
        }

        public CIPrimeEnt GetCIPrimeByPrimeIdAndCiId(int primeId, int ciId)
        {
            return this.Repository.GetCIPrimeByPrimeIdAndCiId(primeId, ciId);
        }

        public void ManageCIPrime(int ciId, IEnumerable<CIPrimeEnt> ciPrimeList)
        {
            List<int> existingPrimeIdList = GetPrimesIdsByCiId(ciId);
            List<int> ciPrimeIdList = ciPrimeList.Select(x => x.PrimeId).ToList();

            foreach (int primeBddId in existingPrimeIdList)
            {
                if (!ciPrimeIdList.Contains(primeBddId))
                {
                    DeleteCIPrimeById(primeBddId, ciId);
                }
            }

            foreach (int primeId in ciPrimeIdList)
            {
                if (!existingPrimeIdList.Contains(primeId))
                {
                    AddCIPrimes(primeId, ciId);
                }
            }
        }

        public List<int> GetPrimesIdsByCiId(int ciId)
        {
            return this.Repository.GetPrimesIdsByCiId(ciId);
        }

        public void AddCIPrimes(int primeId, int ciId)
        {
            var ciPrime = new CIPrimeEnt();
            ciPrime.CiId = ciId;
            ciPrime.PrimeId = primeId;
            ciPrime.AuteurCreationId = utilisateurMgr.GetContextUtilisateurId();
            ciPrime.DateCreation = DateTime.UtcNow;
            this.Repository.AddCiPrimes(ciPrime);
        }

        public void DeleteCIPrimeById(int primeId, int ciId)
        {
            this.Repository.DeleteCIPrimeById(primeId, ciId);
        }

        public IEnumerable<PrimeEnt> SearchPrimesAllowedByCI(string text, int societeId, int ciId)
        {
            return this.Repository.SearchPrimesAllowedByCi(text, societeId, ciId);
        }

        public async Task<IEnumerable<PrimeEnt>> SearchLightAsync(string text, int page, int pageSize, int? societeId, int? ciId, bool isRapportPrime)
        {
            Expression<Func<PrimeEnt, bool>> predicate = null;
            IQueryable<PrimeEnt> query = null;

            if (isRapportPrime)
            {
                predicate = p => p.PrimeType == ListePrimeType.PrimeTypeMensuelle;
            }
            else
            {
                predicate = p => ((string.IsNullOrEmpty(text) || p.Code.Contains(text) || p.Libelle.Contains(text))
                        && p.Actif
                        && (p.PrimeType == ListePrimeType.PrimeTypeHoraire || p.PrimeType == ListePrimeType.PrimeTypeJournaliere));
            }

            if (societeId.HasValue)
            {
                var societe = societeManager.GetSocieteById(societeId.Value, new List<Expression<Func<SocieteEnt, object>>> { x => x.AssocieSeps });
                if (sepService.IsSep(societe))
                {
                    query = Repository.SearchLightSEP(text, societe, ciId, predicate);
                }
                else
                {
                    if (isRapportPrime)
                    {
                        query = (await Repository.SearchPrimeAllWithFiltersAsync(societeId.GetValueOrDefault(), predicate)).AsQueryable();
                    }
                    else
                    {
                        query = Repository.SearchLightBaseAsync(text, societeId, ciId, predicate);
                    }
                }
            }

            query = FilterGroupe(query);

            return query.OrderBy(p => p.Code).Skip((page - 1) * pageSize).Take(pageSize);
        }

        private IQueryable<PrimeEnt> FilterGroupe(IQueryable<PrimeEnt> query)
        {
            var user = utilisateurMgr.GetContextUtilisateur();
            var userGroupeId = user.Personnel?.Societe?.Groupe?.GroupeId;
            if (userGroupeId.HasValue && !user.SuperAdmin)
            {
                query = query.Where(p => !p.GroupeId.HasValue || userGroupeId == p.GroupeId);
            }

            return query;
        }

        public SearchPrimeEnt GetDefaultFilter()
        {
            var recherche = new SearchPrimeEnt();
            recherche.Code = true;
            recherche.Libelle = true;
            recherche.Actif = false;
            return recherche;
        }

        private Expression<Func<PrimeEnt, bool>> GetPredicate(string text, SearchPrimeEnt filters)
        {
            if (string.IsNullOrEmpty(text))
            {
                return p => !filters.Actif || p.Actif;
            }

            return p => (filters.Code && p.Code.Contains(text)
                         || filters.Libelle && p.Libelle.Contains(text))
                        && (!filters.Actif || p.Actif);
        }

        public async Task<bool> IsPrimeUsedAsync(int primeId)
        {
            return await Repository.IsPrimeUsedAsync(primeId, await BuildStoredProceduresDependanceParameter(primeId));
        }

        private async Task<string> BuildStoredProceduresDependanceParameter(int primeId)
        {
            int ciPrimeID = await cIPrimeRepository.GetCIPrimeIdAsync(primeId);
            int pointageAnticipePrimeId = await pointageAnticipePrimeRepository.GetPointageAnticipePrimeIdAsync(primeId);

            if (ciPrimeID != default || pointageAnticipePrimeId != default)
            {
                var rplId = await rapportPrimeLigneRepository.GetRapportLignePrimeIdAsync(primeId);
                var rplpId = await rapportPrimeLignePrimeRepository.GetRapportPrimeLignePrimeIdAsync(primeId);

                return $"'FRED_CI_PRIME',{ciPrimeID} | 'FRED_POINTAGE_ANTICIPE_PRIME',{pointageAnticipePrimeId} | 'FRED_RAPPORT_LIGNE_PRIME',{rplId}  | 'FRED_RAPPORTPRIME_LIGNE_PRIME',{rplpId}";
            }

            return string.Empty;
        }

        public IEnumerable<PrimeEnt> SearchByGroupe(int groupeId, int societeId, bool? isEtamIac, string text, int page, int pageSize, int? ciId, bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null)
        {
            if (ciId.HasValue)
            {
                return this.Repository.SearchByGroupeAndCiId(groupeId, societeId, isEtamIac, text, page, pageSize, ciId.Value, isOuvrier, isEtam, isCadre);
            }

            return this.Repository.SearchByGroupe(groupeId, societeId, isEtamIac, text, page, pageSize, isOuvrier, isEtam, isCadre);
        }
    }
}
