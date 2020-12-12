using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Organisation;
using Fred.Business.Organisation.Tree;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Organisation;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Web.Models.CI;
using Fred.Web.Shared.Extentions;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.CI
{
    public abstract class CIManager : Manager<CIEnt, ICIRepository>, ICIManager
    {
        private readonly IEtablissementComptableManager etabComptaManager;
        private readonly IOrganisationManager organisationManager;
        private readonly ISocieteManager societeManager;
        private readonly IUtilisateurManager userManager;
        private readonly IDatesClotureComptableManager dateClotureComptableManager;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly ISepService sepService;
        private readonly IContratInterimaireManager contratInterimaireManager;
        private readonly IAffectationRepository affectationRepo;
        private readonly IMapper mapper;

        protected CIManager(
            IUnitOfWork uow,
            ICIRepository ciRepository,
            ICIValidator validator,
            IEtablissementComptableManager etabComptaManager,
            IOrganisationManager organisationManager,
            ISocieteManager societeManager,
            IUtilisateurManager userManager,
            IDatesClotureComptableManager dateClotureComptableManager,
            IOrganisationTreeService organisationTreeService,
            ISepService sepService,
            IContratInterimaireManager contratInterimaireManager,
            IAffectationRepository affectationRepo,
            IMapper mapper)
            : base(uow, ciRepository, validator)
        {
            this.etabComptaManager = etabComptaManager;
            this.organisationManager = organisationManager;
            this.societeManager = societeManager;
            this.userManager = userManager;
            this.dateClotureComptableManager = dateClotureComptableManager;
            this.organisationTreeService = organisationTreeService;
            this.sepService = sepService;
            this.contratInterimaireManager = contratInterimaireManager;
            this.affectationRepo = affectationRepo;
            this.mapper = mapper;
        }

        #region Gestion des CI

        /// <summary>
        /// Recherche un ci selon les filtres definis
        /// </summary>
        /// <param name="filters">les filtres</param>
        /// <param name="orderBy">les orderby</param>
        /// <param name="includeProperties">les includes</param>
        /// <param name="page">la page corrante</param>
        /// <param name="pageSize">la taille de la page</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Liste de ci</returns>
        public List<CIEnt> Search(List<Expression<Func<CIEnt, bool>>> filters,
                                       Func<IQueryable<CIEnt>, IOrderedQueryable<CIEnt>> orderBy = null,
                                       List<Expression<Func<CIEnt, object>>> includeProperties = null,
                                       int? page = null,
                                       int? pageSize = null,
                                       bool asNoTracking = true)
        {
            return Repository.Search(filters, orderBy, includeProperties, page, pageSize, asNoTracking);
        }

        /// <inheritdoc />
        public IEnumerable<CIEnt> GetCIList(bool onlyChantierFred = false, int? groupeId = null)
        {
            return Repository.Get(onlyChantierFred, groupeId);
        }

        /// <summary>
        /// Retourne le ci par rapport à son identifiant unique 
        /// </summary>
        /// <param name="id">Identifiant Unique</param>
        /// <param name="withSocieteInclude">Indique si on inclut la société</param>
        /// <returns>Renvoie le ci.</returns>
        public CIEnt GetCiById(int id, bool withSocieteInclude = false)
        {
            return Repository.GetCiById(id, withSocieteInclude);
        }

        /// <summary>
        /// Retourne la liste CI en fonction d'une organisation
        /// </summary>
        /// <param name="organisationId">OrganisationId choisie</param>
        /// <param name="period">Période</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Renvoie la liste des CI en fonction de l'organisation</returns>
        public IEnumerable<CIEnt> GetCIList(int organisationId, DateTime? period, int page = 1, int pageSize = 10)
        {
            List<CIEnt> result = new List<CIEnt>();
            int typeOrgaCiId = organisationManager.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeCi);
            int typeOrgaSousCiId = organisationManager.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeSousCi);
            OrganisationEnt o = organisationManager.GetOrganisationById(organisationId);
            // Si l'organisationId passé en paramètre est déjà un CI, on le renvoie directement
            if (o?.TypeOrganisationId == typeOrgaCiId || o?.TypeOrganisationId == typeOrgaSousCiId)
            {
                CIEnt ci = GetCIById(o.CI.CiId, byPassCheckAccess: true);
                ci.IsClosed = (period.HasValue) ? IsCIClosed(o.CI.CiId, period.Value) : default(bool);
                result.Add(ci);
            }
            else
            {
                IEnumerable<OrganisationLightEnt> orgaLightList = organisationManager.GetOrganisationsAvailable(null, new List<int> { typeOrgaCiId, typeOrgaSousCiId }, null, organisationId);
                // Gestion de la pagination
                if (page != default(int) && pageSize != default(int))
                {
                    orgaLightList = orgaLightList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                List<CIEnt> cis = Repository.GetByOrganisationId(orgaLightList.Select(x => x.OrganisationId)).Where(ci => ci != null).ToList();
                foreach (CIEnt ci in cis)
                {
                    // CODEREVIEW : DM : Il faut ajouter un statut "Cloturé" sur le CI, et faire ce calcul une seul fois au moment de la clôture comptable.
                    ci.IsClosed = (period.HasValue) ? IsCIClosed(ci.CiId, period.Value) : default(bool);
                    result.Add(ci);
                }
            }
            return result;
        }

        /// <inheritdoc />
        public IEnumerable<CIEnt> GetCIList(int organisationId)
        {
            return GetCIList(organisationId, null, default(int), default(int));
        }

        /// <summary>
        /// Vérifie si un CI est clôturé pour une période donnée
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="period">Période</param>
        /// <returns>Vrai si le CI est clôturé, sinon faux</returns>
        public bool IsCIClosed(int ciId, DateTime period)
        {
            return dateClotureComptableManager.IsPeriodClosed(ciId, period.Year, period.Month);
        }

        /// <inheritdoc />
        public CIEnt GetCIById(int ciId, int userId = 0, bool byPassCheckAccess = false, bool overrideSocietyByOrganisationParent = false)
        {
            if (!byPassCheckAccess)
            {
                // Si on n'a pas d'utilisateur courant
                if (userManager.GetContextUtilisateurId() == 0 && userId != 0)
                {
                    CheckAccessToEntity(ciId, userId);
                }
                else
                {
                    CheckAccessToEntity(ciId);
                }
            }
            // Récupération du Ci
            CIEnt ci = Repository.Get(ciId);
            // Si le Ci est trouvé, récupération des parents.
            if (ci != null)
            {
                ci.Parents = organisationManager.GetOrganisationParentByOrganisationId(ci.Organisation.OrganisationId);
                if (overrideSocietyByOrganisationParent)
                {
                    ci.Societe = ci.Parents.Single(o => o.Societe != null).Societe;
                    ci.SocieteId = ci.Societe.SocieteId;
                    ci.Societe.Groupe = ci.Parents.Single(o => o.Groupe != null).Groupe;
                    ci.Societe.GroupeId = ci.Societe.Groupe.GroupeId;
                    ci.Societe.Groupe.Pole = ci.Parents.Single(o => o.Pole != null).Pole;
                    //FIX: Le code du pole contient des espaces O_o
                    ci.Societe.Groupe.Pole.Code = ci.Societe.Groupe.Pole.Code.Trim();
                    ci.Societe.Groupe.PoleId = ci.Societe.Groupe.Pole.PoleId;
                }
            }
            return ci;
        }

        /// <summary>
        /// Retourne le ci portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="id">Identifiant du CI à retrouver.</param>    
        /// <returns>CI retrouvé, sinon null.</returns>
        public CIEnt GetCI(int id)
        {
            return Repository.Get(id);
        }

        /// <inheritdoc />
        public CIEnt GetCI(string code, string codeSocieteCompta)
        {
            try
            {
                return Repository.Query().Include(s => s.Societe).Get().FirstOrDefault(x => x.Code == code && x.Societe.CodeSocieteComptable == codeSocieteCompta);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Get Ci by organisation id
        /// </summary>
        /// <param name="organisationId">Organisation id</param>
        /// <returns>CiEnt avec l'identifiant organisation id</returns>
        public CIEnt GetCiByOrganisationId(int organisationId)
        {
            try
            {
                return Repository.GetByOrganisationId(new int[] { organisationId }, false)?.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <inheritdoc />
        public CIEnt AddCI(CIEnt ciEnt)
        {
            int? organisationId;
            if (ciEnt.Sep)
            {
                organisationId = societeManager.GetOrganisationIdBySocieteId(ciEnt.SocieteId);
            }
            else
            {
                organisationId = etabComptaManager.GetOrganisationByEtablissementId(ciEnt.EtablissementComptableId.Value).OrganisationId;
            }
            ciEnt.Organisation = organisationManager.GenerateOrganisation(Constantes.OrganisationType.CodeCi, organisationId);
            BusinessValidation(ciEnt);
            CIEnt newCi = Repository.AddCI(ciEnt);
            Save();

            // Ajoute une devise de référence par défaut (Devise de la Société du CI)
            if (newCi.SocieteId.HasValue)
            {
                if (newCi.Societe == null)
                {
                    newCi.Societe = societeManager.GetSocieteById(newCi.SocieteId.Value);
                }
                DeviseEnt devise = societeManager.GetListSocieteDeviseRef(newCi.Societe);
                var ciDevise = new CIDeviseEnt { CiId = newCi.CiId, DeviseId = devise.DeviseId, Reference = true };
                Repository.AddCIDevise(ciDevise);
                Save();
            }
            return newCi;
        }

        /// <inheritdoc />
        public CIEnt UpdateCI(CIEnt ciEnt)
        {
            BusinessValidation(ciEnt);
            // Si le Ci est trouvé, récupération des parents.
            if (ciEnt != null)
            {
                ciEnt.Parents = organisationManager.GetOrganisationParentByOrganisationId(ciEnt.Organisation.OrganisationId);
            }
            Repository.UpdateCI(ciEnt);
            Save();

            return ciEnt;
        }

        /// <inheritdoc />
        public void DeleteCIById(int id)
        {
            Repository.DeleteCIById(id);
            Save();
        }

        /// <inheritdoc />
        public SearchCIEnt GetFilter()
        {
            SearchCIEnt searchCi = new SearchCIEnt()
            {
                CodeAsc = true,
                ValueText = string.Empty,
                DateOuvertureFrom = null,
                DateFermetureTo = null,
                DateFermetureFrom = null,
                DateOuvertureTo = null,
                CITypeList = { }
            };

            OnGetFilter(searchCi);

            return searchCi;
        }

        /// <summary>
        /// Méthode virtuelle à implémenter dans une classe dérivée pour le multi-société 
        /// </summary>
        /// <param name="searchCi"></param>
        protected virtual void OnGetFilter(SearchCIEnt searchCi)
        {
            // rien à faire par défaut
        }

        /// <summary>
        /// Récupérer la liste des types de CI à mettre dans le filtre de recherche.
        /// </summary>
        /// <returns>Liste des types de CI</returns>
        protected IEnumerable<CITypeSearchEnt> GetCITypesFilterValues()
        {
            return Repository.GetCITypes()?.Select(ciType => new CITypeSearchEnt
            {
                CITypeId = ciType.CITypeId,
                Designation = ciType.Designation,
                RessourceKey = ciType.RessourceKey,
                Selected = false
            });
        }

        /// <summary>
        /// Méthode de recherche des CIs liés à une société
        /// Utilisé dans l'écran de création des contrat intérimaires
        /// </summary>
        /// <param name="text">Le texte de recherche</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>        
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="includeSep">inclusion des SEP</param>
        /// <returns>La liste des CIs</returns>
        public IEnumerable<CIEnt> GetCisOfUserFilteredBySocieteId(string text, int page, int pageSize, int societeId, bool includeSep = true)
        {
            IEnumerable<CIEnt> cis = null;
            UtilisateurEnt user = this.userManager.GetContextUtilisateur();
            List<int> userCIsIds = userManager.GetAllCIbyUser(user.UtilisateurId).ToList();
            if (userCIsIds.Count > 0)
            {
                IRepositoryQuery<CIEnt> query = GetSearchLightQuery(text, userCIsIds);

                bool isGRZB = user.Personnel?.Societe?.Groupe.Code == Constantes.CodeGroupeRZB;
                if (isGRZB)
                {
                    List<int> ciOrganisationIds = GetCisOrganisationsIdBySocieteId(societeId, includeSep, true);

                    query = query.Filter(x => ciOrganisationIds.Contains(x.Organisation.OrganisationId));
                }

                cis = query.GetPage(page, pageSize).ToList();
            }
            return cis ?? new List<CIEnt>();
        }

        public virtual async Task<IEnumerable<CIModel>> SearchLightModelAsync(string text, int page, int pageSize, int? personnelSocieteId = null) 
        {
            IEnumerable<CIEnt> cis = await SearchLightAsync(text, page, pageSize, personnelSocieteId).ConfigureAwait(false);

            IEnumerable<CIModel> cisModel = mapper.Map<IEnumerable<CIModel>>(cis);

            return cisModel;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CIEnt>> SearchLightAsync(string text, int page, int pageSize, int? personnelSocieteId = null)
        {
            IEnumerable<CIEnt> cis = new List<CIEnt>();
            UtilisateurEnt user = await userManager.GetContextUtilisateurAsync().ConfigureAwait(false);
            List<int> userCIsIds = (await userManager.GetAllCIbyUserAsync(user.UtilisateurId).ConfigureAwait(false)).ToList();
            if (userCIsIds.Count > 0)
            {
                IRepositoryQuery<CIEnt> query = GetSearchLightQuery(text, userCIsIds);
                bool isGRZB = user.Personnel?.Societe?.Groupe.Code == Constantes.CodeGroupeRZB;
                if (isGRZB && personnelSocieteId.HasValue)
                {
                    List<int> ciOrganisationIds = GetCisOrganisationsIdBySocieteId(personnelSocieteId.Value);
                    if (ciOrganisationIds.Count > 0)
                    {
                        query = query.Filter(x => ciOrganisationIds.Contains(x.Organisation.OrganisationId));
                    }
                }

                cis = query.GetPage(page, pageSize).ToList();
            }
            return cis;
        }

        /// <summary>
        /// Retourne la requête des CIs, filtrable selon les besoins
        /// </summary>
        /// <param name="text">Le texte de recherche</param>
        /// <param name="ciIdList">La liste des CIs de l'utilisateur</param>
        /// <returns>Retourne la requête générique des CIs</returns>
        private IRepositoryQuery<CIEnt> GetSearchLightQuery(string text, List<int> ciIdList)
        {
            return Repository.Query().Include(e => e.EtablissementComptable).Include(e => e.EtablissementComptable.Societe)
                             .Include(s => s.Societe).Include(s => s.Societe.Groupe).Include(s => s.Societe.TypeSociete).Include(pl => pl.PaysLivraison).Include(pf => pf.PaysFacturation).Include(p => p.Pays).Include(c => c.Organisation).Include(a => a.Affectations).Include(a => a.ResponsableAdministratif)
                             .OrderBy(o => o.OrderBy(ci => ci.Code)).Filter(c => ciIdList.Contains(c.CiId))
                             .Filter(ci => ci.DateFermeture == null || ci.DateFermeture > DateTime.Now).Filter(ci => string.IsNullOrEmpty(text) || ci.Code.Contains(text) || ci.Libelle.Contains(text));
        }

        /// <summary>
        /// SearchLight CI pour Compte Interne SEP
        /// </summary>
        /// <param name="text">Texte à rechercher</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="currentCiId">CI actuel sur lequel on va associer un Compte Interne SEP</param>
        /// <returns>Liste de CI</returns>
        public List<CIEnt> SearchLightCompteInterneSep(string text, int page, int pageSize, int currentCiId)
        {
            List<int> ciOrganisationIds = new List<int>();
            SocieteEnt societeSep = GetSocieteByCIId(currentCiId);
            SocieteEnt societeGerante = sepService.GetSocieteGerante(societeSep.SocieteId);
            IEnumerable<CIEnt> query = new List<CIEnt>();
            if (societeGerante != null)
            {
                OrganisationTree mainOrganisationTree = organisationTreeService.GetOrganisationTree();
                var cis = mainOrganisationTree.GetAllCisOfSociete(societeGerante.SocieteId);
                if (cis?.Any() == true)
                {
                    ciOrganisationIds = cis.Select(x => x.OrganisationId).ToList();
                }
                query = Repository.Query()
                                  .Filter(ci => ci.DateFermeture == null || ci.DateFermeture > DateTime.Now).Filter(ci => string.IsNullOrEmpty(text) || ci.Code.Contains(text) || ci.Libelle.Contains(text)).Filter(x => ciOrganisationIds.Count != 0 && ciOrganisationIds.Contains(x.Organisation.OrganisationId))
                                  .OrderBy(o => o.OrderBy(ci => ci.Code)).GetPage(page, pageSize);
            }
            return query.ToList();
        }

        /// <summary>
        /// Méthode recherche des CI qui sont eligibles au affectation d'un moyen en fonction des rôles de l'utilisateur
        /// </summary>
        /// <param name="text">Le texte de recherche</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>Retourne une liste des référentiels</returns>
        public IEnumerable<CIEnt> CiSearchLightForAffectationMoyen(string text, int page, int pageSize)
        {
            CiForMoyenAffectationsRolesFiltersEnt ciForMoyenAffectationsRolesFilters = GetCiForMoyenAffectationsRolesFilters();
            List<int> ciIdList = userManager.GetAllCIbyUser(userManager.GetContextUtilisateurId()).ToList();
            return Repository.Query()
                                .Include(c => c.EtablissementComptable.Societe)
                                .Include(c => c.Societe)
                                .Include(c => c.Societe.Groupe)
                                .Include(c => c.Organisation)
                                .Include(c => c.Affectations)
                                .Include(c => c.ResponsableAdministratif)
                                .Filter(c => ciIdList.Contains(c.CiId))
                                .Filter(ciForMoyenAffectationsRolesFilters.GetPredicateWhere())
                                .Filter(ci => ci.DateFermeture == null || ci.DateFermeture > DateTime.Now)
                                .Filter(ci => string.IsNullOrEmpty(text) || ci.Code.Contains(text) || ci.Libelle.Contains(text))
                                .OrderBy(o => o.OrderBy(ci => ci.Code))
                                .GetPage(page, pageSize)
                                .AsEnumerable();
        }

        /// <summary>
        /// recuperer la liste de CI afféctés à un personnel
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">page size</param>
        /// <param name="personnelId">personnel id</param>
        /// <param name="societeId">Societe identifiant</param>
        /// <returns>retourne liste des CI d'un personnel donné</returns>
        public async Task<IEnumerable<CIEnt>> SearchLightByPersonnelId(string text, int page, int pageSize, int personnelId, int? societeId = null)
        {
            UtilisateurEnt currentUser = await userManager.GetContextUtilisateurAsync().ConfigureAwait(false);
            if (currentUser.Personnel?.Societe?.Groupe.Code == Constantes.CodeGroupeRZB && societeId.HasValue)
            {
                IEnumerable<CIEnt> ciList = await SearchLightAsync(text, page, pageSize, societeId).ConfigureAwait(false); ;
                return HandleGetRefPersonnelCiAffectation(personnelId, ciList);
            }

            return Repository.SearchLightByPersonnelIdStandard(text, page, pageSize, personnelId);
        }

        /// <inheritdoc />
        public IEnumerable<CIEnt> SearchLightForInterimaire(string text, int page, int pageSize, int interimaireId, DateTime date)
        {
            List<ContratInterimaireEnt> contratsActifInPeriode = contratInterimaireManager.GetContratInterimaireByPersonnelId(interimaireId)
               .Where(c => c.DateDebut.Date <= date.Date && date.Date <= c.DateFin.AddDays(c.Souplesse).Date).ToList();

            ContratInterimaireEnt contratInterimaireMaxDate = contratsActifInPeriode.OrderByDescending(c => c.DateDebut).FirstOrDefault();
            List<int> etablissementComptableIds = contratInterimaireMaxDate.ZonesDeTravail.Select(z => z.EtablissementComptableId).ToList();

            List<int> ciIdList = userManager.GetAllCIbyUser(userManager.GetContextUtilisateurId()).ToList();
            IEnumerable<CIEnt> query = new List<CIEnt>();
            if (ciIdList.Count > 0)
            {
                query = Repository.Query()
                    .Include(e => e.EtablissementComptable.Societe)
                    .Include(s => s.Societe)
                    .Include(pl => pl.PaysLivraison)
                    .Include(pf => pf.PaysFacturation)
                    .Include(p => p.Pays)
                    .Include(t => t.CIType)
                    .Filter(c => ciIdList.Contains(c.CiId) && (c.CiId == contratInterimaireMaxDate.CiId || etablissementComptableIds.Contains(c.EtablissementComptableId.Value)))
                    .Filter(ci => ci.DateFermeture == null || ci.DateFermeture >= date)
                    .Filter(ci => string.IsNullOrEmpty(text) || ci.Code.Contains(text) || ci.Libelle.Contains(text))
                    .OrderBy(o => o.OrderBy(ci => ci.Code)).GetPage(page, pageSize);
            }
            return query;
        }

        /// <summary>
        /// Retourne la liste de CI filtrée en fonction de critères liés au budget
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">page size</param>
        /// <param name="periodeApplication">période de mise en application du budget</param>
        /// <returns>retourne liste des CI filtrés pour le budget</returns>
        public IEnumerable<CIEnt> SearchLightForBudget(string text, int page, int pageSize, int? periodeApplication)
        {
            List<int> ciIdList = userManager.GetAllCIbyUser(userManager.GetContextUtilisateurId()).ToList();
            return Repository.Query()
                    .Filter(ci => ci.DateFermeture == null || ci.DateFermeture > DateTime.Now).Filter(ci => ci.Budgets.Any(x => (x.BudgetEtat.Code == Fred.Entities.Constantes.EtatBudget.EnApplication || x.BudgetEtat.Code == Fred.Entities.Constantes.EtatBudget.Archive) && (!periodeApplication.HasValue || (x.PeriodeDebut <= periodeApplication && (!x.PeriodeFin.HasValue || x.PeriodeFin <= periodeApplication))) && x.DateSuppressionBudget == null))
                    .Filter(c => ciIdList.Contains(c.CiId)).Filter(ci => string.IsNullOrEmpty(text) || ci.Code.Contains(text) || ci.Libelle.Contains(text))
                    .OrderBy(o => o.OrderBy(ci => ci.Code)).GetPage(page, pageSize);
        }

        /// <summary>
        /// Retourne la liste CI du profil paie selon l'organisation choisie
        /// </summary>
        /// <param name="organisationId">OrganisationId choisie</param>
        /// <returns>Renvoie la liste des CI choisie par profil paie.</returns>
        public IEnumerable<int> GetAllCIbyOrganisation(int organisationId)
        {
            int idTypeCi = organisationManager.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeCi);
            int idTypeSousCi = organisationManager.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeSousCi);
            var orgaLightList = organisationManager.GetOrganisationsAvailable(types: new List<int> { idTypeCi, idTypeSousCi }, organisationIdPere: organisationId);
            return Repository.GetCIIdListByOrganisationId(orgaLightList.Select(q => q.OrganisationId));
        }

        /// <summary>
        /// Retourne l'organisationId pour un ci
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <returns>L organisationId</returns>
        public int? GetOrganisationIdByCiId(int ciId)
        {
            return Repository.GetOrganisationIdByCiId(ciId);
        }

        /// <inheritdoc />
        public IEnumerable<CITypeEnt> GetCITypes()
        {
            try
            {
                return Repository.GetCITypes();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne les cis appartenant à un établissement comptable pour picklist
        /// </summary>
        /// <param name="organisationId">identifiant unique de l'organisation de l'établissemet comptable</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">taille de la page</param>
        /// <returns>Liste de Ci appartenant à une société</returns>
        public IEnumerable<OrganisationEnt> SearchLightOrganisationCiByOrganisationPereId(int organisationId, int page, int pageSize)
        {
            IEnumerable<CIEnt> result = Repository.SearchLightOrganisationCiByOrganisationPereId(organisationId, page, pageSize);
            IEnumerable<OrganisationEnt> cis = result.Select(o => o.Organisation);
            return cis;
        }

        /// <summary>
        /// Get Ci by identifiant unique de société
        /// </summary>
        /// <param name="societeId">identifiant unique de société</param>
        /// <returns>Liste de CIs</returns>
        public List<CIEnt> GetCIListBySocieteId(int societeId)
        {
            return Repository.GetCIListBySocieteId(societeId);
        }

        /// <summary>
        /// Get liste CiId by identifiant unique de société
        /// </summary>
        /// <param name="societeId">identifiant unique de société</param>
        /// <returns>Liste de CIs</returns>
        public List<int> GetCiIdListBySocieteId(int societeId)
        {
            return Repository.GetCiIdListBySocieteId(societeId);
        }

        /// <summary>
        /// Get liste ciid par identifiant unique de société pour traitement des exports reception intérimaire
        /// </summary>
        /// <param name="societeId">identifiant unique de société</param>
        /// <returns>Liste de CIs</returns>
        public List<int> GetCiIdListBySocieteIdForInterimaire(int societeId)
        {
            return Repository.GetCiIdListBySocieteIdForInterimaire(societeId);
        }

        /// <summary>
        /// Renvoie la liste des Ci par liste des codes
        /// </summary>
        /// <param name="codeList">Liste des codes des Cis à renvoyer</param>
        /// <returns>Liste des Cis qui corresponds aux codes demandés</returns>
        public IEnumerable<CIEnt> GetCiByCodeList(IEnumerable<string> codeList)
        {
            if (codeList.IsNullOrEmpty())
            {
                return new List<CIEnt>();
            }
            return Repository.GetCiByCodeList(codeList);
        }

        /// <summary>
        /// Permet d'obtenir la liste des cis generique absence
        /// </summary>
        /// <returns>liste de CIEnt</returns>
        public List<CIEnt> GetCisAbsenceGenerique()
        {
            return Repository.GetCisAbsenceGenerique();
        }

        /// <summary>
        /// Récupére un ci pour l'alimentation Figgo
        /// </summary>
        /// <param name="societeId">societeID</param>
        /// <param name="etablissementComptableId">EtablissementComtableId</param>
        /// <returns>entite Ci</returns>
        public CIEnt GetCIBySocieteIdAndEtablissementIdForFiggo(int societeId, int etablissementComptableId)
        {
            try
            {
                return Repository.Query().Get().FirstOrDefault(x => x.SocieteId == societeId && x.EtablissementComptableId == etablissementComptableId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        #endregion

        #region Gestion des Sociétés

        /// <inheritdoc />
        public SocieteEnt GetSocieteByCIId(int ciId, bool withIncludeTypeSociete = false)
        {
            SocieteEnt ret = null;
            var organisationId = Repository.GetOrganisationIdByCiId(ciId);
            if (organisationId.HasValue)
            {
                ret = societeManager.GetSocieteByOrganisationIdEx(organisationId.Value, withIncludeTypeSociete);
            }

            return ret;
        }


        /// <inheritdoc />
        public EtablissementComptableEnt GetEtablissementComptableByCIId(int ciId)
        {
            EtablissementComptableEnt ret = null;
            var ciEnt = Repository.Get(ciId);
            if (ciEnt != null)
            {
                ret = etabComptaManager.GetEtablissementComptableByOrganisationIdEx(ciEnt.Organisation.OrganisationId);
            }
            return ret;
        }

        #endregion

        #region Gestion des Devises

        /// <inheritdoc />
        public IEnumerable<CIDeviseEnt> GetCIDevise(int ciId)
        {
            return Repository.GetCIDevise(ciId);
        }

        /// <inheritdoc />
        public DeviseEnt GetDeviseRef(int ciId)
        {
            return Repository.GetDeviseRef(ciId);
        }

        /// <inheritdoc />
        public IEnumerable<CIDeviseEnt> ManageCIDevise(int ciId, IEnumerable<CIDeviseEnt> ciDeviseList)
        {
            List<int> existingCIDeviseList = GetCIDevise(ciId).Select(x => x.CiDeviseId).ToList();
            List<int> ciDeviseIdList = ciDeviseList.Select(x => x.CiDeviseId).ToList();
            // Suppresion des relations CIDevise
            if (existingCIDeviseList.Count > 0)
            {
                foreach (int ciDeviseId in existingCIDeviseList)
                {
                    if (!ciDeviseIdList.Contains(ciDeviseId))
                    {
                        Repository.DeleteCIDevise(ciDeviseId);
                    }
                }
            }

            Repository.AddOrUpdateCIDevises(ciDeviseList);
            Save();

            return ciDeviseList;
        }

        /// <inheritdoc />
        public CIDeviseEnt UpdateCIDevise(CIDeviseEnt ciDeviseEnt)
        {
            Repository.UpdateCIDevise(ciDeviseEnt);
            Save();

            return ciDeviseEnt;
        }

        /// <inheritdoc />
        public IEnumerable<DeviseEnt> GetCIDeviseSecList(int ciId)
        {
            return Repository.GetCIDeviseSecList(ciId).ToList();
        }

        /// <summary>
        /// Evalue si le Ci possèdent plusieurs Devises
        /// </summary>
        /// <param name="idCI"> Identifiant du CI </param>
        /// <returns> Vrai si le Ci possède plusieurs Devises, faux sinon </returns>
        public bool IsCiHaveManyDevises(int idCI)
        {
            return Repository.IsCiHaveManyDevises(idCI);
        }

        /// <inheritdoc />
        public override void CheckAccessToEntity(CIEnt entity)
        {
            int userId = userManager.GetContextUtilisateurId();
            CheckAccessToEntity(entity, userId);
        }

        /// <inheritdoc />
        public virtual void CheckAccessToEntity(CIEnt entity, int userId)
        {
            if (entity.Organisation == null)
            {
                Repository.PerformEagerLoading(entity, x => x.Organisation);
            }
            if (entity.Organisation != null)
            {
                List<OrganisationLightEnt> orgaList = userManager.GetOrganisationAvailableByUserAndByTypeOrganisation(userId, entity.Organisation.TypeOrganisationId).ToList();
                if (orgaList.All(o => o.OrganisationId != entity.Organisation.OrganisationId))
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }

        #endregion

        #region Gestion des Ressources

        /// <inheritdoc />
        public IEnumerable<CIRessourceEnt> ManageCIRessource(IEnumerable<CIRessourceEnt> ciRessourceList)
        {
            List<CIRessourceEnt> result = ciRessourceList.ToList();
            if (ciRessourceList?.Any() == true)
            {
                foreach (CIRessourceEnt ciRessource in result)
                {
                    // Suppression de l'association CIRessource si la consommation est à nulle
                    if (!ciRessource.Consommation.HasValue)
                    {
                        Repository.DeleteCIRessource(ciRessource.CiRessourceId);
                    }
                    else
                    {
                        if (ciRessource.CiRessourceId.Equals(0))
                        {
                            Repository.AddCIRessource(ciRessource);
                        }
                        else
                        {
                            Repository.UpdateCIRessource(ciRessource);
                        }
                    }
                }

                Save();
            }

            return result;
        }

        #endregion

        #region Gestion des dates
        /// <inheritdoc/>
        public DateTime? GetDateOuvertureCi(int ciId)
        {
            return Repository.GetDateOuvertureCi(ciId);
        }
        #endregion

        #region Private methodes

        /// <summary>
        /// Récupérer le filtre des permissions pour les CI éligibles à l'affectation des moyens
        /// </summary>
        /// <returns>Le filtre des CI en fonction des permissions de l'utilisateur connecté</returns>
        private CiForMoyenAffectationsRolesFiltersEnt GetCiForMoyenAffectationsRolesFilters()
        {
            CiForMoyenAffectationsRolesFiltersEnt ciForMoyenAffectationsRolesFilters = new CiForMoyenAffectationsRolesFiltersEnt();
            int userId = userManager.GetContextUtilisateurId();
            if (userManager.IsSuperAdmin(userId))
            {
                ciForMoyenAffectationsRolesFilters.IsSuperAdmin = true;
            }
            else if (userManager.HasPermissionToSeeAllCi())
            {
                ciForMoyenAffectationsRolesFilters.HasPermissionToSeeAllCi = true;
            }
            else
            {
                if (userManager.HasPermissionToSeeResponsableCiList())
                {
                    ciForMoyenAffectationsRolesFilters.HasPermissionToSeeResponsableCiList = true;
                    ciForMoyenAffectationsRolesFilters.ResponsableCiList = userManager.GetCiListOfResponsable().Select(c => c.CiId).ToList();
                }

                if (userManager.HasPermissionToSeeDelegueCiList())
                {
                    ciForMoyenAffectationsRolesFilters.HasPermissionToSeeDelegueCiList = true;
                    ciForMoyenAffectationsRolesFilters.DelegueCiList = userManager.GetCiListForDelegue().Select(c => c.CiId).ToList();
                }
            }
            return ciForMoyenAffectationsRolesFilters;
        }

        /// <summary>
        /// Récupère les OrganisationId des Ci affectés à la société
        /// </summary>
        /// <param name="societeId">Id de la société</param>
        /// <param name="includeSep">Inclusion des CI SEP</param>
        /// <param name="fromContratInterim">Inclusion des CI SEP when societe is gerante</param>
        /// <returns>Id organisation des CI</returns>
        private List<int> GetCisOrganisationsIdBySocieteId(int societeId, bool includeSep = true, bool fromContratInterim = false)
        {
            List<int> ciOrganisationIds = new List<int>();
            List<OrganisationBase> cis;

            OrganisationTree mainOrganisationTree = organisationTreeService.GetOrganisationTree();
            cis = mainOrganisationTree.GetAllCisOfSociete(societeId);
            if (cis?.Any() == true)
            {
                ciOrganisationIds = cis.Select(x => x.OrganisationId).ToList();
            }
            //include cis SEP
            if (includeSep)
            {
                List<int> societeSep = fromContratInterim ? sepService.GetSepSocieteParticipantForContratIterimaire(societeId) : sepService.GetSepSocieteParticipant(societeId);
                if (societeSep?.Any() == true)
                {
                    cis.Clear();
                    foreach (int sepAsso in societeSep)
                    {
                        cis = mainOrganisationTree.GetAllCisOfSociete(sepAsso);
                        if (cis?.Any() == true)
                        {
                            ciOrganisationIds.AddRange(cis.Select(x => x.OrganisationId).ToList());
                        }
                    }
                }
            }

            ciOrganisationIds.Sort();
            return ciOrganisationIds;
        }
        #endregion

        /// <summary>
        /// Retourne un ci sans le mettre dans le context(AsNoTracking)
        /// Utile pour faire une comparaison des valeurs de champs.
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <returns>Un ci détaché du contexte</returns>
        public CIEnt GetCiForCompare(int ciId)
        {
            return Repository.Query().Filter(x => x.CiId == ciId).Get().AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Ajoute ou met à jour des CI selon la liste en paramètre
        /// </summary>
        /// <param name="cis">Liste des CI</param>   
        /// <param name="updateOrganisation">Mets aussi a jour l'organisation quand on met a jour le ci</param> 
        /// <returns>Les CI ajoutés</returns>
        public List<CIEnt> AddOrUpdateCIList(List<CIEnt> cis, bool updateOrganisation = false)
        {
            return Repository.AddOrUpdateCIList(cis, updateOrganisation).ToList();
        }

        /// <summary>
        /// Retourne les ci dont le code est contenu dans la liste ciCodes.
        /// ATTENTION: cette methode est a utliser avec precausion.
        /// En effet, le Code ci n'est pas unique, il est unique pour une societe donnée !!!
        /// </summary>
        /// <param name="ciCodes">liste de code ci</param>
        /// <returns>Les ci qui ont le code contenu dans ciCodes </returns>
        public List<CIEnt> GetCisByCodes(List<string> ciCodes)
        {
            return this.Repository.Query().Filter(x => ciCodes.Contains(x.Code)).Get().AsNoTracking().ToList();
        }

        /// <summary>
        /// Retourne une liste de ci en fonction d'une liste ce ciid
        /// </summary>
        /// <param name="ciIds">Liste de ciid</param>
        /// <returns>Liste de ci</returns>
        public List<CIEnt> GetCisByIdsLight(List<int> ciIds)
        {
            return Repository.GetCIsByIdsLight(ciIds).ToList();
        }

        /// <summary>
        /// Retourne une liste de ci en fonction d'une liste ce ciid
        /// </summary>
        /// <param name="ciIds">Liste de ciid</param>
        /// <returns>Liste de ci</returns>
        public List<CIEnt> GetCisByIds(List<int> ciIds)
        {
            return Repository.GetCIsByIds(ciIds).ToList();
        }

        public IReadOnlyList<int> GetCiIdsBySocieteIds(List<int> societeIds)
        {
            return Repository.GetCiIdsBySocieteIds(societeIds).ToList();
        }

        /// <summary>
        /// Insertion de masse des CIDevise
        /// </summary>
        /// <param name="ciDeviseList">Liste de CIDevise</param>
        public void BulkAddCIDevise(IEnumerable<CIDeviseEnt> ciDeviseList)
        {
            this.Repository.BulkAddCIDevise(ciDeviseList);
        }

        /// <summary>
        /// Récupère le ci de passage d'un ci sep ainsi que sa société
        /// </summary>
        /// <param name="ciId">Identifiant unique du ci sep </param>
        /// <returns>Ci de passage avec sa societe inclus</returns>
        public CIEnt GetCICompteInterneByCiid(int ciId)
        {
            return Repository.GetCICompteInterneByCiid(ciId);
        }

        /// <summary>
        /// Handle get CI by personnel and ci affectations
        /// </summary>
        /// <param name="personnelId">personnel identifiant</param>
        /// <param name="ciList">List des Cis</param>
        /// <returns>List des ci</returns>
        private IEnumerable<CIEnt> HandleGetRefPersonnelCiAffectation(int personnelId, IEnumerable<CIEnt> ciList)
        {
            List<CIEnt> ciListResult = new List<CIEnt>();
            foreach (CIEnt ci in ciList)
            {
                if (!ci.IsDisableForPointage || ci.IsDisableForPointage && affectationRepo.GetAffectationByCiAndPersonnel(ci.CiId, personnelId) != null)
                {
                    ciListResult.Add(ci);
                }
            }
            return ciListResult;
        }

        /// <summary>
        /// Get ci for affectation by ci id
        /// </summary>
        /// <param name="ciId">Ci Identifier</param>
        /// <returns>Ci ent</returns>
        public CIEnt GetCiForAffectationByCiId(int ciId)
        {
            return Repository.GetCiForAffectationByCiId(ciId);
        }

        /// <summary>
        /// Récupère le ci par code et l'identifiant de la société 
        /// </summary>
        /// <param name="code">Code CI</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Ci de passage avec sa societe inclus</returns>
        public CIEnt GetCIByCodeAndSocieteId(string code, int societeId)
        {
            return Repository.GetCIByCodeAndSocieteId(code, societeId);
        }
    }
}
