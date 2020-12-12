using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.CI;
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
using Fred.Entities.Utilisateur;
using Fred.Web.Models.CI;

namespace Fred.GroupSpecific.Fes
{
    public class FesCIManager : CIManager
    {
        private readonly IUtilisateurManager userManager;
        private readonly IMapper mapper;

        public FesCIManager(
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
            : base(
                uow,
                ciRepository,
                validator,
                etabComptaManager,
                organisationManager,
                societeManager,
                userManager,
                dateClotureComptableManager,
                organisationTreeService,
                sepService,
                contratInterimaireManager,
                affectationRepo,
                mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        protected override void OnGetFilter(SearchCIEnt searchCi)
        {
            UtilisateurEnt utilisateur = userManager.GetContextUtilisateur();

            if (utilisateur?.Personnel?.Societe?.Groupe?.Code == Constantes.CodeGroupeFES)
            {
                searchCi.CITypeList = GetCITypesFilterValues();
            }
        }

        public override async Task<IEnumerable<CIModel>> SearchLightModelAsync(string text, int page, int pageSize, int? personnelSocieteId = null)
        {
            IEnumerable<CIEnt> cis = await SearchLightAsync(text, page, pageSize, personnelSocieteId).ConfigureAwait(false); ;

            IEnumerable<CIFullLibelleModel> cisModel = mapper.Map<IEnumerable<CIFullLibelleModel>>(cis ?? new List<CIEnt>());

            return cisModel;
        }
    }
}
